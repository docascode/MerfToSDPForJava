namespace Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP
{
    using Newtonsoft.Json;
    using YamlDotNet.Serialization;
    public class TypeParameter
    {
        [JsonProperty("description")]
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [JsonProperty("name")]
        [YamlMember(Alias = "name")]
        public string Name { get; set; }
    }
}
