using System.Net.Http;
using System.Text.RegularExpressions;
using GptActionsOrchestrator.Logging;
using GptActionsOrchestrator.Service.Models;
using NuciLog.Core;
using NuciWeb.HTTP;

namespace GptActionsOrchestrator.Service
{
    public class SteamStoreService(ILogger logger) : ISteamStoreService
    {
        const string StorefrontApiUrl = "http://store.steampowered.com/api";
        const string StorefrontApiCountry = "RO";
        const string StorefrontApiFilters = "basic";

        readonly HttpClient httpClient = HttpClientCreator.Create();
        readonly ILogger logger = logger;

        public SteamAppEntity GetAppData(string appId)
        {
            const string namePattern = "\"name\": *\"([^\"]*)\"";

            logger.Info(
                MyOperation.SteamStoreAppDataRetrieval,
                OperationStatus.Started,
                new LogInfo(MyLogInfoKey.AppId, appId));

            string endpoint = $"{StorefrontApiUrl}/appdetails?appids={appId}&cc={StorefrontApiCountry}&filters={StorefrontApiFilters}";
            string responseContent = httpClient.GetStringAsync(endpoint).Result;

            SteamAppEntity steamAppEntity = new()
            {
                Id = appId,
                Name = Regex.Match(responseContent, namePattern).Groups[1].Value
            };

            logger.Debug(
                MyOperation.SteamStoreAppDataRetrieval,
                OperationStatus.Success,
                new LogInfo(MyLogInfoKey.AppId, appId));

            return steamAppEntity;
        }
    }
}