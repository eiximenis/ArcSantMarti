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
        public const byte ADD_HLBC = 0x9;
        public const byte ADD_HLDE = 0x19;
        public const byte ADD_HLHL = 0x29;
        public const byte ADD_HLSP = 0x39;
        public const byte ADD_IXBC = 0x9;
        public const byte ADD_IXDE = 0x19;
        public const byte ADD_IXIX = 0x29;
        public const byte ADD_IXSP = 0x39;
        public const byte ADD_IYBC = 0x9;
        public const byte ADD_IYDE = 0x19;
        public const byte ADD_IYIY = 0x29;
        public const byte ADD_IYSP = 0x39;

        public static class Bases
        {
            public const byte LD_XY = 0b01000000;                // Base for LD X,Y opcodes   
        }
        
        public static class Prefixes
        {
            public const byte DD = 0xdd;
            public const byte FD = 0xfd;
        }
    }
}
