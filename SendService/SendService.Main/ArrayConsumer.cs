using DataContracts;
using MassTransit;
using System.Runtime;

namespace SendService.Main
{
    public class ArrayConsumer : IConsumer<ArrayMessage>
    {
        public ArrayConsumer()
        {
        }

        public Task Consume(ConsumeContext<ArrayMessage> context)
        {
            var message = context.Message;
            Console.WriteLine("Дабавили массив: ");
            Console.WriteLine(message.Index);
            //заглушка
            object locker = new();
            lock (locker)
            {
                GlobalStore.AddArrays(message.Index, message.Array);
            }
            return Task.FromResult(context.Message);
        }
    }
}
