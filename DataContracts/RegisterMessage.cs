using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public class RegisterMessage
    {
        public Guid Id { get; }

        public string Port { get; set; }

        public bool Flag { get; set; }

        public bool IsDeleted { get; set; }

        public RegisterMessage()
        {
            Id = Guid.NewGuid();
        }
    }
}
