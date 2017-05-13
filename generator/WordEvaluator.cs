namespace Nagominashare
{
    abstract class WordEvaluator
    {
        private static WordEvaluator instance = null;

        public static WordEvaluator GetInstance()
        {
            return instance ?? (instance = new GoogleWordEvaluator());
        }

        public abstract long Evaluate(string word);
    }
}