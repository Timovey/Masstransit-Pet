using DataContracts;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusService.Main
{
    public class RequestConsumer : IConsumer<RequestMessage>
    {
        public RequestConsumer()
        {
        }
        public Task Consume(ConsumeContext<RequestMessage> context)
        {
            RequestMessage message = context.Message;

            //Console.WriteLine("Всего портов: ");
            Console.WriteLine("Пришел запрос");

            context.Publish<NewRequestMessage>(new
            {
                Port = message.Port,
                Time = message.Time
            });

            return Task.FromResult(context.Message);
        }
    }
}
