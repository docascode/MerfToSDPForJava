namespace Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP
{
    using Newtonsoft.Json;
    using YamlDotNet.Serialization;
    public class ExceptionType
    {
        [JsonProperty("type")]
        [YamlMember(Alias = "type")]
        public string Type { get; set; }

        [JsonProperty("description")]
        [YamlMember(Alias = "description")]
        public string Description { get; set; }
    }

}
