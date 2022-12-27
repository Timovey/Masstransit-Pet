using MassTransit;

namespace StartService.Main
{
    public class ArrayReadyDefinition :
    ConsumerDefinition<ArrayReadyConsumer>
    {
        public ArrayReadyDefinition()
        {

        }
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<ArrayReadyConsumer> consumerConfigurator)
        {
            //endpointConfigurator.UseInMemoryOutbox();
        }
    }
}
