using System;
using System.Net;
using System.Linq;

namespace UW.Web.Services.GWSClient.Utils
{
    static class ClientUtilities
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Convert a UNIX timestamp (ms) into a local DateTime object
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromUnixTimestamp(long timestamp)
        {
            return epoch.AddMilliseconds(timestamp).ToLocalTime();
        }
    }

    public static class Enums
    {
        /// <summary>
        /// Performs a strict TryParse, failing if the resulting Enum variant is not a member of the target type.
        /// This is the desired behavior of Enum.TryParse.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public static bool TryStrictParse<T>(string val, out T res) where T : struct
        {
            return Enum.TryParse(val, ignoreCase: true, out res) && Enum.IsDefined(typeof(T), res);
        }

        /// <summary>
        /// Specialized TryStrictParse for HttpStatusCodes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public static bool TryStrictParse<T>(HttpStatusCode code, out T res) where T : struct
        {
            var val = code.DowncastString();
            return TryStrictParse<T>(val, out res);
        }

        internal static string DowncastString(this HttpStatusCode code) => ((int)code).ToString();
    }

    public static class UriExtensions
    {
        public static Uri Append(this Uri uri, params string[] paths)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            var filtered = paths.Where(p => !string.IsNullOrWhiteSpace(p));

            return new Uri(filtered.Aggregate(uri.AbsoluteUri, (current, path) =>
            {
                return $"{current.TrimEnd('/')}/{path.TrimStart('/')}";
            }));
        }

        public static Uri Params(this Uri uri, string query)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));
            if (string.IsNullOrWhiteSpace(query))
            {
                return uri;
            }

            return new Uri($"{uri.AbsoluteUri.TrimEnd('?')}?{query.TrimStart('?')}");
        }

        public static bool Any(this HttpStatusCode code, params HttpStatusCode[] codes)
        {
            return codes.Any(c => c == code);
        }

        public static bool Any(this HttpStatusCode code, params int[] codes)
        {
            return codes.Any(c => c == (int)code);
        }
    }
}
