using System;
using System.Collections.Generic;
using System.Linq;
using Nagominashare.DajareGenerator;

namespace Nagominashare.WordDictionary
{
    class Searcher
    {
        private static Searcher instance = null;

        public static Searcher GetInstance()
        {
            if (instance == null)
                instance = new Searcher(WordComparer.GetInstance());
            return instance;
        }

        private List<IDictionaryWord> dictionaryWords;
        private IWordComparer wc;

        private Searcher(IWordComparer wc)
        {
            this.wc = wc;
            DictionaryParser dp = new DictionaryParser();
            dictionaryWords = dp.Parse(new DictionaryReader().ReadAll()).ToList();
            dictionaryWords =
                dictionaryWords.Where(
                        v =>
                            v.GetWord().GetHinshi() == Hinshi.Meishi ||
                            v.GetWord().GetHinshi() == Hinshi.Doshi ||
                            v.GetWord().GetHinshi() == Hinshi.Keiyoshi ||
                            v.GetWord().GetHinshi() == Hinshi.Rentaishi)
                    .ToList();
        }

        public double Evaluate(IWord source, IDictionaryWord target)
        {
            double[,,] dp;
            wc.DoDP(source, (IWord) target, out dp);
            double res = 0;
            for (int i = 0; i < dp.GetLength(1); i++)
            {
                res = Math.Max(res, dp[dp.GetLength(0) - 1, i, 0]);
            }
            if (res + 1e-6 > dp.GetLength(0) - 1) return 0;
            return res;
        }

        public IDictionaryWord FindBestWord(IWord source)
        {
            int minUsingCount = 100100100;
            foreach (IDictionaryWord dw in dictionaryWords)
            {
                minUsingCount = Math.Min(minUsingCount, dw.GetUsingCount());
            }
            double maxEval = -100100100;
            IDictionaryWord res = null;
            foreach (IDictionaryWord dw in dictionaryWords)
            {
                if (minUsingCount != dw.GetUsingCount())
                    continue;
                double eval = Evaluate(source, dw);
                if (maxEval < eval)
                {
                    maxEval = eval;
                    res = dw;
                }
            }
            if (res == null)
            {
                ;
            }
            return res;
        }
    }
}