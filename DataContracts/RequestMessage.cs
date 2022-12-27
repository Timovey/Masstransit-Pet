namespace DataContracts
{
    public class RequestMessage
    {
        public Guid Id { get; }

        public int Time { get; set; }

        public string Port { get; set; }

        public RequestMessage()
        {
            Id = Guid.NewGuid();
        }
    }
}
