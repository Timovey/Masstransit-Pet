using DataContracts;
using MassTransit;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;

namespace SendService.Main
{
    public class Worker : BackgroundService
    {
        readonly IBus _bus;
        private readonly PortSetting _settings;
        private readonly IComputeApi _computeApi;
        public async override Task StopAsync(CancellationToken stoppingToken)
        {
            await _bus.Publish(new RegisterMessage
            {
                Port = _settings.Urls,
                IsDeleted = true,
                Flag = GlobalStore.Flag
            });
        }
        public Worker(IBus bus, IOptions<PortSetting> options, IComputeApi computeApi)
        {
            _bus = bus;
            _settings = options.Value;
            _computeApi = computeApi;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //если есть флаг и в сторе лежат массивы
                if (GlobalStore.Flag && GlobalStore.GetArraysCount() > 0)
                {
                    ArrayMessage array;
                    object locker = new();
                    lock (locker)
                    {
                        array = GlobalStore.GetArray();
                        GlobalStore.IsSendRequest = false;
                    }
                    int sum = await _computeApi.GetArraySum(array);
                    await _bus.Publish(new ArrayReadyMessage
                    {
                        Index = array.Index,
                        Sum = sum
                    }, stoppingToken);
                    Console.WriteLine("Массив:");
                    Console.WriteLine(array.Index);
                    Console.WriteLine("Посчитали сумму");
                    Console.WriteLine(sum);
                }
                //если флага нет и мы не отправляли запрос и у нас есть массивы
                else if(!GlobalStore.Flag && GlobalStore.IsSendRequest == false && GlobalStore.GetArraysCount() > 0)
                {
                    Console.WriteLine("Отправили запрос на флаг ");
                    await _bus.Publish(new RequestMessage
                    {
                        Port = _settings.Urls,
                        Time = GlobalStore.GetTime()
                    }, stoppingToken);
                    object locker = new();
                    lock (locker)
                    {
                        GlobalStore.IsSendRequest = true;
                    }
                }    
            } 
        }
       
    }
}
