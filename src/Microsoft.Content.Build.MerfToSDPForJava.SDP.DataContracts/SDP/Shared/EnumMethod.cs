namespace Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using YamlDotNet.Serialization;
    public class EnumMethod
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

        [JsonProperty("exceptions")]
        [YamlMember(Alias = "exceptions")]
        public IEnumerable<ExceptionType> Exceptions { get; set; }

        [JsonProperty("returns")]
        [YamlMember(Alias = "returns")]
        public Return Returns { get; set; }

        [JsonProperty("summary")]
        [YamlMember(Alias = "summary")]
        public string Summary { get; set; }

        [JsonProperty("syntax")]
        [YamlMember(Alias = "syntax")]
        public string Syntax { get; set; }

        [JsonProperty("uid")]
        [YamlMember(Alias = "uid")]
        public string Uid { get; set; }
    }
}
