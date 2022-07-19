using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler
{
    public static class Z80Opcodes
    {
        public const byte EXX = 0xd9;
        public const byte ADD_AN = 0xc6;                         // ADD A,n

        public static class Bases
        {
            public const byte LD_XY = 0b01000000;                // Base for LD X,Y opcodes
            
        }
    }
}
