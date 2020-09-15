namespace Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using YamlDotNet.Serialization;
    public class EnumSDPModel:AbstractSDPModelBase
    {
        [JsonIgnore]
        [YamlIgnore]
        public override string YamlMime { get; } = "YamlMime:JavaEnum";

        [JsonProperty("fields")]
        [YamlMember(Alias = "fields")]
        public IEnumerable<EnumField> Fields { get; set; }

        [JsonProperty("implements")]
        [YamlMember(Alias = "implements")]
        public IEnumerable<string> Implements { get; set; }

        [JsonProperty("inheritances")]
        [YamlMember(Alias = "inheritances")]
        public IEnumerable<string> Inheritances { get; set; }

        [JsonProperty("inheritedMembers")]
        [YamlMember(Alias = "inheritedMembers")]
        public IEnumerable<string> InheritedMembers { get; set; }

        [JsonProperty("methods")]
        [YamlMember(Alias = "methods")]
        public IEnumerable<EnumMethod> Methods { get; set; }

        [JsonProperty("nameWithType")]
        [YamlMember(Alias = "nameWithType")]
        public string NameWithType { get; set; }

        [JsonProperty("syntax")]
        [YamlMember(Alias = "syntax")]
        public string Syntax { get; set; }
    }
}
