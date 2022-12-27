using DataContracts;
using MassTransit;
using Microsoft.Extensions.Options;
using System.IO;

namespace SendService.Main
{
    public class RequestConsumer : IConsumer<NewRequestMessage>
    {
        private readonly PortSetting _settings;
        public RequestConsumer(IOptions<PortSetting> options)
        {
            _settings = options.Value;
        }

        public async Task Consume(ConsumeContext<NewRequestMessage> context)
        {
            var message = context.Message;
            Console.WriteLine("Поступил запрос на флаг от");
            Console.WriteLine(message.Port);
            Console.WriteLine("С временем");
            Console.WriteLine(message.Time);
            if(message.Port.Contains(_settings.Urls))
            {
                Console.WriteLine("Это наш же запрос");
                return;
            }
            if(!GlobalStore.Flag)
            {
                Console.WriteLine("У нас нет флага");
                Console.WriteLine("Ждем флага");
            }
            while(!GlobalStore.Flag)
            {
            }

            Console.WriteLine("Наше время");
            Console.WriteLine(GlobalStore.GetTime());
            if (GlobalStore.GetTime() > message.Time)
            {
                Console.WriteLine("Отправили флаг");
                object locker = new();
                lock (locker)
                {
                    GlobalStore.Flag = false;
                    //context.Publish<ResponseMessage>(new
                    //{
                    //    Port = _settings.Urls,
                    //    Time = GlobalStore.GetTime(),
                    //    Flag = true
                    //});
                }
                HttpClient client = new HttpClient();
                string path = String.Concat(message.Port, "/response/SetFlag");
                Console.WriteLine("По путы");
                Console.WriteLine(path);
                await client.PostAsJsonAsync(path, new ResponseMessage()
                {
                    Flag = true,
                    Port = _settings.Urls

                });
            }
            else
            {
                bool isMore = false;
                if(GlobalStore.GetTime() <= message.Time)
                {
                    Console.WriteLine("Время не меньше");
                }
                while(!context.CancellationToken.IsCancellationRequested && !isMore)
                {
                    if(GlobalStore.GetTime() > message.Time)
                    {
                        isMore = true;
                    }
                }
                Console.WriteLine("Отправляем флаг");
                HttpClient client = new HttpClient();
                string path = String.Concat(message.Port, "/response/SetFlag");
                Console.WriteLine("По путы");
                Console.WriteLine(path);
                await client.PostAsJsonAsync(path, new ResponseMessage()
                {
                    Flag = true,
                    Port = _settings.Urls

                });
            }
                
            
        }
    }
}
