namespace Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference
{
    using System;

    using YamlDotNet.Serialization;

    [Serializable]
    public class TocItemYaml
    {
        [YamlMember(Alias = "uid")]
        public string Uid { get; set; }

        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "type")]
        public string Type { get; set; }

        [YamlMember(Alias = "href")]
        [YamlIgnore]
        public string Href { get; set; }

        [YamlMember(Alias = "originalHref")]
        public string OriginalHref { get; set; }

        [YamlMember(Alias = "homepage")]
        public string Homepage { get; set; }

        [YamlMember(Alias = "items")]
        public TocYaml Items { get; set; }
    }
}
