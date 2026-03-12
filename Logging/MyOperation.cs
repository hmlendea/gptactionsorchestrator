using NuciLog.Core;

namespace GptActionsOrchestrator.Logging
{
    public sealed class MyOperation : Operation
    {
        MyOperation(string name) : base(name) { }

        public static Operation PersonalLogRetrieval => new MyOperation(nameof(PersonalLogRetrieval));

        public static Operation SteamStoreAppDataRetrieval => new MyOperation(nameof(SteamStoreAppDataRetrieval));
    }
}
