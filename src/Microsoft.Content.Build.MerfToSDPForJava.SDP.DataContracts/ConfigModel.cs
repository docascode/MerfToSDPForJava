namespace Microsoft.Content.Build.MerfToSDPForJava.SDP.DataContracts
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using Microsoft.Content.Build.MerfToSDPForJava.Constants;

    [Serializable]
    public class ConfigModel
    {
        [JsonProperty(Constants.InputPaths, Required = Required.DisallowNull)]
        public List<string> InputPaths { get; set; }

        [JsonProperty(Constants.OutputPath, Required = Required.DisallowNull)]
        public string OutputPath { get; set; }

        [JsonProperty(Constants.Language)]
        public string Language { get; set; } = "java";
    }
}
