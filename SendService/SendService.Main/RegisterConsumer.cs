using DataContracts;
using MassTransit;
using Microsoft.Extensions.Options;

namespace SendService.Main
{
    public class RegisterConsumer : IConsumer<NewRegisterMessage>
    {
        private readonly PortSetting _settings;
        public RegisterConsumer(IOptions<PortSetting> options)
        {
            _settings = options.Value;
        }

        public Task Consume(ConsumeContext<NewRegisterMessage> context)
        {
            var message = context.Message;
            //заглушка
            object locker = new();
            lock(locker)
            {
                if (message.FlagPort != null)
                {
                    GlobalStore.Flag = _settings.Urls.Contains(message.FlagPort);
                }
                GlobalStore.SetPorts(message.Ports);
                Console.WriteLine("Флаг ");
                Console.WriteLine(GlobalStore.Flag);
                Console.WriteLine("Порты: ");
                Console.WriteLine(GlobalStore.GetPorts().Count);
            }
            return Task.FromResult(context.Message);
        }
    }
}
