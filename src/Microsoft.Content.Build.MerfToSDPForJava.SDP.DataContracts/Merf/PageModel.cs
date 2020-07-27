namespace Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference
{
    using System;
    using System.Collections.Generic;
    using YamlDotNet.Serialization;

    [Serializable]
    public class PageModel
    {
        [YamlMember(Alias = "items")]
        public List<ArticleItemYaml> Items { get; set; } = new List<ArticleItemYaml>();

        [YamlMember(Alias = "metadata")]
        public Dictionary<string, object> Metadata { get; set; }
    }
}
