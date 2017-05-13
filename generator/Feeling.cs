using System;
using System.Json;

namespace Nagominashare
{
    class Feeling : IFeeling
    {
        public int Calm { get; }
        public int Anger { get; }
        public int Joy { get; }
        public int Sorrow { get; }
        public int Energy { get; }
        public int error;

        public Feeling(string json)
        {
            JsonArray jsonArray = new JsonArray(JsonValue.Parse(json));
            if (jsonArray != 0)
            {
                Console.Error.WriteLine("Feeling API error[: " + error);
            }
            else
            {
                error = jsonArray[0];
                Calm = jsonArray[1];
                Anger = jsonArray[2];
                Joy = jsonArray[3];
                Sorrow = jsonArray[4];
                Energy = jsonArray[5];
            }
        }

        public bool IsError()
        {
            return error != 0;
        }

        public int GetErrorCode()
        {
            return error;
        }

        public int EvaluateSmile()
        {
            return Joy * Energy;
        }
    }
}