namespace Microsoft.Content.Build.MerfToSDPForJava.SDP.DataContracts
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using YamlDotNet.Serialization;
    public class Member
    {
        [JsonProperty("fullName")]
        [YamlMember(Alias = "fullName")]
        public string FullName { get; set; }
        [JsonProperty("name")]
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [JsonProperty("nameWithType")]
        [YamlMember(Alias = "nameWithType")]
        public string NameWithType { get; set; }

        [JsonProperty("overridden")]
        [YamlMember(Alias = "overridden")]
        public string Overridden { get; set; }

        [JsonProperty("parameters")]
        [YamlMember(Alias = "parameters")]
        public IEnumerable<Parameter> Parameters { get; set; }

        [JsonProperty("interfaces")]
        [YamlMember(Alias = "interfaces")]
        public IEnumerable<Return> Returns { get; set; }

        [JsonProperty("summary")]
        [YamlMember(Alias = "summary")]
        public string Summary { get; set; }

        [JsonProperty("syntax")]
        [YamlMember(Alias = "syntax")]
        public string Syntax { get; set; }

        [JsonProperty("typeParameters")]
        [YamlMember(Alias = "typeParameters")]
        public IEnumerable<TypeParameter> TypeParameters { get; set; }

        [JsonProperty("uid")]
        [YamlMember(Alias = "uid")]
        public string Uid { get; set; }
    }
}
