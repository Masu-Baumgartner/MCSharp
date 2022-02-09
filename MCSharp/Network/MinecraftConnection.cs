using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using MCSharp.Enums;

using Logging.Net;
using MCSharp.Pakets;
using Org.BouncyCastle.Bcpg;
using System.IO;
using Ionic.Zlib;
using MCSharp.Pakets.Client.Login;
using Newtonsoft.Json.Linq;

namespace MCSharp.Network
{
    public class MinecraftConnection
    {
        private TcpClient Tcp { get; set; }
        public MinecraftFlow Flow { get; private set; }
        public MinecraftState State { get; set; }
        public MinecraftStream ReadStream { get; private set; }
        public MinecraftStream WriteStream { get; private set; }
        public CancellationToken CancellationToken { get; set; }

        public Queue<PacketQueueItem> PaketQueue { get; set; } = new Queue<PacketQueueItem>();
        public PaketRegistry WriterRegistry { get; set; }
        public PaketRegistry ReaderRegistry { get; set; }

        public Thread ReadThread { get; private set; }
        public Thread WriteThread { get; private set; }

        public bool CompressionEnabled { get; set; }
        public int CompressionThreshold { get; set; } = 256 ;

        public IPaketHandler Handler { get; set; }

        private int _lastReceivedPacketId;
        private int _lastSentPacketId;

        public MinecraftConnection(TcpClient tcp)
        {
            Tcp = tcp;

            Flow = MinecraftFlow.ClientToServer;
            State = MinecraftState.Handshaking;

            ReadStream = new MinecraftStream(Tcp.GetStream(), CancellationToken.None);
            WriteStream = new MinecraftStream(Tcp.GetStream(), CancellationToken.None);
        }

        public void Start()
        {
            ReadThread = new Thread(ProcessNetworkRead);
            WriteThread = new Thread(ProcessNetworkWrite);

            ReadThread.Name = "MCSharp - Read";
            WriteThread.Name = "MCSharp - Write";

            ReadThread.Start();
            WriteThread.Start();
        }

        public MinecraftConnection(TcpClient tcp, MinecraftFlow flow) : this(tcp)
        {
            Flow = flow;

            State = MinecraftState.Handshaking;

            ReadStream = new MinecraftStream(Tcp.GetStream(), CancellationToken.None);
            WriteStream = new MinecraftStream(Tcp.GetStream(), CancellationToken.None);
        }

        public MinecraftConnection(TcpClient tcp, MinecraftFlow flow, MinecraftState state, CancellationToken cancellationToken) : this(tcp, flow)
        {
            State = state;
            ReadStream = new MinecraftStream(Tcp.GetStream(), CancellationToken.None);
            WriteStream = new MinecraftStream(Tcp.GetStream(), CancellationToken.None);
            CancellationToken = cancellationToken;
        }

        public void EnableEncryption(byte[] key)
        {
            WriteStream.InitEncryption(key);
            ReadStream.InitEncryption(key);
        }

        public void ProcessNetworkRead()
        {
            try
            {
                SpinWait sw = new SpinWait();

                while (!CancellationToken.IsCancellationRequested)
                {
                    if (CancellationToken.IsCancellationRequested)
                        break;

                    if (ReadStream.DataAvailable)
                    {
                        IPaket paket = TryReadPaket(out var lastPaketId);
                        _lastReceivedPacketId = lastPaketId;

                        if (paket != null && Handler != null)
                        {
                            switch (State)
                            {
                                case MinecraftState.Handshaking:
                                    Handler.Handshake(paket);
                                    break;
                                case MinecraftState.Status:
                                    Handler.Status(paket);
                                    break;
                                case MinecraftState.Login:
                                    Handler.Login(paket);
                                    break;
                                case MinecraftState.Play:
                                    Handler.Play(paket);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        sw.SpinOnce();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Error reading paket: (State {State}) {e.Message}");
            }
        }

        public void ProcessNetworkWrite()
        {
            try
            {
                SpinWait sw = new SpinWait();

                while (!CancellationToken.IsCancellationRequested)
                {
                    if (CancellationToken.IsCancellationRequested)
                        break;

                    IPaket toSend = null;
                    MinecraftState state = MinecraftState.Handshaking;

                    lock (PaketQueue)
                    {
                        if (PaketQueue.Count > 0)
                        {
                            var mcs = PaketQueue.Dequeue();
                            toSend = mcs.Paket;
                            state = mcs.State;
                        }
                    }

                    if (toSend != null)
                    {
                        byte[] data = EncodePaket(toSend, state);

                        WriteStream.WriteVarInt(data.Length);
                        WriteStream.Write(data);

                        if (toSend is SetCompressionPaket)
                        {
                            CompressionEnabled = true;
                        }
                    }

                    sw.SpinOnce();
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Error writing paket: (State: {State}) {e.Message}");
            }
        }

        public byte[] EncodePaket(IPaket paket, MinecraftState state)
        {
            byte[] encodedPacket;

            using (MemoryStream ms = new MemoryStream())
            {
                using (MinecraftStream mc = new MinecraftStream(ms, CancellationToken))
                {
                    mc.WriteVarInt(WriterRegistry.GetPaketId(paket, state));
                    paket.Encode(mc);
                }

                encodedPacket = ms.ToArray();
            }

            if (CompressionEnabled)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (MinecraftStream mc = new MinecraftStream(ms, CancellationToken))
                    {

                        if (encodedPacket.Length >= CompressionThreshold)
                        {
                            //byte[] compressed;
                            //CompressData(encodedPacket, out compressed);

                            mc.WriteVarInt(encodedPacket.Length);

                            using (ZlibStream outZStream = new ZlibStream(
                                mc, CompressionMode.Compress, true))
                            {
                                outZStream.Write(encodedPacket, 0, encodedPacket.Length);
                            }
                            //mc.Write(compressed);
                        }
                        else //Uncompressed
                        {
                            mc.WriteVarInt(0);
                            mc.Write(encodedPacket);
                        }
                    }

                    encodedPacket = ms.ToArray();
                }
            }

            return encodedPacket;
        }

        public IPaket TryReadPaket(out int lastPaketId)
        {
            IPaket result = null;

            int packetId = 0;
            byte[] data = new byte[0];

            if (!CompressionEnabled)
            {
                int length = ReadStream.ReadVarInt();

                int packetIdLength;
                packetId = ReadStream.ReadVarInt(out packetIdLength);

                _lastReceivedPacketId = lastPaketId = packetId;

                if (length - packetIdLength > 0)
                {
                    data = ReadStream.Read(length - packetIdLength);
                }
            }
            else
            {
                int packetLength = ReadStream.ReadVarInt();

                int br;
                int dataLength = ReadStream.ReadVarInt(out br);

                int readMore;

                if (dataLength == 0)
                {
                    packetId = ReadStream.ReadVarInt(out readMore);
                    _lastReceivedPacketId = lastPaketId = packetId;
                    data = ReadStream.Read(packetLength - (br + readMore));
                }
                else
                {
                    var data2 = ReadStream.Read(packetLength - br);

                    using (MinecraftStream a = new MinecraftStream(CancellationToken))
                    {
                        using (ZlibStream outZStream = new ZlibStream(
                            a, CompressionMode.Compress, true))
                        {
                            outZStream.Write(data2);
                            //  outZStream.Write(data, 0, data.Length);
                        }

                        a.Seek(0, SeekOrigin.Begin);

                        int l;
                        packetId = a.ReadVarInt(out l);
                        _lastReceivedPacketId = lastPaketId = packetId;
                        data = a.Read(dataLength - l);
                    }
                }
            }

            //Logger.Debug(BitConverter.GetBytes(packetId)[0].ToString());

            if (ReaderRegistry.Pakets[State].ContainsKey((byte)packetId))
            {
                Type packetType = ReaderRegistry.Pakets[State][(byte)packetId].GetType();
                result = (IPaket)packetType.GetConstructors()[0].Invoke(new object[0]);

                using (var memoryStream = new MemoryStream(data))
                {
                    using (MinecraftStream minecraftStream = new MinecraftStream(memoryStream, CancellationToken))
                    {
                        result.Decode(minecraftStream);
                    }
                }

                if (result is SetCompressionPaket setCompressionPaket)
                {
                    CompressionThreshold = setCompressionPaket.Threshold;
                    CompressionEnabled = true;
                }

            }
            else
                Logger.Warn("Unimplemented paket: " + (byte)packetId);

            return result;
        }

        public void SendPaket(IPaket paket)
        {
            lock (PaketQueue)
            {
                PaketQueue.Enqueue(new PacketQueueItem()
                {
                    State = State,
                    Paket = paket
                });
            }
        }
    }
}
