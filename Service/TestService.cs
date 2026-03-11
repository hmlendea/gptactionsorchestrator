using NuciLog.Core;

namespace GptActionsOrchestrator.Service
{
    public class TestService(ILogger logger) : ITestService
    {
        public string Test(string test)
        {
            return test;
        }
    }
}