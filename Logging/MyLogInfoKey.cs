using NuciLog.Core;

namespace GptActionsOrchestrator.Logging
{
    public sealed class MyLogInfoKey : LogInfoKey
    {
        MyLogInfoKey(string name) : base(name) { }

        public static LogInfoKey Action => new MyLogInfoKey(nameof(Action));
    }
}
