﻿namespace Microsoft.Content.Build.MerfToSDPForJava.DataContracts
{
    using System.Collections.Generic;

    public class ServiceMapping : List<ServiceMappingItem>
    {
        public ServiceMapping(IEnumerable<ServiceMappingItem> items) : base(items)
        {
        }

        public ServiceMapping() : base()
        {
        }
    }
}
