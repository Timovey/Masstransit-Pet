using DataContracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Runtime;

namespace SendService.Main.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ResponseController : ControllerBase
    {
        private readonly PortSetting _settings;
        public ResponseController(IOptions<PortSetting> options)
        {
            _settings = options.Value;
        }

        [HttpPost]
        public Task SetFlag(ResponseMessage responseMessage)
        {
            Console.WriteLine("К нам пришел флаг (контроллер)");
            if(responseMessage != null && responseMessage.Flag && !responseMessage.Port.Contains(_settings.Urls))
            {
                Console.WriteLine("Устанавливаем флаг");
                object locker = new();
                lock (locker)
                {
                    GlobalStore.Flag = true;
                }
            }
            

            return Task.CompletedTask;
        }
    }
}
