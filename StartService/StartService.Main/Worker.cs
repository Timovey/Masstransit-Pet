using DataContracts;
using MassTransit;
using Microsoft.Extensions.Options;
using System;

namespace StartService.Main
{
    public class Worker : BackgroundService
    {
        readonly IBus _bus;
        private readonly LengthSetting _settings;
        public Worker(IBus bus, IOptions<LengthSetting> options)
        {
            _bus = bus;
            _settings = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int length = _settings.Length;
            int[][] arr = new int[length][];
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                arr[i] = new int[length];
                for (int j = 0; j < length; j++)
                {
                    arr[i][j] = random.Next(100);
                }
            }
            int attemts = 0;
           
            GlobalSum.StartTime = DateTime.Now;
            arr = MatrixHelper.Transpon(arr);
            Console.WriteLine($"Время начальное {GlobalSum.StartTime}");
            GlobalSum.Sum = 0;
            GlobalSum.Count = 0;
            attemts++;
            for (int i = 0; i < length; i++)
            {
                await _bus.Publish(new ArrayMessage
                {
                    Array = arr[i],
                    Index = i
                }, stoppingToken);
            }
            Console.WriteLine("Отправили запросы");

            DateTime postSend = DateTime.Now;
            while(true)
            {
                if((DateTime.Now - postSend).TotalSeconds > 14 + length/1000* length /1000)
                {
                    Console.WriteLine($"Много времени");
                    break;
                }
                if (GlobalSum.Count == _settings.Length)
                {
                    Console.WriteLine($"Все хорошо, вот сумма {GlobalSum.Sum}");
                    Console.WriteLine($"Время {(DateTime.Now - GlobalSum.StartTime).TotalSeconds}");
                    Console.WriteLine($"Время конечное {DateTime.Now}");
                    break;
                }

            }            
        }
    }
}
