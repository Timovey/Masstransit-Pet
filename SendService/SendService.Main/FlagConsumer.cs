using DataContracts;
using MassTransit;
using Microsoft.Extensions.Options;

namespace SendService.Main
{
    public class FlagConsumer : IConsumer<ResponseMessage>
    {
        private readonly PortSetting _settings;
        public FlagConsumer(IOptions<PortSetting> options)
        {
            _settings = options.Value;
        }

        public Task Consume(ConsumeContext<ResponseMessage> context)
        {
            var message = context.Message;
            Console.WriteLine("Пришел ответ");
            Console.WriteLine(message.Flag);
            Console.WriteLine(message.Port);
            if (message.Flag)
            {
                Console.WriteLine("Устанавливаем флаг");
                //заглушка
                object locker = new();
                lock (locker)
                {
                    GlobalStore.Flag = true;
                }
            }
            return Task.FromResult(context.Message);
        }
    }
}
