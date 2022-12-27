using MassTransit;

namespace SendService.Main
{
    public class RegisterConsumerDefinition :
    ConsumerDefinition<RegisterConsumer>
    {
        public RegisterConsumerDefinition()
        {

        }
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<RegisterConsumer> consumerConfigurator)
        {
            //endpointConfigurator.UseInMemoryOutbox();
        }
    }
}
