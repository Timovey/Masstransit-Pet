using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SendService.Main;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json;
using DataContracts;
using RabbitMQ.Client;
using Refit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
string port = builder.Configuration["Urls"];

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<RegisterConsumer>(typeof(RegisterConsumerDefinition))
    .Endpoint(e =>
    {
        e.Name = $"mycompany.domains.queues.events.{port.Split(':').Last().Split('/').First()}";
    });

    x.AddConsumer<RequestConsumer>(typeof(RequestConsumerDefinition))
    .Endpoint(e =>
    {
        e.Name = $"request.queue.{port.Split(':').Last().Split('/').First()}";
    });

    //x.AddConsumer<FlagConsumer>(typeof(FlagConsumerDefinition))
    //.Endpoint(e =>
    //{
    //    e.Name = "flag";
    //});
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
   
});
builder.Services.AddHostedService<Worker>();
var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    //cfg.ReceiveEndpoint($"mycompany.domains.queues.events.{port.Split(':').Last().Split('/').First()}", conf =>
    //{
    //   //conf.ConfigureConsumer(cfg, typeof(RegisterConsumer));
    //    //conf.Consumer<RegisterConsumer>(RegisterConsumerDefinition);
    //});

    cfg.ReceiveEndpoint("array", e =>
    {
        e.Consumer<ArrayConsumer>();
    });
});
await busControl.StartAsync(new CancellationToken());

string rabbitMqAddress = "rabbitmq://localhost:15672";
string rabbitMqQueue = "mycompany.domains.queues";
Task<ISendEndpoint> sendEndpointTask = busControl.GetSendEndpoint(new Uri(string.Concat(rabbitMqAddress, "/", rabbitMqQueue)));
ISendEndpoint sendEndpoint = sendEndpointTask.Result;

Task sendTask = sendEndpoint.Send<RegisterMessage>(new
{
    Port = port,
    IsDeleted = false,
    Flag = false,
}, c =>
{
    c.FaultAddress = new Uri("rabbitmq://localhost:15672/accounting/mycompany.queues.errors.newcustomers");
});

builder.Services.AddOptions<MassTransitHostOptions>()
    .Configure(options =>
    {
        options.WaitUntilStarted = true;
    });

// Add services to the container.
builder.Services.Configure<PortSetting>(builder.Configuration);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var refitSettings = new RefitSettings
{
    ContentSerializer = new SystemTextJsonContentSerializer(
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }
    )
};

//!_! ------------------ Auth
var computeAddress = new Uri(builder.Configuration["ComputeSettings:BaseAddress"]);
builder.Services.AddRefitClient<IComputeApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = computeAddress);


var app = builder.Build();

if (app.Environment.IsDevelopment() || 1 == 1)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();

