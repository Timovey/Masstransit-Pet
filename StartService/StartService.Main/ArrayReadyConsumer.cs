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
            //консьюмер вызывается параллельно, поэтому сумма и счётчик обновляются атомарно
            GlobalSum.Add(message.Sum);
        }
    }
}
