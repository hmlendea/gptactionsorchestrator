using NuciLog.Core;

namespace GptActionsOrchestrator.Logging
{
    public sealed class MyOperation : Operation
    {
        MyOperation(string name) : base(name) { }

        public static Operation GetPersonalLogs => new MyOperation(nameof(GetPersonalLogs));

        public static Operation SteamStoreAppDataRetrieval => new MyOperation(nameof(SteamStoreAppDataRetrieval));
    }
}
