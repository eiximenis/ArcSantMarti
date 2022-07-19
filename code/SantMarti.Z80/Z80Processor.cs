using SantMarti.Z80.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80
{
    public class Z80Processor
    {
        public Z80Registers Registers { get; }
        public RamMemory Memory { get; }
        public Z80Processor()
        {
            Registers = new Z80Registers();
            Memory = new RamMemory(48);
        }


    }
}
