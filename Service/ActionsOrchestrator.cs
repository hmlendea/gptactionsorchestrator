using System;
using System.Collections.Generic;
using GptActionsOrchestrator.Integrations.PersonalLogManager.Service;
using GptActionsOrchestrator.Models;
using GptActionsOrchestrator.Responses;

namespace GptActionsOrchestrator.Service
{
    public sealed class ActionsOrchestrator(
        IPersonalLogManagerService personalLogManagerService,
        ISteamStoreService steamStoreService) : IActionsOrchestrator
    {
        public GetActionResponse Get(Dictionary<string, string> rawParameters)
        {
            Dictionary<string, object> parameters = BuildParameters(rawParameters);
            GptAction action = GetGptActionFromParameters(rawParameters);
            object data;

            if (action == GptAction.GetPersonalLogs)
            {
                data = personalLogManagerService.GetPersonalLogs(
                    (string)(parameters.TryGetValue("date", out object date) ? date : null),
                    (string)(parameters.TryGetValue("time", out object time) ? time : null),
                    (string)(parameters.TryGetValue("template", out object template) ? template : null),
                    (string)(parameters.TryGetValue("localisation", out object localisation) ? localisation : null),
                    (Dictionary<string, string>)(parameters.TryGetValue("data", out object dataDict) ? dataDict : null),
                    int.Parse((string)(parameters.TryGetValue("count", out object count) ? count : "100")));
            }
            else if (action == GptAction.GetSteamAppData)
            {
                data = steamStoreService.GetAppData((string)(parameters.TryGetValue("appId", out object appId) ? appId : null));
            }
            else
            {
                throw new NotImplementedException($"Action '{action.Name}' is not implemented.");
            }

            return new GetActionResponse
            {
                GptActionName = action.ToString(),
                Data = data
            };
        }

        Dictionary<string, object> BuildParameters(Dictionary<string, string> rawParameters)
        {
            Dictionary<string, object> parameters = [];

            foreach (var pair in rawParameters)
            {
                if (pair.Key.Contains('.'))
                {
                    string[] parts = pair.Key.Split('.', 2);
                    string parentKey = parts[0];
                    string childKey = parts[1];

                    if (!parameters.ContainsKey(parentKey))
                    {
                        parameters[parentKey] = new Dictionary<string, string>();
                    }

                    ((Dictionary<string, string>)parameters[parentKey])[childKey] = pair.Value;
                }
                else
                {
                    parameters[pair.Key] = pair.Value;
                }
            }

            return parameters;
        }

        GptAction GetGptActionFromParameters(Dictionary<string, string> parameters)
        {
            if (parameters.TryGetValue("action", out string actionName))
            {
                return GptAction.FromString(actionName);
            }

            return GptAction.Unknown;
        }
    }
}
