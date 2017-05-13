using System;
using System.Linq;
using Nagominashare.SentenceParsing;

namespace Nagominashare.DajareGenerator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var input = Console.ReadLine().Trim();
            var parser = new SentenceParser();
            var words = parser.ExtractWords(input).Result;
            var generator = new Generator();
            var result = generator.Generate(words.ToList()).GetAwaiter().GetResult();
            Console.WriteLine(result.ToString());
        }
    }
}