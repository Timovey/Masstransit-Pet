using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public class ArrayMessage
    {
        public Guid Id { get; }

        public int Index { get; set; }

        public int[] Array { get; set; }

        public ArrayMessage()
        {
            Id = Guid.NewGuid();
        }
    }
}
