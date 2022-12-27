using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public class ArrayReadyMessage
    {
        public Guid Id { get; }

        public int Index { get; set; }

        public int Sum { get; set; }

        public ArrayReadyMessage()
        {
            Id = Guid.NewGuid();
        }
    }
}
