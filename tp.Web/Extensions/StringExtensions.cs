namespace tp.Web
{
    static public class StringExtensions
    {

        /// <summary>
        /// Converts a string to an HTML-encoded string.
        /// </summary>
        static public string HtmlEncode(this string Text)
        {
            
            return string.IsNullOrWhiteSpace(Text) ? Text : WebUtility.HtmlEncode(Text);
        }
        /// <summary>
        /// Converts a string that has been HTML-encoded for HTTP transmission into a decoded string
        /// </summary>
        static public string HtmlDecode(this string Text)
        {
            return string.IsNullOrWhiteSpace(Text) ? Text : WebUtility.HtmlDecode(Text);
        }
        /// <summary>
        /// Converts a string to an Url-encoded string.
        /// </summary>
        static public string UrlEncode(this string Text)
        {
            return string.IsNullOrWhiteSpace(Text) ? Text : WebUtility.UrlEncode(Text);
        }
        /// <summary>
        /// Converts a string that has been Url-encoded into a decoded string
        /// </summary>
        static public string UrlDecode(this string Text)
        {
            return string.IsNullOrWhiteSpace(Text) ? Text : WebUtility.UrlDecode(Text);
        }
    }
}
