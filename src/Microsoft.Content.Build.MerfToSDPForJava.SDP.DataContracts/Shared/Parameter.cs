namespace Microsoft.Content.Build.MerfToSDPForJava.SDP.DataContracts
{
    using Newtonsoft.Json;
    using YamlDotNet.Serialization;
    public class Parameter
    {
        [JsonProperty("description")]
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [JsonProperty("name")]
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        [YamlMember(Alias = "type")]
        public string Type { get; set; }

    }

}
