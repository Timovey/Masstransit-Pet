using DataContracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace SendService.Main.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SendController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        public SendController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(RequestMessage requestMessage)
        {
            await _publishEndpoint.Publish<RequestMessage>(new
            {
                Id = requestMessage.Id,
                requestMessage.Time
            });
            return Ok();
        }
    }
}
