using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace SantMarti.Z80.Assembler
{
    public static class Z80Opcodes
    {
        public const byte EXX = 0xd9;
        public const byte ADD_AN = 0xc6;                        // ADD A,n
        public const byte ADD_AHL = 0x86;                       // ADD A,(HL)
        public const byte ADD_HLBC = 0x9;                       // ADD HL,BC
        public const byte ADD_HLDE = 0x19;                      // ADD HL,DE
        public const byte ADD_HLHL = 0x29;                      // ADD HL,HL
        public const byte ADD_HLSP = 0x39;                      // ADD HL,SP
        public const byte ADD_IXBC = 0x9;                       // ADD IX,BC
        public const byte ADD_IXDE = 0x19;                      // ADD IX,DE
        public const byte ADD_IXIX = 0x29;                      // ADD IX,IX
        public const byte ADD_IXSP = 0x39;                      // ADD IX,SP
        public const byte ADD_IYBC = 0x9;                       // ADD IY,BC
        public const byte ADD_IYDE = 0x19;                      // ADD IY,DE
        public const byte ADD_IYIY = 0x29;                      // ADD IY,IY
        public const byte ADD_IYSP = 0x39;                      // ADD IY,SP

        public const byte ADD_AIXIY = 0x8e;                     // ADD A,(IX + n) or ADD A,(IY + n)


        public const byte LD_AI = 0x57;                         // LD A,I
        public const byte LD_AR = 0x5f;                         // LD A,R
        public const byte LD_IA = 0x47;                         // LD I,A
        public const byte LD_RA = 0x4f;                         // LD R,A
        public const byte LD_SPNN = 0x31;                       // LD SP,nn

        public static class Bases
        {
            public const byte LD_RR = 0b01000000;                // Base for LD r,r' opcodes   
            public const byte LD_RN = 0b00000110;                // Base for LD r,n opcodes
            public const byte LD_RNN = 0b00000001;               // Base for LD r,nn opcodes  
            public const byte ADD_AR = 0b10000000;               // Base for ADD A,r
        }
        
        public static class Prefixes
        {
            public const byte DD = 0xdd;
            public const byte FD = 0xfd;
            public const byte ED = 0xed;
        }
    }
}
