using System.Collections.Generic;
using NuciAPI.Requests;

namespace GptActionsOrchestrator.Integrations.PersonalLogManager.Client
{
    public class GetPersonalLogsRequest : NuciApiRequest
    {
        public string Date { get; set; }

        public string Time { get; set; }

        public string Template { get; set; }

        public string Localisation { get; set; } = "en";

        public Dictionary<string, string> Data { get; set; }

        public int Count { get; set; } = 1;
    }
}
