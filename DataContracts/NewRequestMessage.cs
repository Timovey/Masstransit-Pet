using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public class NewRequestMessage
    {
        public Guid Id { get; }

        public int Time { get; set; }

        public string Port { get; set; }

        public NewRequestMessage()
        {
            Id = Guid.NewGuid();
        }
    }
}
