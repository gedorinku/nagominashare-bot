namespace Nagominashare
{
    interface IFeeling
    {
        int Calm { get; }
        int Anger { get; }
        int Joy { get; }
        int Sorrow { get; }
        int Energy { get; }
        bool IsError();
        int GetErrorCode();
        int EvaluateSmile();
    }
}