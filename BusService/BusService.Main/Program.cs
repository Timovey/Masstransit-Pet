using BusService.Main;
using MassTransit;

RegisterConsumer registerConsumer = new RegisterConsumer();
RequestConsumer requestConsumer = new RequestConsumer();

IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host("localhost", "/", h =>
    {
        h.Username("guest");
        h.Password("guest");
    });

    cfg.ReceiveEndpoint("mycompany.domains.queues", conf =>
    {
        conf.Instance(registerConsumer);
    });

    cfg.ReceiveEndpoint("request.queue", conf =>
    {
        conf.Instance(requestConsumer);
    });
});

rabbitBusControl.Start();
Console.WriteLine("Bus is started");
Console.ReadKey();

rabbitBusControl.Stop();