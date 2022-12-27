using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public class NewRegisterMessage
    {
        public Guid Id { get; }

        public HashSet<string> Ports { get; set; }

        public string FlagPort { get; set; }

        public NewRegisterMessage()
        {
            Id = Guid.NewGuid();
        }
    }
}
