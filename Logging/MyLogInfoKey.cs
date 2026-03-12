using NuciLog.Core;

namespace GptActionsOrchestrator.Logging
{
    public sealed class MyLogInfoKey : LogInfoKey
    {
        MyLogInfoKey(string name) : base(name) { }

        public static LogInfoKey GptAction => new MyLogInfoKey(nameof(GptAction));

        public static LogInfoKey AppId => new MyLogInfoKey(nameof(AppId));
    }
}
