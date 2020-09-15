namespace Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP
{
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using YamlDotNet.Serialization;
    public class TypeSDPModel : AbstractSDPModelBase
    {
        [JsonIgnore]
        [YamlIgnore]
        public override string YamlMime { get; } = "YamlMime:JavaType";


        /// <summary>
        /// A collection of reference to the constructors defined in this type.
        /// </summary>
        [JsonProperty("constructors")]
        [YamlMember(Alias = "constructors")]
        public IEnumerable<string> Constructors { get; set; }

        [JsonProperty("fields")]
        [YamlMember(Alias = "fields")]
        public IEnumerable<string> Fields { get; set; }

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
        public IEnumerable<string> Methods { get; set; }

        [JsonProperty("nameWithType")]
        [YamlMember(Alias = "nameWithType")]
        public string NameWithType { get; set; }

        [JsonProperty("syntax")]
        [YamlMember(Alias = "syntax")]
        public string Syntax { get; set; }

        [JsonProperty("type")]
        [YamlMember(Alias = "type")]
        public string Type { get; set; }

        [JsonProperty("typeParameters")]
        [YamlMember(Alias = "typeParameters")]
        public IEnumerable<TypeParameter> TypeParameters { get; set; }
    }
}
