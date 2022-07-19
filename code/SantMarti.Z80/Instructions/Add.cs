using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Instructions
{
    public static class Add
    {
        /// <summary>
        /// ADD A,n: Adds n to A
        /// </summary>
        public static void AddAN(Instruction instruction, Z80Processor processor)
        {
            var data = processor.FetchNext();

        }
    }
}
