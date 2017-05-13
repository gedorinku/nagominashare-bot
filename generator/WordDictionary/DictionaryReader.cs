using System.Collections.Generic;
using System.IO;

namespace Nagominashare.WordDictionary
{
    class DictionaryReader : IDictionaryReader
    {
        public IEnumerable<string> ReadAll()
        {
            var result = new List<string>();
            using (
                var reader = new StreamReader("Assets/NAIST_Japanese_Dictionary_Small.txt"
                ))
            {
                long count = 0;
                while (!reader.EndOfStream)
                {
                    result.Add(reader.ReadLine());
                    count++;
                }
            }

            return result;
        }
    }
}