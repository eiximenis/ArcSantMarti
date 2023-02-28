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
        public const byte ADD_A_N = 0xc6;                        // ADD A,n
        public const byte ADD_A_HLRef = 0x86;                       // ADD A,(HL)
        public const byte ADD_HL_BC = 0x9;                       // ADD HL,BC
        public const byte ADD_HL_DE = 0x19;                      // ADD HL,DE
        public const byte ADD_HL_HL = 0x29;                      // ADD HL,HL
        public const byte ADD_HL_SP = 0x39;                      // ADD HL,SP
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
        public const byte LD_SP_NN = 0x31;                      // LD SP,nn
        public const byte LD_A_NNRef = 0x3a;                    // LD A,(nn)
        public const byte LD_NNRef_A = 0x32;                    // LD (nn),A
        public static byte LD_HLRef_Byte = 0x36;                // LD (HL),n
        public static byte LD_Disp_Byte = 0x36;                 // LD (IX|IY + n),n
        public static byte LD_A_BCRef = 0xa;                    // LD A,(BC)
        public static byte LD_A_DERef = 0x1a;                   // LD A,(DE)
        public static byte LD_BCRef_A = 0x2;                    // LD (BC),A
        public static byte LD_DERef_A = 0x12;                   // LD (DE),A
        public static byte LD_IX_IY_NN = 0x21;                  // LD IX,nn or LD IY,nn
        public static byte LD_HL_NNRef = 0x2a;                  // LD HL,(nn)
        public static byte LD_IX_IY_NNRef = 0x2a;               // LD IX,(nn) or LD IY,(nn)
        public static byte LD_NNRef_HL = 0x22;                  // LD (nn),HL
        public static byte LD_NNRef_IX_IY = 0x22;               // LD (nn),IX or LD (nn),IY
        public static byte LD_SP_HL = 0xf9;                     // LD SP,HL
        public static byte LD_SP_IX_IY = 0xf9;                  // LD SP,IX or LD SP,IY


        public static class Bases
        {
            public const byte LD_R_R = 0b01000000;                // Base for LD r,r' opcodes   
            public const byte LD_R_N = 0b00000110;                // Base for LD r,n opcodes
            public const byte LD_R_NN = 0b00000001;               // Base for LD r,nn opcodes
            public const byte LD_R_HLRef = 0b01000110;             // Base for LD r,(HL) opcodes
            public const byte LD_HLRef_R = 0b01110000;               // Base for LD (HL),r opcodes  
            public const byte LD_R_Displacement = 0b01000110;      // Base for LD r,(IX|IY + n) opcodes
            public const byte LD_Displacement_R = 0b01110000;       // Base for LD (IX|IY + n),r opcodes
            public const byte LD_DD_NNRef = 0b01001011;            // Base for LD BC|DE|SP,(nn) opcodes
            public static byte LD_NNRef_DD = 0b01000011;              // Base for LD (nn),BC|DE|SP opcodes

            public const byte ADD_A_R = 0b10000000;               // Base for ADD A,r
        }
        
        public static class Prefixes
        {
            public const byte DD = 0xdd;
            public const byte FD = 0xfd;
            public const byte ED = 0xed;
        }
    }
}
