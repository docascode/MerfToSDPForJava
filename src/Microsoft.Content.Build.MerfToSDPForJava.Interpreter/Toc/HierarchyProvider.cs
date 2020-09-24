namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using System.Collections.Concurrent;

    public class HierarchyProvider
    {
        ///Track the newly generated SDP file(Exclude nameSpace). Key is the uid of its parent node, and value is its corresponding TocItemYaml.
        private static ConcurrentDictionary<string, TocItemYaml> tracker_not_nameSpace = new ConcurrentDictionary<string, TocItemYaml>();
        ///Track the newly generated SDP file (Only include nameSpace). 
        private static  ConcurrentQueue<TocItemYaml> tracker_nameSpace = new ConcurrentQueue<TocItemYaml>();

        public void Save(string uid, ArticleItemYaml articleItemYaml)
        {
            if (articleItemYaml == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(uid) && (articleItemYaml.Type == MemberType.Field || articleItemYaml.Type == MemberType.Method || articleItemYaml.Type == MemberType.Constructor))
            {
                if (tracker_not_nameSpace.ContainsKey(uid))
                {
                    var item = tracker_not_nameSpace[uid];

                    if (item.Items == null)
                    {
                        item.Items = new TocYaml();
                    }

                    item.Items.Add(new TocItemYaml
                    {
                        Uid = articleItemYaml.Uid,
                        Name = articleItemYaml.Name.RemoveFromValue("("),
                        Type = articleItemYaml.Type?.ToString().ToLower()
                    });
                }
            }
            else if (articleItemYaml.Type == MemberType.Namespace)
            {
                if (articleItemYaml == null )
                {
                    return;
                }

                var tocItem = new TocItemYaml()
                {
                    Uid = articleItemYaml.Uid,
                    Name = articleItemYaml.Name.RemoveFromValue("("),
                    Items = new TocYaml()
                };

                articleItemYaml.Children?.ForEach(item => { tocItem.Items.Add(new TocItemYaml() { Uid = item });});
                tracker_nameSpace.Enqueue(tocItem);
            }
            else
            {
                var tocItem = new TocItemYaml
                {
                    Uid = articleItemYaml.Uid,
                    Name = articleItemYaml.Name.RemoveFromValue("(")
                };

                tracker_not_nameSpace.AddOrUpdate(articleItemYaml.Uid, tocItem, (key, oldValue) =>
                {
                    oldValue = tocItem;
                    return oldValue;
                });
            }
        }

        public TocYaml BuildToc()
        {
            TocYaml tocItemYamls = new TocYaml();
            foreach (var item in tracker_nameSpace)
            {
                BuildChild(item);
                tocItemYamls.Add(item);
            }

            return tocItemYamls;
        }

        private void BuildChild(TocItemYaml tocItemYamls)
        {
            if (tocItemYamls.Items != null)
            {
                var tempTocYaml = new TocYaml();
                foreach (var item in tocItemYamls.Items)
                {
                    TocItemYaml temp;
                    if (tracker_not_nameSpace.TryGetValue(item.Uid, out temp))
                    {
                        tempTocYaml.Add(temp);
                    }
                }
                
                tocItemYamls.Items = tempTocYaml;
            }
        }
    }
}
