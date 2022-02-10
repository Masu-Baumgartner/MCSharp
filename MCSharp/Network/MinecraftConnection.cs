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
using System.Data;

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

        public bool CompressionEnabled { get; set; } = false;
        public int CompressionThreshold { get; set; } = 256;

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
                        IPaket paket = TryReadPacket(ReadStream, out var lastPaketId);
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
                    int id = WriterRegistry.GetPaketId(paket, state);
                    mc.WriteVarInt(id);
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

        public IPaket TryReadPacket(MinecraftStream stream, out int lastPacketId)
        {
            IPaket paket = null;
            int paketId = -1;
            byte[] paketData;

            if (!CompressionEnabled)
            {
                //Logger.Debug("Using default reader");

                int lenght = stream.ReadVarInt();
                int paketIdLenght = -1;
                paketId = stream.ReadVarInt(out paketIdLenght);

                int dataLenght = lenght - paketIdLenght;

                if (dataLenght > 0)
                {
                    paketData = stream.Read(dataLenght);
                }
                else
                {
                    paketData = new byte[0];
                    //Logger.Debug("Empty paket");
                }
            }
            else
            {
                int paketLenght = stream.ReadVarInt();
                int dataLenghtLenght;
                int dataLenght = stream.ReadVarInt(out dataLenghtLenght);

                if (dataLenght == 0)
                {
                    //Logger.Debug("Using reader without compressing -> threshold");

                    int paketIdLenght;
                    paketId = stream.ReadVarInt(out paketIdLenght);
                    paketData = stream.Read(paketLenght - dataLenghtLenght - paketIdLenght);
                }
                else
                {
                    //Logger.Debug("Using compress reader");

                    var cache = stream.Read(paketLenght - dataLenghtLenght);

                    using (MinecraftStream a = new MinecraftStream(CancellationToken))
                    {
                        using (ZlibStream zstream = new ZlibStream(a, CompressionMode.Decompress, true))
                        {
                            zstream.Write(cache);
                        }

                        a.Seek(0, SeekOrigin.Begin);

                        int paketIdLenght;
                        paketId = a.ReadVarInt(out paketIdLenght);

                        int dataSize = paketLenght - dataLenghtLenght - paketIdLenght;
                        paketData = a.Read(dataSize);
                    }
                }
            }

            lastPacketId = paketId;

            if (ReaderRegistry.Pakets[State].ContainsKey((byte)paketId))
            {
                paket = ReaderRegistry.Pakets[State][(byte)paketId];
            }

            //Logger.Info($"Got packet: {paket} (0x{paketId:X2})");

            try
            {
                if (paket == null)
                {
                    //Logger.Warn("Unknown paket: " + paketId);

                    return null;
                }

                //Logger.Debug($"Received {paket}");

                using (var memoryStream = new MemoryStream(paketData))
                {
                    using (MinecraftStream minecraftStream = new MinecraftStream(memoryStream, CancellationToken))
                    {
                        paket.Decode(minecraftStream);
                    }
                }

                if(paket is SetCompressionPaket setCompressionPaket)
                {
                    CompressionThreshold = setCompressionPaket.Threshold;
                    CompressionEnabled = true;
                }

                return paket;
            }
            catch (Exception e)
            {
                Logger.Error("WTF: " + e.Message);
                return null;
            }
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
