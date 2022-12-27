using MassTransit;

namespace SendService.Main
{
    public class RequestConsumerDefinition :
    ConsumerDefinition<RequestConsumer>
    {
        public RequestConsumerDefinition()
        {

        }
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<RequestConsumer> consumerConfigurator)
        {
            //endpointConfigurator.UseInMemoryOutbox();
        }
    }
}
