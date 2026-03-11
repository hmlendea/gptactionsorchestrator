using Microsoft.AspNetCore.Mvc;
using NuciAPI.Requests;

namespace GptActionsOrchestrator.Requests
{
    public sealed class GetActionRequest : NuciApiRequest
    {
        [FromQuery(Name = "action")]
        public string ActionName { get; set; }
    }
}
