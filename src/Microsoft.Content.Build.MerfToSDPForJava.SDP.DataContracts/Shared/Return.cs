namespace Microsoft.Content.Build.MerfToSDPForJava.SDP.DataContracts
{
    using Newtonsoft.Json;
    using YamlDotNet.Serialization;
    public class Return
    {
        [JsonProperty("description")]
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        [YamlMember(Alias = "type")]
        public string Type { get; set; }
    }
}
