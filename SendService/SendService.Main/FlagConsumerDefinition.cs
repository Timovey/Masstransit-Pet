using MassTransit;

namespace SendService.Main
{
    public class FlagConsumerDefinition :
    ConsumerDefinition<FlagConsumer>
    {
    public FlagConsumerDefinition()
    {

    }
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<FlagConsumer> consumerConfigurator)
    {
        //endpointConfigurator.UseInMemoryOutbox();
    }
}
}
