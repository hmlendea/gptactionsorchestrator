using System.Collections.Generic;
using GptActionsOrchestrator.Integrations.PersonalLogManager.Service.Models;

namespace GptActionsOrchestrator.Integrations.PersonalLogManager.Service
{
    public interface IPersonalLogManagerService
    {
        public PersonalLogs GetPersonalLogs(
            string date,
            string time,
            string template,
            string localisation,
            Dictionary<string, string> data,
            int count);
    }
}
