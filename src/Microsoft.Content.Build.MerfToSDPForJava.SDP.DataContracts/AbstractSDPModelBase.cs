namespace Microsoft.Content.Build.MerfToSDPForJava.SDP.DataContracts
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using YamlDotNet.Serialization;
    public abstract class AbstractSDPModelBase
    {
        public AbstractSDPModelBase()
        {
            Metadata = new Dictionary<string, object>();
        }

        [JsonIgnore]
        [YamlIgnore]
        abstract public string YamlMime { get; }

        [JsonProperty("uid")]
        [YamlMember(Alias = "uid")]
        public string Uid { get; set; }

        [JsonProperty("artifact")]
        [YamlMember(Alias = "artifact")]
        public string Artifact { get; set; }

        [JsonProperty("fullName")]
        [YamlMember(Alias = "fullName")]
        public string FullName { get; set; }

        [JsonProperty("name")]
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [JsonProperty("package")]
        [YamlMember(Alias = "package")]
        public string Package { get; set; }

        [JsonProperty("summary")]
        [YamlMember(Alias = "summary")]
        public string Summary { get; set; }

        [JsonProperty("metadata")]
        [YamlMember(Alias = "metadata")]
        public Dictionary<string, object> Metadata { get; set; }
    }
}
