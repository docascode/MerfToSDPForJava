namespace Microsoft.Content.Build.MerfToSDPForJava.Common
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading;
    using YamlDotNet.Serialization;

    public static class YamlUtility
    {
        private static readonly ThreadLocal<ISerializer> serializer
            = new ThreadLocal<ISerializer>(() =>
            new SerializerBuilder()
            .DisableAliases()
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
            .WithEventEmitter(next => new StringQuotingEmitter(next))
            .Build());

        private static readonly ThreadLocal<IDeserializer> deserializer
            = new ThreadLocal<IDeserializer>(() => new DeserializerBuilder().IgnoreUnmatchedProperties().Build());

        public static void Serialize(TextWriter writer, object graph, string comments)
        {
            if (!string.IsNullOrEmpty(comments))
            {
                foreach (var comment in comments.Split('\n'))
                {
                    writer.Write("### ");
                    writer.WriteLine(comment.TrimEnd('\r'));
                }
            }
            serializer.Value.Serialize(writer, graph);
        }

        public static void Serialize(string path, object graph, string comments)
        {
            using (var writer = new StreamWriter(path))
            {
                Serialize(writer, graph, comments);
            }
        }

        public static T Deserialize<T>(TextReader reader)
        {
            return deserializer.Value.Deserialize<T>(reader);
        }

        public static string EncodeXrefLink(string uid)
        {
            return $"<xref href=\"{uid}\" data-throw-if-not-resolved=\"False\"/>";
        }

        public static string EncodeXrefLink(string text, string uid, string altText = null)
        {
            return $"<xref href=\"{uid ?? UrlEncodeLinkText(text)}?alt={UrlEncodeLinkText(altText) ?? UrlEncodeLinkText(uid)}&text={UrlEncodeLinkText(text)}\" data-throw-if-not-resolved=\"False\"/>";
        }
        private static string UrlEncodeLinkText(string text)
        {
            return WebUtility.UrlEncode(text);
        }

        public static string EncodeXrefEmptyUid(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return text;
            }
            else
            {
                string pattern = @"(?<start><xref uid="""" data-throw-if-not-resolved=""false"")>(?<name>\S+)(?<end><\/xref>)";

                string replaced = Regex.Replace(text, pattern, m =>
                                        m.Groups["start"].Value + " data-raw-source=\"" 
                                        + UrlEncodeLinkText(m.Groups["name"].Value) + "\">" + m.Groups["end"].Value);

                return replaced;
            }
        }
    }
}
