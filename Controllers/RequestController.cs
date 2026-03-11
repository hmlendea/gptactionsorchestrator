using Microsoft.AspNetCore.Mvc;
using NuciAPI.Controllers;
using GptActionsOrchestrator.Configuration;
using GptActionsOrchestrator.Service;
using GptActionsOrchestrator.Requests;

namespace GptActionsOrchestrator.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RequestController(
        ITestService testService,
        SecuritySettings securitySettings) : NuciApiController
    {
        readonly NuciApiAuthorisation authorisation = NuciApiAuthorisation.ApiKey(securitySettings.ApiKey);

        [HttpGet]
        public ActionResult Get([FromQuery] GetActionRequest request)
            => ProcessRequest(request, () => testService.Test("test"), authorisation);
    }
}
