namespace AirHttp.Uri.Extensions
{
    public static class UriBuilderExtensions
    {
        public static UriBuilder AddHttp(this string uri)
        {
            var builder = new UriBuilder(uri);
            builder.AddHttp();
            return builder;
        }

        public static UriBuilder AddHttps(this string uri)
        {
            var builder = new UriBuilder(uri);
            builder.AddHttps();
            return builder;
        }

        public static UriBuilder AddWWW(this string uri)
        {
            var builder = new UriBuilder(uri);
            builder.AddWWW();
            return builder;
        }

        public static UriBuilder AddSegment(this string uri, string segment)
        {
            var builder = new UriBuilder(uri);
            builder.AddSegment(segment);
            return builder;
        }

        public static UriBuilder AddParam(this string uri, string name, string value)
        {
            var builder = new UriBuilder(uri);
            builder.AddParam(name, value);
            return builder;
        }
    }

}