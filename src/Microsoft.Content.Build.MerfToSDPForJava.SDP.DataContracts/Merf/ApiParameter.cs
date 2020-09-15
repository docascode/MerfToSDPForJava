﻿namespace Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference
{
    using System;
    using YamlDotNet.Serialization;

    [Serializable]
    public class ApiParameter
    {
        [YamlMember(Alias = "id")]
        public string Name { get; set; }

        [YamlMember(Alias = "type")]
        public string Type { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }
    }
}
