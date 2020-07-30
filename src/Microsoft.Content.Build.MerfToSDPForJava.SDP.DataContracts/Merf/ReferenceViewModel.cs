﻿namespace Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference
{
    using System;
    using System.Collections.Generic;
    using YamlDotNet.Core;
    using YamlDotNet.Serialization;

    [Serializable]
    public class ReferenceViewModel
    {
        [YamlMember(Alias = "uid")]
        public string Uid { get; set; }

        [YamlMember(Alias = "parent")]
        public string Parent { get; set; }

        [YamlMember(Alias = "definition")]
        public string Definition { get; set; }

        [YamlMember(Alias = "isExternal")]
        public bool? IsExternal { get; set; }

        [YamlMember(Alias = "href")]
        public string Href { get; set; }

        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "nameWithType")]
        public string NameWithType { get; set; }

        [YamlMember(Alias = "fullName")]
        public string FullName { get; set; }

        [YamlMember(Alias = "type")]
        public MemberType? Type { get; set; }

        [YamlMember(Alias = "spec.cplusplus")]
        public List<SpecViewModel> SpecForCpp { get; set; }

        [YamlMember(Alias = "spec.java")]
        public List<SpecViewModel> SpecForJava { get; set; }

        [YamlMember(Alias = "summary", ScalarStyle = ScalarStyle.DoubleQuoted)]
        public string Summary { get; set; }

        [YamlMember(Alias = "syntax")]
        public SyntaxDetailViewModel Syntax { get; set; }

        [YamlMember(Alias = "package")]
        public string PackageName { get; set; }
    }
}
