using Microsoft.AspNetCore.Mvc;
using NuciAPI.Controllers;
using GptActionsOrchestrator.Configuration;
using GptActionsOrchestrator.Models;
using GptActionsOrchestrator.Requests;
using GptActionsOrchestrator.Service;
using NuciAPI.Responses;
using System.Linq;

namespace GptActionsOrchestrator.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActionsController(
        IActionsOrchestrator actionsOrchestrator,
        SecuritySettings securitySettings) : NuciApiController
    {
        readonly NuciApiAuthorisation authorisation = NuciApiAuthorisation.ApiKey(securitySettings.ApiKey);

        [HttpGet]
        public ActionResult Get([FromQuery] GetActionRequest request)
        {
            GptAction action = GptAction.FromString(request.GptActionName);

            if (action.Equals(GptAction.Unknown))
            {
                return BadRequest(new NuciApiErrorResponse($"The requested action is not valid."));
            }

            return ProcessRequest(
                request,
                () => actionsOrchestrator.Get(Request.Query.ToDictionary(
                    pair => pair.Key,
                    pair => pair.Value.ToString()
                )),
                authorisation);
        }
    }
}
