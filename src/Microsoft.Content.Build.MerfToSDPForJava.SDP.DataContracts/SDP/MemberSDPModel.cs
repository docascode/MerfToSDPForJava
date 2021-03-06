﻿namespace Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP
{
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using YamlDotNet.Serialization;
    public class MemberSDPModel:AbstractSDPModelBase
    {
        [JsonIgnore]
        [YamlIgnore]
        public override string YamlMime { get; } = "YamlMime:JavaMember";

        [JsonProperty("nameWithType")]
        [YamlMember(Alias = "nameWithType")]
        public string NameWithType { get; set; }

        [JsonProperty("type")]
        [YamlMember(Alias = "type")]
        public string Type{ get; set; }

        [JsonProperty("members")]
        [YamlMember(Alias = "members")]
        public List<Member> Members { get; set; }
    }
}
