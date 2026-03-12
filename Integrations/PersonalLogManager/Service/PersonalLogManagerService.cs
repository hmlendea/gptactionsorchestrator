using System;
using System.Collections.Generic;
using System.Net.Http;
using GptActionsOrchestrator.Configuration;
using GptActionsOrchestrator.Integrations.PersonalLogManager.Client;
using GptActionsOrchestrator.Integrations.PersonalLogManager.Configuration;
using GptActionsOrchestrator.Integrations.PersonalLogManager.Service.Models;
using GptActionsOrchestrator.Logging;
using NuciAPI.Client;
using NuciAPI.Responses;
using NuciLog.Core;

namespace GptActionsOrchestrator.Integrations.PersonalLogManager.Service
{
    public sealed class PersonalLogManagerService(
        PersonalLogManagerSettings plmSettings,
        SecuritySettings securitySettings,
        ILogger logger) : IPersonalLogManagerService
    {
        readonly NuciApiClient client = new(plmSettings.BaseUrl);

        public PersonalLogs GetPersonalLogs(
            string date,
            string time,
            string template,
            string localisation,
            Dictionary<string, string> data,
            string count)
        {
            IEnumerable<LogInfo> logInfos =
            [
                new(MyLogInfoKey.Template, template),
                new(MyLogInfoKey.Date, date),
                new(MyLogInfoKey.Time, time),
                new(MyLogInfoKey.Localisation, localisation),
                new(MyLogInfoKey.Count, count)
            ];

            logger.Info(
                MyOperation.GetPersonalLogs,
                OperationStatus.Started,
                logInfos);

            try
            {
                PersonalLogs personalLogs = RetrievePersonalLogs(date, time, template, localisation, data, count);

                logger.Debug(
                    MyOperation.GetPersonalLogs,
                    OperationStatus.Success,
                    logInfos);

                return personalLogs;
            }
            catch (Exception exception)
            {
                logger.Error(
                    MyOperation.GetPersonalLogs,
                    OperationStatus.Failure,
                    exception,
                    logInfos);

                throw;
            }
        }

        PersonalLogs RetrievePersonalLogs(
            string date,
            string time,
            string template,
            string localisation,
            Dictionary<string, string> data,
            string count)
        {
            NuciApiRequestAuthorisationInfo authorisation = new()
            {
                ClientId = securitySettings.ClientId,
                BearerToken = plmSettings.ApiKey,
                HmacSharedSecretKey = plmSettings.HmacSigningKey
            };

            NuciApiResponse response =
                client.SendRequestAsync<GetPersonalLogsRequest, GetPersonalLogsResponse>(
                    HttpMethod.Get,
                    BuildRequest(date, time, template, localisation, data, count),
                    authorisation,
                    "PersonalLog").Result;

            if (!response.IsSuccessful)
            {
                throw new Exception(response.Message);
            }

            return new()
            {
                Logs = ((GetPersonalLogsResponse)response).Logs
            };
        }

        GetPersonalLogsRequest BuildRequest(
            string date,
            string time,
            string template,
            string localisation,
            Dictionary<string, string> data,
            string count)
        {
            GetPersonalLogsRequest request = new()
            {
                Date = date,
                Time = time,
                Template = template,
                Localisation = localisation,
                Data = data
            };

            if (string.IsNullOrWhiteSpace(localisation))
            {
                request.Localisation = "ro";
            }

            if (string.IsNullOrWhiteSpace(count))
            {
                request.Count = 1000;
            }
            else
            {
                request.Count = int.Parse(count);
            }

            return request;
        }
    }
}
