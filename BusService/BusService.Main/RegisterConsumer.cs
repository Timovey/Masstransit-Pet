using DataContracts;
using MassTransit;

namespace BusService.Main
{
    public class RegisterConsumer : IConsumer<RegisterMessage>
    {
        private readonly HashSet<string> _ports;

        private string flagPort;
        public RegisterConsumer()
        {
            _ports = new HashSet<string>();
        }
        public Task Consume(ConsumeContext<RegisterMessage> context)
        {
            RegisterMessage message = context.Message;
            if(message.IsDeleted)
            {
                Console.WriteLine("Нас покинула фея: ");
                Console.WriteLine(message.Port);
                _ports.Remove(message.Port);
                if (message.Flag)
                {
                    flagPort = _ports.FirstOrDefault();
                }
            }
            else
            {
                Console.WriteLine("Вместе мы сильны, чудеса творить вольны, новый сервис с нами на порту: ");
                Console.WriteLine(message.Port);
                if (flagPort == null || flagPort.Length == 0)
                {
                    flagPort = message.Port;
                }
                _ports.Add(message.Port);
            }
            Console.WriteLine("Всего портов: ");
            Console.WriteLine(_ports.Count);
            context.Publish<NewRegisterMessage>(new
            {
                Ports = _ports,
                FlagPort = flagPort
            });

            return Task.FromResult(context.Message);
        }
    }
}
