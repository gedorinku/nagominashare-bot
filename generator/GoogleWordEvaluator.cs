using System.IO;
using System.Json;
using System.Net;

namespace Nagominashare
{
    class GoogleWordEvaluator : WordEvaluator
    {
        public override long Evaluate(string word)
        {
            const string key = Variables.GoogleApiKey;
            const string cx = Variables.GoogleCustomSearchCx;
            var url = $"https://www.googleapis.com/customsearch/v1?key={key}&cx={cx}&q={word}";
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Timeout = 1000 * 10;
            request.Method = "GET";

            WebResponse response = request.GetResponse();
            var responseStream = response.GetResponseStream();

            using (var sr = new StreamReader(responseStream))
            {
                var raw = sr.ReadToEnd();
                return ParseResponse(raw);
            }
        }

        private static long ParseResponse(string raw)
        {
            var json = JsonValue.Parse(raw);
            var rawTotalResults =
                json["searchInformation"]["totalResults"].ToString().Replace("\"", "");
            return long.Parse(rawTotalResults);
        }
    }
}