﻿namespace Microsoft.Content.Build.MerfToSDPForJava.DataContracts
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
