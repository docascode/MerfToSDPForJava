namespace Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP
{
    using Newtonsoft.Json;
    using YamlDotNet.Serialization;
    public class ExceptionType
    {
        [JsonProperty("name")]
        [YamlMember(Alias = "name")]
        public string Type { get; set; }
    }
}
