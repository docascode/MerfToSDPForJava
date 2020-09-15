namespace Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class TocYaml
        : List<TocItemYaml>
    {
        public TocYaml(IEnumerable<TocItemYaml> items) : base(items) { }
        public TocYaml() : base() { }
    }
}
