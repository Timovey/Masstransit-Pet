using DataContracts;
using MassTransit;
using Microsoft.Extensions.Options;

namespace StartService.Main
{
    public class ArrayReadyConsumer : IConsumer<ArrayReadyMessage>
    {
        private readonly LengthSetting _settings;
        public ArrayReadyConsumer(IOptions<LengthSetting> options)
        {
            _settings = options.Value;
        }

        public async Task Consume(ConsumeContext<ArrayReadyMessage> context)
        {
            var message = context.Message;
            //Console.WriteLine(message.Index);
            //заглушка
            GlobalSum.Sum += message.Sum;
            GlobalSum.AddCount();
        }
    }
}
