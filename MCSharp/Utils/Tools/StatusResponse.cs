using MCSharp.Pakets;
using MCSharp.Pakets.Client.Status;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCSharp.Utils.Tools
{
    public class StatusResponse
    {
        internal readonly StatusResponseObject generator = new StatusResponseObject();
        public StatusResponse()
        {
            SetVersionName("1.18.1");
            SetProtocolVersion(757);
            SetMaxPlayers(100);
            SetOnlinePlayers(0);
            SetMotd("\u00a74A Minecraft Server\u00a76 -\u00a7a With C#");
            SetIcon(Properties.Resources.DefaultMcsharpIcon);
        }
        public StatusResponse SetVersionName(string name)
        {
            generator.Version.Name = name;
            return this;
        }
        public StatusResponse SetProtocolVersion(long version)
        {
            generator.Version.Protocol = version;
            return this;
        }
        public StatusResponse SetMaxPlayers(long max)
        {
            generator.Players.Max = max;
            return this;
        }
        public StatusResponse SetOnlinePlayers(long online)
        {
            generator.Players.Online = online;
            return this;
        }
        public StatusResponse AddSamplePlayer(string username)
        {
            var nl = generator.Players.Sample.ToList();
            nl.Add(new Sample()
            {
                Id = Guid.NewGuid(),
                Name = username,
            });
            generator.Players.Sample = nl.ToArray();
            return this;
        }
        public StatusResponse SetMotd(string motd)
        {
            generator.Description.Text = motd;
            return this;
        }
        public StatusResponse SetIcon(byte[] icon)
        {
            return SetIcon(Convert.ToBase64String(icon));
        }
        public StatusResponse SetIcon(string base64)
        {
            generator.Favicon = "data:image/png;base64," + base64;
            return this;
        }
        public IPaket GetPaket()
        {
            StatusResponsePaket srp = new StatusResponsePaket()
            {
                Status = JsonConvert.SerializeObject(generator),
            };
            return srp;
        }
    }
    internal partial class StatusResponseObject
    {
        [JsonProperty("version")]
        public Version Version { get; set; } = new Version();

        [JsonProperty("players")]
        public Players Players { get; set; } = new Players();

        [JsonProperty("description")]
        public Description Description { get; set; } = new Description();

        [JsonProperty("favicon")]
        public string Favicon { get; set; }
    }

    internal partial class Description
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    internal partial class Players
    {
        [JsonProperty("max")]
        public long Max { get; set; }

        [JsonProperty("online")]
        public long Online { get; set; }

        [JsonProperty("sample")]
        public Sample[] Sample { get; set; }
    }

    internal partial class Sample
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }
    }

    internal partial class Version
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("protocol")]
        public long Protocol { get; set; }
    }
}