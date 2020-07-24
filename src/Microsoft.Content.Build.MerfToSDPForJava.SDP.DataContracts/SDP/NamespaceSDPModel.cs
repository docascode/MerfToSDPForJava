namespace Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using YamlDotNet.Serialization;
    public class NamespaceSDPModel: AbstractSDPModelBase
    {
        [JsonIgnore]
        [YamlIgnore]
        public override string YamlMime { get; } = "YamlMime:JavaPackage";

        [JsonProperty("classes")]
        [YamlMember(Alias = "classes")]
        public IEnumerable<string> Classes { get; set; }

        [JsonProperty("enums")]
        [YamlMember(Alias = "enums")]
        public IEnumerable<string> Enums { get; set; }

        [JsonProperty("interfaces")]
        [YamlMember(Alias = "interfaces")]
        public IEnumerable<string> Interfaces { get; set; }
    }
}
