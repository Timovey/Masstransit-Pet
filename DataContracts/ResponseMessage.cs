using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public class ResponseMessage
    {
        public Guid Id { get; }

        public bool Flag { get; set; }

        public string Port { get; set; }

        public ResponseMessage()
        {
            Id = Guid.NewGuid();
        }
    }
}
