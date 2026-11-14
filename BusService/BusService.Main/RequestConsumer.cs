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
        public async Task Consume(ConsumeContext<RequestMessage> context)
        {
            RequestMessage message = context.Message;

            //Console.WriteLine("Всего портов: ");
            Console.WriteLine("Пришел запрос");

            // без await исключение публикации терялось, а сообщение могло не уйти
            await context.Publish<NewRequestMessage>(new
            {
                Port = message.Port,
                Time = message.Time
            });
        }
    }
}
