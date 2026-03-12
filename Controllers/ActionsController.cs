using Microsoft.AspNetCore.Mvc;
using NuciAPI.Controllers;
using GptActionsOrchestrator.Configuration;
using GptActionsOrchestrator.Models;
using GptActionsOrchestrator.Requests;
using GptActionsOrchestrator.Service;
using NuciAPI.Responses;
using GptActionsOrchestrator.Responses;

namespace GptActionsOrchestrator.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActionsController(
        ISteamStoreService steamStoreService,
        SecuritySettings securitySettings) : NuciApiController
    {
        readonly NuciApiAuthorisation authorisation = NuciApiAuthorisation.ApiKey(securitySettings.ApiKey);

        [HttpGet]
        public ActionResult Get([FromQuery] GetActionRequest request)
            => ProcessGetRequest(request);

        public ActionResult ProcessGetRequest(GetActionRequest request)
        {
            GptAction action = GptAction.FromString(request.GptActionName);

            if (action.Equals(GptAction.GetSteamAppData))
            {
                return ProcessRequest(
                    request,
                    () => new GetActionResponse
                    {
                        GptActionName = request.GptActionName,
                        Data = steamStoreService.GetAppData(Request.Query["appId"])
                    },
                    authorisation);
            }

            return BadRequest(new NuciApiErrorResponse("Unsupported action."));
        }
    }
}
