using DataContracts;
using MassTransit;

namespace BusService.Main
{
    public class RegisterConsumer : IConsumer<RegisterMessage>
    {
        // Консьюмер зарегистрирован через conf.Instance(...): один объект обслуживает все потоки
        // MassTransit, поэтому изменения списка портов и флага выполняются под общей блокировкой.
        private readonly object _sync = new();

        private readonly HashSet<string> _ports;

        private string? flagPort;
        public RegisterConsumer()
        {
            _ports = new HashSet<string>();
        }
        public async Task Consume(ConsumeContext<RegisterMessage> context)
        {
            RegisterMessage message = context.Message;
            HashSet<string> portsSnapshot;
            string? flagPortSnapshot;

            lock (_sync)
            {
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

                // копия, чтобы сериализация сообщения не читала коллекцию, которую меняет другой поток
                portsSnapshot = new HashSet<string>(_ports);
                flagPortSnapshot = flagPort;
            }

            // без await исключение публикации терялось, а сообщение могло не уйти
            await context.Publish<NewRegisterMessage>(new
            {
                Ports = portsSnapshot,
                FlagPort = flagPortSnapshot
            });
        }
    }
}
