using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace Nagominashare.SentenceParsing
{
    class SentenceParser
    {
        private static readonly TaskFactory<IEnumerable<IWord>> ParserTaskFactory =
            new TaskFactory<IEnumerable<IWord>>();

        public Task<IEnumerable<IWord>> ExtractWords(string sentences)
        {
            return ParserTaskFactory.StartNew(() => ParseSentence(sentences));
        }

        private static IEnumerable<IWord> ParseSentence(string sentence)
        {
            if (string.IsNullOrEmpty(sentence))
            {
                return new List<IWord>();
            }

            const string appid = Variables.YahooAppId;
            var uri = "http://jlp.yahooapis.jp/MAService/V1/parse?appid=" + appid + "&sentence=" +
                      sentence + "&results=ma";
            var request = (HttpWebRequest) WebRequest.Create(uri);
            WebResponse response = request.GetResponse();
            var respStream = response.GetResponseStream();

            using (var sr = new StreamReader(respStream))
            {
                var res = sr.ReadToEnd();
                XmlDocument document = new XmlDocument();
                document.LoadXml(res);
                var root = document.DocumentElement;
                var wordList = root["ma_result"].GetElementsByTagName("word");

                var wordsResult = new List<IWord>();
                for (var i = 0; i < wordList.Count; ++i)
                {
                    var wordNode = wordList[i];
                    var word = new Word(wordNode["surface"].InnerText,
                        wordNode["reading"].InnerText, wordNode["pos"].InnerText);
                    if (word.ContainsAlpha()) continue;
                    wordsResult.Add(word);
                }

                return wordsResult;
            }
        }
    }
}