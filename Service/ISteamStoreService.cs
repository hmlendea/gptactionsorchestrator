using GptActionsOrchestrator.Service.Models;

namespace GptActionsOrchestrator.Service
{
    public interface ISteamStoreService
    {
        public SteamAppEntity GetAppData(string appId);
    }
}
