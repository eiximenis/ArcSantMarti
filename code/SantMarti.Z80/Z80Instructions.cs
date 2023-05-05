using SantMarti.Z80.Instructions;

namespace SantMarti.Z80

{
    /// <summary>
    /// This class contains ALL z80 instructions (main and variants).
    /// </summary>
    class Z80Instructions
    {

        private readonly Instruction[] _unprefixed;
        private readonly Instruction[] _cbPrefixed;
        private readonly Instruction[] _ddPrefixed;
        private readonly Instruction[] _edPrefixed;
        private readonly Instruction[] _fdPrefixed;
        private readonly Instruction[] _ddcbPrefixed;
        private readonly Instruction[] _fdcbPrefixed;
        

        public Z80Instructions()
        {
            _unprefixed = CreateUnprefixedInstructions().ToArray();
            _cbPrefixed = CreateCBPrefixedInstructions().ToArray();
            _edPrefixed = AddMissingNOPs(CreateEDPrefixedInstructions());
            _ddPrefixed = AddMissingUnprefixed(CreateDDPrefixedInstructions(), _unprefixed);
            _fdPrefixed = AddMissingUnprefixed(CreateFDPrefixedInstructions(), _unprefixed);
            _ddcbPrefixed = CreateDDCBPrefixedInstructions().ToArray();
            _fdcbPrefixed = CreateFDCBPrefixedInstructions().ToArray();
        }

        public Instruction this[byte opcode, OpcodePrefix prefix] => prefix switch
        {
            OpcodePrefix.CB => _cbPrefixed[opcode],
            OpcodePrefix.DD => _ddPrefixed[opcode],
            OpcodePrefix.ED => _edPrefixed[opcode],
            OpcodePrefix.FD => _fdPrefixed[opcode],
            OpcodePrefix.DDCB => _ddcbPrefixed[opcode],
            OpcodePrefix.FDCB => _fdcbPrefixed[opcode],
            _ => _unprefixed[opcode]
        };
        

        private IEnumerable<Instruction> CreateUnprefixedInstructions()
        {
            var unprefixed = new List<Instruction>();
            // For x = 0 and z=0 (Relative jumps and assorted ops)
            unprefixed.Add(new Instruction(0x0, "NOP", 4, Nop.NOP));
            unprefixed.Add(new Instruction(0x1, "LD BC,nn", 10));
            unprefixed.Add(new Instruction(0x2, "LD (BC),A", 7));
            unprefixed.Add(new Instruction(0x3, "INC BC", 6));
            unprefixed.Add(new Instruction(0x4, "INC B", 4 , Increment.INC_B));
            unprefixed.Add(new Instruction(0x5, "DEC B", 4));
            unprefixed.Add(new Instruction(0x6, "LD B,n", 7));
            unprefixed.Add(new Instruction(0x7, "RLCA", 4));
            unprefixed.Add(new Instruction(0x8, "EX AF, AF'", 4, Exchange.EXAFAF2));
            unprefixed.Add(new Instruction(0x9, "ADD HL,BC", 11));
            unprefixed.Add(new Instruction(0xa, "LD A,(BC)", 7));
            unprefixed.Add(new Instruction(0xb, "DEC BC", 6));
            unprefixed.Add(new Instruction(0xc, "INC C", 4, Increment.INC_C));
            unprefixed.Add(new Instruction(0xd, "DEC C", 4));
            unprefixed.Add(new Instruction(0xe, "LD C,n", 7));
            unprefixed.Add(new Instruction(0xf, "RRCA", 4));
            
            unprefixed.Add(new Instruction(0x10, "DJNZ d", 13, 8, Jump.DZNZ));
            unprefixed.Add(new Instruction(0x11, "LD DE,nn", 10));
            unprefixed.Add(new Instruction(0x12, "LD (DE),A", 7));
            unprefixed.Add(new Instruction(0x13, "INC DE", 6));
            unprefixed.Add(new Instruction(0x14, "INC D", 4, Increment.INC_D));
            unprefixed.Add(new Instruction(0x15, "DEC D", 4));
            unprefixed.Add(new Instruction(0x16, "LD D,n", 7));
            unprefixed.Add(new Instruction(0x17, "RLA", 4));
            unprefixed.Add(new Instruction(0x18, "JR d", 12));
            unprefixed.Add(new Instruction(0x19, "ADD HL,DE", 11));
            unprefixed.Add(new Instruction(0x1a, "LD A,(DE)", 7));
            unprefixed.Add(new Instruction(0x1b, "DEC DE", 6));
            unprefixed.Add(new Instruction(0x1c, "INC E", 4, Increment.INC_E));
            unprefixed.Add(new Instruction(0x1d, "DEC E", 4));
            unprefixed.Add(new Instruction(0x1e, "LD E,n", 7));
            unprefixed.Add(new Instruction(0x1f, "RRA", 4));

            unprefixed.Add(new Instruction(0x20, "JR NZ,d", 12));
            unprefixed.Add(new Instruction(0x21, "LD HL,nn", 10));
            unprefixed.Add(new Instruction(0x22, "LD (nn),HL", 16));
            unprefixed.Add(new Instruction(0x23, "INC HL", 6));
            unprefixed.Add(new Instruction(0x24, "INC H", 4, Increment.INC_H));
            unprefixed.Add(new Instruction(0x25, "DEC H", 4));
            unprefixed.Add(new Instruction(0x26, "LD H,n", 7));
            unprefixed.Add(new Instruction(0x27, "DAA", 4, Daa.DAA));
            unprefixed.Add(new Instruction(0x28, "JR Z,d", 12));
            unprefixed.Add(new Instruction(0x29, "ADD HL,HL", 11));
            unprefixed.Add(new Instruction(0x2a, "LD HL,(nn)", 16));
            unprefixed.Add(new Instruction(0x2b, "DEC HL", 6));
            unprefixed.Add(new Instruction(0x2c, "INC L", 4, Increment.INC_L));
            unprefixed.Add(new Instruction(0x2d, "DEC L", 4));
            unprefixed.Add(new Instruction(0x2e, "LD L,n", 7));
            unprefixed.Add(new Instruction(0x2f, "CPL", 4));

            unprefixed.Add(new Instruction(0x30, "JR NC,d", 12));
            unprefixed.Add(new Instruction(0x31, "LD SP,nn", 10));
            unprefixed.Add(new Instruction(0x32, "LD (nn),A", 13));
            unprefixed.Add(new Instruction(0x33, "INC SP", 6));
            unprefixed.Add(new Instruction(0x34, "INC (HL)", 7));
            unprefixed.Add(new Instruction(0x35, "DEC (HL)", 7));
            unprefixed.Add(new Instruction(0x36, "LD (HL),n", 10));
            unprefixed.Add(new Instruction(0x37, "SCF", 4));
            unprefixed.Add(new Instruction(0x38, "JR C,d", 12));
            unprefixed.Add(new Instruction(0x39, "ADD HL,SP", 11));
            unprefixed.Add(new Instruction(0x3a, "LD A,(nn)", 13, Load.LD_A_NN));
            unprefixed.Add(new Instruction(0x3b, "DEC SP", 6));
            unprefixed.Add(new Instruction(0x3c, "INC A", 4, Increment.INC_A));
            unprefixed.Add(new Instruction(0x3d, "DEC A", 4));
            unprefixed.Add(new Instruction(0x3e, "LD A,n", 7));
            unprefixed.Add(new Instruction(0x3f, "CCF", 4));

            unprefixed.Add(new Instruction(0x40, "LD B,B", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x41, "LD B,C", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x42, "LD B,D", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x43, "LD B,E", 4, Load.LD_R_R));    
            unprefixed.Add(new Instruction(0x44, "LD B,H", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x45, "LD B,L", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x46, "LD B,(HL)", 7, Load.LD_R_HLRef));
            unprefixed.Add(new Instruction(0x47, "LD B,A", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x48, "LD C,B", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x49, "LD C,C", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x4a, "LD C,D", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x4b, "LD C,E", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x4c, "LD C,H", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x4d, "LD C,L", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x4e, "LD C,(HL)", 7,Load.LD_R_HLRef));
            unprefixed.Add(new Instruction(0x4f, "LD C,A", 4, Load.LD_R_R));

            unprefixed.Add(new Instruction(0x50, "LD D,B", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x51, "LD D,C", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x52, "LD D,D", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x53, "LD D,E", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x54, "LD D,H", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x55, "LD D,L", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x56, "LD D,(HL)", 7, Load.LD_R_HLRef));
            unprefixed.Add(new Instruction(0x57, "LD D,A", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x58, "LD E,B", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x59, "LD E,C", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x5a, "LD E,D", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x5b, "LD E,E", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x5c, "LD E,H", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x5d, "LD E,L", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x5e, "LD E,(HL)", 7, Load.LD_R_HLRef));
            unprefixed.Add(new Instruction(0x5f, "LD E,A", 4, Load.LD_R_R));

            unprefixed.Add(new Instruction(0x60, "LD H,B", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x61, "LD H,C", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x62, "LD H,D", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x63, "LD H,E", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x64, "LD H,H", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x65, "LD H,L", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x66, "LD H,(HL)", 7, Load.LD_R_HLRef));
            unprefixed.Add(new Instruction(0x67, "LD H,A", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x68, "LD L,B", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x69, "LD L,C", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x6a, "LD L,D", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x6b, "LD L,E", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x6c, "LD L,H", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x6d, "LD L,L", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x6e, "LD L,(HL)", 7, Load.LD_R_HLRef));
            unprefixed.Add(new Instruction(0x6f, "LD L,A", 4, Load.LD_R_R));

            unprefixed.Add(new Instruction(0x70, "LD (HL),B", 7, Load.LD_HLRef_R));
            unprefixed.Add(new Instruction(0x71, "LD (HL),C", 7, Load.LD_HLRef_R));
            unprefixed.Add(new Instruction(0x72, "LD (HL),D", 7, Load.LD_HLRef_R));
            unprefixed.Add(new Instruction(0x73, "LD (HL),E", 7, Load.LD_HLRef_R));
            unprefixed.Add(new Instruction(0x74, "LD (HL),H",7 , Load.LD_HLRef_R));
            unprefixed.Add(new Instruction(0x75, "LD (HL),L", 7, Load.LD_HLRef_R));
            unprefixed.Add(new Instruction(0x76, "HALT", 4, Nop.HALT));
            unprefixed.Add(new Instruction(0x77, "LD (HL),A", 7, Load.LD_HLRef_R));
            unprefixed.Add(new Instruction(0x78, "LD A,B", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x79, "LD A,C", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x7a, "LD A,D", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x7b, "LD A,E", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x7c, "LD A,H", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x7d, "LD A,L", 4, Load.LD_R_R));
            unprefixed.Add(new Instruction(0x7e, "LD A,(HL)", 7, Load.LD_A_HLRef));
            unprefixed.Add(new Instruction(0x7f, "LD A,A", 4, Load.LD_R_R));

            unprefixed.Add(new Instruction(0x80, "ADD A,B", 4, Add.Add_R));
            unprefixed.Add(new Instruction(0x81, "ADD A,C", 4, Add.Add_R));
            unprefixed.Add(new Instruction(0x82, "ADD A,D", 4, Add.Add_R));
            unprefixed.Add(new Instruction(0x83, "ADD A,E", 4, Add.Add_R));
            unprefixed.Add(new Instruction(0x84, "ADD A,H", 4, Add.Add_R));
            unprefixed.Add(new Instruction(0x85, "ADD A,L", 4, Add.Add_R));
            unprefixed.Add(new Instruction(0x86, "ADD A,(HL)", 7, Add.Add_HLRef));
            unprefixed.Add(new Instruction(0x87, "ADD A,A", 4, Add.Add_R));
            unprefixed.Add(new Instruction(0x88, "ADC A,B", 4, Add.Add_R));
            unprefixed.Add(new Instruction(0x89, "ADC A,C", 4, Add.Add_R));
            unprefixed.Add(new Instruction(0x8a, "ADC A,D", 4, Add.Add_R));
            unprefixed.Add(new Instruction(0x8b, "ADC A,E", 4, Add.Add_R));
            unprefixed.Add(new Instruction(0x8c, "ADC A,H", 4, Add.Add_R));
            unprefixed.Add(new Instruction(0x8d, "ADC A,L", 4, Add.Add_R));
            unprefixed.Add(new Instruction(0x8e, "ADC A,(HL)", 7));
            unprefixed.Add(new Instruction(0x8f, "ADC A,A", 4));

            unprefixed.Add(new Instruction(0x90, "SUB B", 4));
            unprefixed.Add(new Instruction(0x91, "SUB C", 4));
            unprefixed.Add(new Instruction(0x92, "SUB D", 4));
            unprefixed.Add(new Instruction(0x93, "SUB E", 4));
            unprefixed.Add(new Instruction(0x94, "SUB H", 4));
            unprefixed.Add(new Instruction(0x95, "SUB L", 4));
            unprefixed.Add(new Instruction(0x96, "SUB (HL)", 7));
            unprefixed.Add(new Instruction(0x97, "SUB A", 4));
            unprefixed.Add(new Instruction(0x98, "SBC A,B", 4));
            unprefixed.Add(new Instruction(0x99, "SBC A,C", 4));
            unprefixed.Add(new Instruction(0x9a, "SBC A,D", 4));
            unprefixed.Add(new Instruction(0x9b, "SBC A,E", 4));
            unprefixed.Add(new Instruction(0x9c, "SBC A,H", 4));
            unprefixed.Add(new Instruction(0x9d, "SBC A,L", 4));
            unprefixed.Add(new Instruction(0x9e, "SBC A,(HL)", 7));
            unprefixed.Add(new Instruction(0x9f, "SBC A,A", 4));

            unprefixed.Add(new Instruction(0xa0, "AND B", 4, Logical.AND_R));
            unprefixed.Add(new Instruction(0xa1, "AND C", 4, Logical.AND_R));
            unprefixed.Add(new Instruction(0xa2, "AND D", 4, Logical.AND_R));
            unprefixed.Add(new Instruction(0xa3, "AND E", 4, Logical.AND_R));
            unprefixed.Add(new Instruction(0xa4, "AND H", 4, Logical.AND_R));
            unprefixed.Add(new Instruction(0xa5, "AND L", 4, Logical.AND_R));
            unprefixed.Add(new Instruction(0xa6, "AND (HL)", 7, Logical.AND_HLRef));
            unprefixed.Add(new Instruction(0xa7, "AND A", 4, Logical.AND_R));
            unprefixed.Add(new Instruction(0xa8, "XOR B", 4, Logical.XOR_R));
            unprefixed.Add(new Instruction(0xa9, "XOR C", 4, Logical.XOR_R));
            unprefixed.Add(new Instruction(0xaa, "XOR D", 4, Logical.XOR_R));
            unprefixed.Add(new Instruction(0xab, "XOR E", 4, Logical.XOR_R));
            unprefixed.Add(new Instruction(0xac, "XOR H", 4, Logical.XOR_R));
            unprefixed.Add(new Instruction(0xad, "XOR L", 4, Logical.XOR_R));
            unprefixed.Add(new Instruction(0xae, "XOR (HL)", 7, Logical.XOR_HLRef));
            unprefixed.Add(new Instruction(0xaf, "XOR A", 4, Logical.XOR_R));

            unprefixed.Add(new Instruction(0xb0, "OR B", 4, Logical.OR_R));
            unprefixed.Add(new Instruction(0xb1, "OR C", 4, Logical.OR_R));
            unprefixed.Add(new Instruction(0xb2, "OR D", 4, Logical.OR_R));
            unprefixed.Add(new Instruction(0xb3, "OR E", 4, Logical.OR_R));
            unprefixed.Add(new Instruction(0xb4, "OR H", 4, Logical.OR_R));
            unprefixed.Add(new Instruction(0xb5, "OR L", 4, Logical.OR_R));
            unprefixed.Add(new Instruction(0xb6, "OR (HL)", 7, Logical.OR_HLRef));
            unprefixed.Add(new Instruction(0xb7, "OR A", 4, Logical.OR_R));
            unprefixed.Add(new Instruction(0xb8, "CP B", 4));
            unprefixed.Add(new Instruction(0xb9, "CP C", 4));
            unprefixed.Add(new Instruction(0xba, "CP D", 4));
            unprefixed.Add(new Instruction(0xbb, "CP E", 4));
            unprefixed.Add(new Instruction(0xbc, "CP H", 4));
            unprefixed.Add(new Instruction(0xbd, "CP L", 4));
            unprefixed.Add(new Instruction(0xbe, "CP (HL)", 7));
            unprefixed.Add(new Instruction(0xbf, "CP A", 4));

            unprefixed.Add(new Instruction(0xc0, "RET NZ", 11, 5));   
            unprefixed.Add(new Instruction(0xc1, "POP BC", 10, Stack.POPBC));
            unprefixed.Add(new Instruction(0xc2, "JP NZ,nn", 10, 10));
            unprefixed.Add(new Instruction(0xc3, "JP nn", 10, Jump.JP_NN));
            unprefixed.Add(new Instruction(0xc4, "CALL NZ,nn", 17));
            unprefixed.Add(new Instruction(0xc5, "PUSH BC", 11, Stack.PUSHBC));
            unprefixed.Add(new Instruction(0xc6, "ADD A,n", 7, Add.Add_N));
            unprefixed.Add(new Instruction(0xc7, "RST 00", 11));
            unprefixed.Add(new Instruction(0xc8, "RET Z", 11, 5));
            unprefixed.Add(new Instruction(0xc9, "RET", 10));
            unprefixed.Add(new Instruction(0xca, "JP Z,nn", 10, 10));
            unprefixed.Add(new Instruction(0xcb, "-- CB --", 0));           // CB Prefix
            unprefixed.Add(new Instruction(0xcc, "CALL Z,nn", 17));
            unprefixed.Add(new Instruction(0xcd, "CALL nn", 17));
            unprefixed.Add(new Instruction(0xce, "ADC A,n", 7, Add.Adc_N));
            unprefixed.Add(new Instruction(0xcf, "RST 08", 11));

            unprefixed.Add(new Instruction(0xd0, "RET NC", 11,5 ));
            unprefixed.Add(new Instruction(0xd1, "POP DE", 10, Stack.POPDE));
            unprefixed.Add(new Instruction(0xd2, "JP NC,nn", 10, 10));
            unprefixed.Add(new Instruction(0xd3, "OUT (n),A", 11));
            unprefixed.Add(new Instruction(0xd4, "CALL NC,nn", 17));
            unprefixed.Add(new Instruction(0xd5, "PUSH DE", 11, Stack.PUSHDE));
            unprefixed.Add(new Instruction(0xd6, "SUB n", 7));
            unprefixed.Add(new Instruction(0xd7, "RST 10", 11));
            unprefixed.Add(new Instruction(0xd8, "RET C", 11, 5));
            unprefixed.Add(new Instruction(0xd9, "EXX", 4, Exchange.EXX));
            unprefixed.Add(new Instruction(0xda, "JP C,nn", 10));
            unprefixed.Add(new Instruction(0xdb, "IN A,(n)", 11));
            unprefixed.Add(new Instruction(0xdc, "CALL C,nn", 17));
            unprefixed.Add(new Instruction(0xdd, "-- DD --", 0));       // DD Prefix
            unprefixed.Add(new Instruction(0xde, "SBC A,n", 7));
            unprefixed.Add(new Instruction(0xdf, "RST 18", 11));

            unprefixed.Add(new Instruction(0xe0, "RET PO", 11, 5));
            unprefixed.Add(new Instruction(0xe1, "POP HL", 10, Stack.POPHL));
            unprefixed.Add(new Instruction(0xe2, "JP PO,nn", 10));
            unprefixed.Add(new Instruction(0xe3, "EX (SP),HL", 19));
            unprefixed.Add(new Instruction(0xe4, "CALL PO,nn", 17));
            unprefixed.Add(new Instruction(0xe5, "PUSH HL", 11, Stack.PUSHL));
            unprefixed.Add(new Instruction(0xe6, "AND n", 7, Logical.AND_N));
            unprefixed.Add(new Instruction(0xe7, "RST 20", 11));
            unprefixed.Add(new Instruction(0xe8, "RET PE", 11, 5));
            unprefixed.Add(new Instruction(0xe9, "JP (HL)", 4, Jump.JP_HL));
            unprefixed.Add(new Instruction(0xea, "JP PE,nn", 10));
            unprefixed.Add(new Instruction(0xeb, "EX DE,HL", 4, Exchange.EXDEHL));
            unprefixed.Add(new Instruction(0xec, "CALL PE,nn", 17));
            unprefixed.Add(new Instruction(0xed, "-- ED --", 0));       // ED Prefix
            unprefixed.Add(new Instruction(0xee, "XOR n", 7));
            unprefixed.Add(new Instruction(0xef, "RST 28", 11));

            unprefixed.Add(new Instruction(0xf0, "RET P", 11, 5));
            unprefixed.Add(new Instruction(0xf1, "POP AF", 10, Stack.POPAF));
            unprefixed.Add(new Instruction(0xf2, "JP P,nn", 10, 10));
            unprefixed.Add(new Instruction(0xf3, "DI", 4));
            unprefixed.Add(new Instruction(0xf4, "CALL P,nn", 17, 10));
            unprefixed.Add(new Instruction(0xf5, "PUSH AF", 11, Stack.PUSHAF));
            unprefixed.Add(new Instruction(0xf6, "OR n", 7));
            unprefixed.Add(new Instruction(0xf7, "RST 30", 11));
            unprefixed.Add(new Instruction(0xf8, "RET M", 11, 5));
            unprefixed.Add(new Instruction(0xf9, "LD SP,HL", 6));
            unprefixed.Add(new Instruction(0xfa, "JP M,nn", 10, 10));
            unprefixed.Add(new Instruction(0xfb, "EI", 4));
            unprefixed.Add(new Instruction(0xfc, "CALL M,nn", 17, 10));
            unprefixed.Add(new Instruction(0xfd, "-- FD --", 0));       // FD Prefix
            unprefixed.Add(new Instruction(0xfe, "CP n", 7));
            unprefixed.Add(new Instruction(0xff, "RST 38", 11));


            return unprefixed;
        }
        private IEnumerable<Instruction> CreateCBPrefixedInstructions()
        {
            var cbPrefixed = new List<Instruction>();

            cbPrefixed.Add(new Instruction(0x00, "RLC B", 8));
            cbPrefixed.Add(new Instruction(0x01, "RLC C", 8));
            cbPrefixed.Add(new Instruction(0x02, "RLC D", 8));
            cbPrefixed.Add(new Instruction(0x03, "RLC E", 8));
            cbPrefixed.Add(new Instruction(0x04, "RLC H", 8));
            cbPrefixed.Add(new Instruction(0x05, "RLC L", 8));
            cbPrefixed.Add(new Instruction(0x06, "RLC (HL)", 15));
            cbPrefixed.Add(new Instruction(0x07, "RLC A", 8));
            cbPrefixed.Add(new Instruction(0x08, "RRC B", 8));
            cbPrefixed.Add(new Instruction(0x09, "RRC C", 8));
            cbPrefixed.Add(new Instruction(0x0a, "RRC D", 8));
            cbPrefixed.Add(new Instruction(0x0b, "RRC E", 8));
            cbPrefixed.Add(new Instruction(0x0c, "RRC H", 8));
            cbPrefixed.Add(new Instruction(0x0d, "RRC L", 8));
            cbPrefixed.Add(new Instruction(0x0e, "RRC (HL)", 15));
            cbPrefixed.Add(new Instruction(0x0f, "RRC A", 8));

            cbPrefixed.Add(new Instruction(0x10, "RL B", 8));
            cbPrefixed.Add(new Instruction(0x11, "RL C", 8));
            cbPrefixed.Add(new Instruction(0x12, "RL D", 8));
            cbPrefixed.Add(new Instruction(0x13, "RL E", 8));
            cbPrefixed.Add(new Instruction(0x14, "RL H", 8));
            cbPrefixed.Add(new Instruction(0x15, "RL L", 8));
            cbPrefixed.Add(new Instruction(0x16, "RL (HL)", 15));
            cbPrefixed.Add(new Instruction(0x17, "RL A", 8));
            cbPrefixed.Add(new Instruction(0x18, "RR B", 8));
            cbPrefixed.Add(new Instruction(0x19, "RR C", 8));
            cbPrefixed.Add(new Instruction(0x1a, "RR D", 8));
            cbPrefixed.Add(new Instruction(0x1b, "RR E", 8));
            cbPrefixed.Add(new Instruction(0x1c, "RR H", 8));
            cbPrefixed.Add(new Instruction(0x1d, "RR L", 8));
            cbPrefixed.Add(new Instruction(0x1e, "RR (HL)", 15));
            cbPrefixed.Add(new Instruction(0x1f, "RR A", 8));

            cbPrefixed.Add(new Instruction(0x20, "SLA B", 8));
            cbPrefixed.Add(new Instruction(0x21, "SLA C", 8));
            cbPrefixed.Add(new Instruction(0x22, "SLA D", 8));
            cbPrefixed.Add(new Instruction(0x23, "SLA E", 8));
            cbPrefixed.Add(new Instruction(0x24, "SLA H", 8));
            cbPrefixed.Add(new Instruction(0x25, "SLA L", 8));
            cbPrefixed.Add(new Instruction(0x26, "SLA (HL)", 15));
            cbPrefixed.Add(new Instruction(0x27, "SLA A", 8));
            cbPrefixed.Add(new Instruction(0x28, "SRA B", 8));
            cbPrefixed.Add(new Instruction(0x29, "SRA C", 8));
            cbPrefixed.Add(new Instruction(0x2a, "SRA D", 8));
            cbPrefixed.Add(new Instruction(0x2b, "SRA E", 8));
            cbPrefixed.Add(new Instruction(0x2c, "SRA H", 8));
            cbPrefixed.Add(new Instruction(0x2d, "SRA L", 8));
            cbPrefixed.Add(new Instruction(0x2e, "SRA (HL)", 15));
            cbPrefixed.Add(new Instruction(0x2f, "SRA A", 8));

            cbPrefixed.Add(new Instruction(0x30, "SLS B", 8));
            cbPrefixed.Add(new Instruction(0x31, "SLS C", 8));
            cbPrefixed.Add(new Instruction(0x32, "SLS D", 8));
            cbPrefixed.Add(new Instruction(0x33, "SLS E", 8));
            cbPrefixed.Add(new Instruction(0x34, "SLS H", 8));
            cbPrefixed.Add(new Instruction(0x35, "SLS L", 8));
            cbPrefixed.Add(new Instruction(0x36, "SLS (HL)", 15));
            cbPrefixed.Add(new Instruction(0x37, "SLS A", 8));
            cbPrefixed.Add(new Instruction(0x38, "SRL B", 8));
            cbPrefixed.Add(new Instruction(0x39, "SRL C", 8));
            cbPrefixed.Add(new Instruction(0x3a, "SRL D", 8));
            cbPrefixed.Add(new Instruction(0x3b, "SRL E", 8));
            cbPrefixed.Add(new Instruction(0x3c, "SRL H", 8));
            cbPrefixed.Add(new Instruction(0x3d, "SRL L", 8));
            cbPrefixed.Add(new Instruction(0x3e, "SRL (HL)", 15));
            cbPrefixed.Add(new Instruction(0x3f, "SRL A", 8));

            cbPrefixed.Add(new Instruction(0x40, "BIT 0,B", 8));
            cbPrefixed.Add(new Instruction(0x41, "BIT 0,C", 8));
            cbPrefixed.Add(new Instruction(0x42, "BIT 0,D", 8));
            cbPrefixed.Add(new Instruction(0x43, "BIT 0,E", 8));
            cbPrefixed.Add(new Instruction(0x44, "BIT 0,H", 8));
            cbPrefixed.Add(new Instruction(0x45, "BIT 0,L", 8));
            cbPrefixed.Add(new Instruction(0x46, "BIT 0,(HL)", 12));
            cbPrefixed.Add(new Instruction(0x47, "BIT 0,A", 8));
            cbPrefixed.Add(new Instruction(0x48, "BIT 1,B", 8));
            cbPrefixed.Add(new Instruction(0x49, "BIT 1,C", 8));
            cbPrefixed.Add(new Instruction(0x4a, "BIT 1,D", 8));
            cbPrefixed.Add(new Instruction(0x4b, "BIT 1,E", 8));
            cbPrefixed.Add(new Instruction(0x4c, "BIT 1,H", 8));
            cbPrefixed.Add(new Instruction(0x4d, "BIT 1,L", 8));
            cbPrefixed.Add(new Instruction(0x4e, "BIT 1,(HL)", 12));
            cbPrefixed.Add(new Instruction(0x4f, "BIT 1,A", 8));

            cbPrefixed.Add(new Instruction(0x50, "BIT 2,B", 8));
            cbPrefixed.Add(new Instruction(0x51, "BIT 2,C", 8));
            cbPrefixed.Add(new Instruction(0x52, "BIT 2,D", 8));
            cbPrefixed.Add(new Instruction(0x53, "BIT 2,E", 8));
            cbPrefixed.Add(new Instruction(0x54, "BIT 2,H", 8));
            cbPrefixed.Add(new Instruction(0x55, "BIT 2,L", 8));
            cbPrefixed.Add(new Instruction(0x56, "BIT 2,(HL)", 12));
            cbPrefixed.Add(new Instruction(0x57, "BIT 2,A", 8));
            cbPrefixed.Add(new Instruction(0x58, "BIT 3,B", 8));
            cbPrefixed.Add(new Instruction(0x59, "BIT 3,C", 8));
            cbPrefixed.Add(new Instruction(0x5a, "BIT 3,D", 8));
            cbPrefixed.Add(new Instruction(0x5b, "BIT 3,E", 8));
            cbPrefixed.Add(new Instruction(0x5c, "BIT 3,H", 8));
            cbPrefixed.Add(new Instruction(0x5d, "BIT 3,L", 8));
            cbPrefixed.Add(new Instruction(0x5e, "BIT 3,(HL)", 12));
            cbPrefixed.Add(new Instruction(0x5f, "BIT 3,A", 8));

            cbPrefixed.Add(new Instruction(0x60, "BIT 4,B", 8));
            cbPrefixed.Add(new Instruction(0x61, "BIT 4,C", 8));
            cbPrefixed.Add(new Instruction(0x62, "BIT 4,D", 8));
            cbPrefixed.Add(new Instruction(0x63, "BIT 4,E", 8));
            cbPrefixed.Add(new Instruction(0x64, "BIT 4,H", 8));
            cbPrefixed.Add(new Instruction(0x65, "BIT 4,L", 8));
            cbPrefixed.Add(new Instruction(0x66, "BIT 4,(HL)", 12));
            cbPrefixed.Add(new Instruction(0x67, "BIT 4,A", 8));
            cbPrefixed.Add(new Instruction(0x68, "BIT 5,B", 8));
            cbPrefixed.Add(new Instruction(0x69, "BIT 5,C", 8));
            cbPrefixed.Add(new Instruction(0x6a, "BIT 5,D", 8));
            cbPrefixed.Add(new Instruction(0x6b, "BIT 5,E", 8));
            cbPrefixed.Add(new Instruction(0x6c, "BIT 5,H", 8));
            cbPrefixed.Add(new Instruction(0x6d, "BIT 5,L", 8));
            cbPrefixed.Add(new Instruction(0x6e, "BIT 5,(HL)", 12));
            cbPrefixed.Add(new Instruction(0x6f, "BIT 5,A", 8));

            cbPrefixed.Add(new Instruction(0x70, "BIT 6,B", 8));
            cbPrefixed.Add(new Instruction(0x71, "BIT 6,C", 8));
            cbPrefixed.Add(new Instruction(0x72, "BIT 6,D", 8));
            cbPrefixed.Add(new Instruction(0x73, "BIT 6,E", 8));
            cbPrefixed.Add(new Instruction(0x74, "BIT 6,H", 8));
            cbPrefixed.Add(new Instruction(0x75, "BIT 6,L", 8));
            cbPrefixed.Add(new Instruction(0x76, "BIT 6,(HL)", 12));
            cbPrefixed.Add(new Instruction(0x77, "BIT 6,A", 8));
            cbPrefixed.Add(new Instruction(0x78, "BIT 7,B", 8));
            cbPrefixed.Add(new Instruction(0x79, "BIT 7,C", 8));
            cbPrefixed.Add(new Instruction(0x7a, "BIT 7,D", 8));
            cbPrefixed.Add(new Instruction(0x7b, "BIT 7,E", 8));
            cbPrefixed.Add(new Instruction(0x7c, "BIT 7,H", 8));
            cbPrefixed.Add(new Instruction(0x7d, "BIT 7,L", 8));
            cbPrefixed.Add(new Instruction(0x7e, "BIT 7,(HL)", 12));
            cbPrefixed.Add(new Instruction(0x7f, "BIT 7,A", 8));

            cbPrefixed.Add(new Instruction(0x80, "RES 0,B", 8));
            cbPrefixed.Add(new Instruction(0x81, "RES 0,C", 8));
            cbPrefixed.Add(new Instruction(0x82, "RES 0,D", 8));
            cbPrefixed.Add(new Instruction(0x83, "RES 0,E", 8));
            cbPrefixed.Add(new Instruction(0x84, "RES 0,H", 8));
            cbPrefixed.Add(new Instruction(0x85, "RES 0,L", 8));
            cbPrefixed.Add(new Instruction(0x86, "RES 0,(HL)", 15));
            cbPrefixed.Add(new Instruction(0x87, "RES 0,A", 8));
            cbPrefixed.Add(new Instruction(0x88, "RES 1,B", 8));
            cbPrefixed.Add(new Instruction(0x89, "RES 1,C", 8));
            cbPrefixed.Add(new Instruction(0x8a, "RES 1,D", 8));
            cbPrefixed.Add(new Instruction(0x8b, "RES 1,E", 8));
            cbPrefixed.Add(new Instruction(0x8c, "RES 1,H", 8));
            cbPrefixed.Add(new Instruction(0x8d, "RES 1,L", 8));
            cbPrefixed.Add(new Instruction(0x8e, "RES 1,(HL)", 15));
            cbPrefixed.Add(new Instruction(0x8f, "RES 1,A", 8));

            cbPrefixed.Add(new Instruction(0x90, "RES 2,B", 8));
            cbPrefixed.Add(new Instruction(0x91, "RES 2,C", 8));
            cbPrefixed.Add(new Instruction(0x92, "RES 2,D", 8));
            cbPrefixed.Add(new Instruction(0x93, "RES 2,E", 8));
            cbPrefixed.Add(new Instruction(0x94, "RES 2,H", 8));
            cbPrefixed.Add(new Instruction(0x95, "RES 2,L", 8));
            cbPrefixed.Add(new Instruction(0x96, "RES 2,(HL)", 15));
            cbPrefixed.Add(new Instruction(0x97, "RES 2,A", 8));
            cbPrefixed.Add(new Instruction(0x98, "RES 3,B", 8));
            cbPrefixed.Add(new Instruction(0x99, "RES 3,C", 8));
            cbPrefixed.Add(new Instruction(0x9a, "RES 3,D", 8));
            cbPrefixed.Add(new Instruction(0x9b, "RES 3,E", 8));
            cbPrefixed.Add(new Instruction(0x9c, "RES 3,H", 8));
            cbPrefixed.Add(new Instruction(0x9d, "RES 3,L", 8));
            cbPrefixed.Add(new Instruction(0x9e, "RES 3,(HL)", 15));
            cbPrefixed.Add(new Instruction(0x9f, "RES 3,A", 8));

            cbPrefixed.Add(new Instruction(0xa0, "RES 4,B", 8));
            cbPrefixed.Add(new Instruction(0xa1, "RES 4,C", 8));
            cbPrefixed.Add(new Instruction(0xa2, "RES 4,D", 8));
            cbPrefixed.Add(new Instruction(0xa3, "RES 4,E", 8));
            cbPrefixed.Add(new Instruction(0xa4, "RES 4,H", 8));
            cbPrefixed.Add(new Instruction(0xa5, "RES 4,L", 8));
            cbPrefixed.Add(new Instruction(0xa6, "RES 4,(HL)", 15));
            cbPrefixed.Add(new Instruction(0xa7, "RES 4,A", 8));
            cbPrefixed.Add(new Instruction(0xa8, "RES 5,B", 8));
            cbPrefixed.Add(new Instruction(0xa9, "RES 5,C", 8));
            cbPrefixed.Add(new Instruction(0xaa, "RES 5,D", 8));
            cbPrefixed.Add(new Instruction(0xab, "RES 5,E", 8));
            cbPrefixed.Add(new Instruction(0xac, "RES 5,H", 8));
            cbPrefixed.Add(new Instruction(0xad, "RES 5,L", 8));
            cbPrefixed.Add(new Instruction(0xae, "RES 5,(HL)", 15));
            cbPrefixed.Add(new Instruction(0xaf, "RES 5,A", 8));

            cbPrefixed.Add(new Instruction(0xb0, "RES 6,B", 8));
            cbPrefixed.Add(new Instruction(0xb1, "RES 6,C", 8));
            cbPrefixed.Add(new Instruction(0xb2, "RES 6,D", 8));
            cbPrefixed.Add(new Instruction(0xb3, "RES 6,E", 8));
            cbPrefixed.Add(new Instruction(0xb4, "RES 6,H", 8));
            cbPrefixed.Add(new Instruction(0xb5, "RES 6,L", 8));
            cbPrefixed.Add(new Instruction(0xb6, "RES 6,(HL)", 15));
            cbPrefixed.Add(new Instruction(0xb7, "RES 6,A", 8));
            cbPrefixed.Add(new Instruction(0xb8, "RES 7,B", 8));
            cbPrefixed.Add(new Instruction(0xb9, "RES 7,C", 8));
            cbPrefixed.Add(new Instruction(0xba, "RES 7,D", 8));
            cbPrefixed.Add(new Instruction(0xbb, "RES 7,E", 8));
            cbPrefixed.Add(new Instruction(0xbc, "RES 7,H", 8));
            cbPrefixed.Add(new Instruction(0xbd, "RES 7,L", 8));
            cbPrefixed.Add(new Instruction(0xbe, "RES 7,(HL)", 15));
            cbPrefixed.Add(new Instruction(0xbf, "RES 7,A", 8));

            cbPrefixed.Add(new Instruction(0xc0, "SET 0,B", 8));
            cbPrefixed.Add(new Instruction(0xc1, "SET 0,C", 8));
            cbPrefixed.Add(new Instruction(0xc2, "SET 0,D", 8));
            cbPrefixed.Add(new Instruction(0xc3, "SET 0,E", 8));
            cbPrefixed.Add(new Instruction(0xc4, "SET 0,H", 8));
            cbPrefixed.Add(new Instruction(0xc5, "SET 0,L", 8));
            cbPrefixed.Add(new Instruction(0xc6, "SET 0,(HL)", 15));
            cbPrefixed.Add(new Instruction(0xc7, "SET 0,A", 8));
            cbPrefixed.Add(new Instruction(0xc8, "SET 1,B", 8));
            cbPrefixed.Add(new Instruction(0xc9, "SET 1,C", 8));
            cbPrefixed.Add(new Instruction(0xca, "SET 1,D", 8));
            cbPrefixed.Add(new Instruction(0xcb, "SET 1,E", 8));
            cbPrefixed.Add(new Instruction(0xcc, "SET 1,H", 8));
            cbPrefixed.Add(new Instruction(0xcd, "SET 1,L", 8));
            cbPrefixed.Add(new Instruction(0xce, "SET 1,(HL)", 15));
            cbPrefixed.Add(new Instruction(0xcf, "SET 1,A", 8));

            cbPrefixed.Add(new Instruction(0xd0, "SET 2,B", 8));
            cbPrefixed.Add(new Instruction(0xd1, "SET 2,C", 8));
            cbPrefixed.Add(new Instruction(0xd2, "SET 2,D", 8));
            cbPrefixed.Add(new Instruction(0xd3, "SET 2,E", 8));
            cbPrefixed.Add(new Instruction(0xd4, "SET 2,H", 8));
            cbPrefixed.Add(new Instruction(0xd5, "SET 2,L", 8));
            cbPrefixed.Add(new Instruction(0xd6, "SET 2,(HL)", 15));
            cbPrefixed.Add(new Instruction(0xd7, "SET 2,A", 8));
            cbPrefixed.Add(new Instruction(0xd8, "SET 3,B", 8));
            cbPrefixed.Add(new Instruction(0xd9, "SET 3,C", 8));
            cbPrefixed.Add(new Instruction(0xda, "SET 3,D", 8));
            cbPrefixed.Add(new Instruction(0xdb, "SET 3,E", 8));
            cbPrefixed.Add(new Instruction(0xdc, "SET 3,H", 8));
            cbPrefixed.Add(new Instruction(0xdd, "SET 3,L", 8));
            cbPrefixed.Add(new Instruction(0xde, "SET 3,(HL)", 15));
            cbPrefixed.Add(new Instruction(0xdf, "SET 3,A", 8));

            cbPrefixed.Add(new Instruction(0xe0, "SET 4,B", 8));
            cbPrefixed.Add(new Instruction(0xe1, "SET 4,C", 8));
            cbPrefixed.Add(new Instruction(0xe2, "SET 4,D", 8));
            cbPrefixed.Add(new Instruction(0xe3, "SET 4,E", 8));
            cbPrefixed.Add(new Instruction(0xe4, "SET 4,H", 8));
            cbPrefixed.Add(new Instruction(0xe5, "SET 4,L", 8));
            cbPrefixed.Add(new Instruction(0xe6, "SET 4,(HL)", 15));
            cbPrefixed.Add(new Instruction(0xe7, "SET 4,A", 8));
            cbPrefixed.Add(new Instruction(0xe8, "SET 5,B", 8));
            cbPrefixed.Add(new Instruction(0xe9, "SET 5,C", 8));
            cbPrefixed.Add(new Instruction(0xea, "SET 5,D", 8));
            cbPrefixed.Add(new Instruction(0xeb, "SET 5,E", 8));
            cbPrefixed.Add(new Instruction(0xec, "SET 5,H", 8));
            cbPrefixed.Add(new Instruction(0xed, "SET 5,L", 8));
            cbPrefixed.Add(new Instruction(0xee, "SET 5,(HL)", 15));
            cbPrefixed.Add(new Instruction(0xef, "SET 5,A", 8));

            cbPrefixed.Add(new Instruction(0xf0, "SET 6,B", 8));
            cbPrefixed.Add(new Instruction(0xf1, "SET 6,C", 8));
            cbPrefixed.Add(new Instruction(0xf2, "SET 6,D", 8));
            cbPrefixed.Add(new Instruction(0xf3, "SET 6,E", 8));
            cbPrefixed.Add(new Instruction(0xf4, "SET 6,H", 8));
            cbPrefixed.Add(new Instruction(0xf5, "SET 6,L", 8));
            cbPrefixed.Add(new Instruction(0xf6, "SET 6,(HL)", 15));
            cbPrefixed.Add(new Instruction(0xf7, "SET 6,A", 8));
            cbPrefixed.Add(new Instruction(0xf8, "SET 7,B", 8));
            cbPrefixed.Add(new Instruction(0xf9, "SET 7,C", 8));
            cbPrefixed.Add(new Instruction(0xfa, "SET 7,D", 8));
            cbPrefixed.Add(new Instruction(0xfb, "SET 7,E", 8));
            cbPrefixed.Add(new Instruction(0xfc, "SET 7,H", 8));
            cbPrefixed.Add(new Instruction(0xfd, "SET 7,L", 8));
            cbPrefixed.Add(new Instruction(0xfe, "SET 7,(HL)", 15));
            cbPrefixed.Add(new Instruction(0xff, "SET 7,A", 8));

            return cbPrefixed;
        }
        private IEnumerable<Instruction> CreateEDPrefixedInstructions()
        {
            var edPrefixed = new List<Instruction>();

            edPrefixed.Add(new Instruction(0x40, "IN B,(C)", 12));
            edPrefixed.Add(new Instruction(0x41, "OUT (C),B", 12));
            edPrefixed.Add(new Instruction(0x42, "SBC HL,BC", 15));
            edPrefixed.Add(new Instruction(0x43, "LD (nn),BC", 20));
            edPrefixed.Add(new Instruction(0x44, "NEG", 8));
            edPrefixed.Add(new Instruction(0x45, "RETN", 14));
            edPrefixed.Add(new Instruction(0x46, "IM 0", 8));
            edPrefixed.Add(new Instruction(0x47, "LD I,A", 9));
            edPrefixed.Add(new Instruction(0x48, "IN C,(C)", 12));
            edPrefixed.Add(new Instruction(0x49, "OUT (C),C", 12));
            edPrefixed.Add(new Instruction(0x4a, "ADC HL,BC", 15));
            edPrefixed.Add(new Instruction(0x4b, "LD BC,(nn)", 20));
            edPrefixed.Add(new Instruction(0x4c, "NEG", 8));
            edPrefixed.Add(new Instruction(0x4d, "RETI", 14));
            edPrefixed.Add(new Instruction(0x4e, "IM 0", 8));
            edPrefixed.Add(new Instruction(0x4f, "LD R,A", 9));

            edPrefixed.Add(new Instruction(0x50, "IN D,(C)", 12));
            edPrefixed.Add(new Instruction(0x51, "OUT (C),D", 12));
            edPrefixed.Add(new Instruction(0x52, "SBC HL,DE", 15));
            edPrefixed.Add(new Instruction(0x53, "LD (nn),DE", 20));
            edPrefixed.Add(new Instruction(0x54, "NEG", 8));
            edPrefixed.Add(new Instruction(0x55, "RETN", 14));
            edPrefixed.Add(new Instruction(0x56, "IM 1", 8));
            edPrefixed.Add(new Instruction(0x57, "LD A,I", 9));
            edPrefixed.Add(new Instruction(0x58, "IN E,(C)", 12));
            edPrefixed.Add(new Instruction(0x59, "OUT (C),E", 12));
            edPrefixed.Add(new Instruction(0x5a, "ADC HL,DE", 15));
            edPrefixed.Add(new Instruction(0x5b, "LD DE,(nn)", 20));
            edPrefixed.Add(new Instruction(0x5c, "NEG", 8));
            edPrefixed.Add(new Instruction(0x5d, "RETI", 14));
            edPrefixed.Add(new Instruction(0x5e, "IM 2", 8));
            edPrefixed.Add(new Instruction(0x5f, "LD A,R", 9));

            edPrefixed.Add(new Instruction(0x60, "IN H,(C)", 12));
            edPrefixed.Add(new Instruction(0x61, "OUT (C),H", 12));
            edPrefixed.Add(new Instruction(0x62, "SBC HL,HL", 15));
            edPrefixed.Add(new Instruction(0x63, "LD (nn),HL", 20));
            edPrefixed.Add(new Instruction(0x64, "NEG", 8));
            edPrefixed.Add(new Instruction(0x65, "RETN", 14));
            edPrefixed.Add(new Instruction(0x66, "IM 0", 8));
            edPrefixed.Add(new Instruction(0x67, "RRD", 18));
            edPrefixed.Add(new Instruction(0x68, "IN L,(C)", 12));
            edPrefixed.Add(new Instruction(0x69, "OUT (C),L", 12));
            edPrefixed.Add(new Instruction(0x6a, "ADC HL,HL", 15));
            edPrefixed.Add(new Instruction(0x6b, "LD HL,(nn)", 20));
            edPrefixed.Add(new Instruction(0x6c, "NEG", 8));
            edPrefixed.Add(new Instruction(0x6d, "RETI", 14));
            edPrefixed.Add(new Instruction(0x6e, "IM 0", 8));
            edPrefixed.Add(new Instruction(0x6f, "RLD", 18));

            edPrefixed.Add(new Instruction(0x70, "IN (C)", 12));
            edPrefixed.Add(new Instruction(0x71, "OUT (C),0", 12));
            edPrefixed.Add(new Instruction(0x72, "SBC HL,SP", 15));
            edPrefixed.Add(new Instruction(0x73, "LD (nn),SP", 20));
            edPrefixed.Add(new Instruction(0x74, "NEG", 8));
            edPrefixed.Add(new Instruction(0x75, "RETN", 14));
            edPrefixed.Add(new Instruction(0x76, "IM 1", 8));
            edPrefixed.Add(new Instruction(0x77, "NOP", 4));
            edPrefixed.Add(new Instruction(0x78, "IN A,(C)", 12));
            edPrefixed.Add(new Instruction(0x79, "OUT (C),A", 12));
            edPrefixed.Add(new Instruction(0x7a, "ADC HL,SP", 15));
            edPrefixed.Add(new Instruction(0x7b, "LD SP,(nn)", 20));
            edPrefixed.Add(new Instruction(0x7c, "NEG", 8));
            edPrefixed.Add(new Instruction(0x7d, "RETI", 14));
            edPrefixed.Add(new Instruction(0x7e, "IM 2", 8));
            edPrefixed.Add(Instruction.Nop(0x7f));

            edPrefixed.Add(new Instruction(0xa0, "LDI", 16));
            edPrefixed.Add(new Instruction(0xa1, "CPI", 16));
            edPrefixed.Add(new Instruction(0xa2, "INI", 16));
            edPrefixed.Add(new Instruction(0xa3, "OUTI", 16));
            edPrefixed.Add(new Instruction(0xa8, "LDD", 16));
            edPrefixed.Add(new Instruction(0xa9, "CPD", 16));
            edPrefixed.Add(new Instruction(0xaa, "IND", 16));
            edPrefixed.Add(new Instruction(0xab, "OUTD", 16));

            edPrefixed.Add(new Instruction(0xb0, "LDIR", 21));
            edPrefixed.Add(new Instruction(0xb1, "CPIR", 21));
            edPrefixed.Add(new Instruction(0xb2, "INIR", 21));
            edPrefixed.Add(new Instruction(0xb3, "OUTIR", 21));
            edPrefixed.Add(new Instruction(0xb8, "LDDR", 21));
            edPrefixed.Add(new Instruction(0xb9, "CPDR", 21));
            edPrefixed.Add(new Instruction(0xba, "INDR", 21));
            edPrefixed.Add(new Instruction(0xbb, "OTDR", 21));

            return edPrefixed;
        }
        private IEnumerable<Instruction> CreateDDPrefixedInstructions()
        {

            var ddPrefixed = new List<Instruction>();

            ddPrefixed.Add(new Instruction(0x09, "ADD IX,BC", 15));

            ddPrefixed.Add(new Instruction(0x19, "ADD IX,DE", 15));

            ddPrefixed.Add(new Instruction(0x21, "LD IX,nn", 14));
            ddPrefixed.Add(new Instruction(0x22, "LD (nn),IX", 20));
            ddPrefixed.Add(new Instruction(0x23, "INC IX", 10));
            ddPrefixed.Add(new Instruction(0x24, "INC IXH", 8));
            ddPrefixed.Add(new Instruction(0x25, "DEC IXH", 8));
            ddPrefixed.Add(new Instruction(0x26, "LD IXH,n", 11));
            ddPrefixed.Add(new Instruction(0x29, "ADD IX,IX", 15));
            ddPrefixed.Add(new Instruction(0x2a, "LD IX,(nn)", 20));
            ddPrefixed.Add(new Instruction(0x2b, "DEC IX", 10));
            ddPrefixed.Add(new Instruction(0x2c, "INC IXL", 8));
            ddPrefixed.Add(new Instruction(0x2d, "DEC IXL", 8));
            ddPrefixed.Add(new Instruction(0x2e, "LD IXL,n", 11));

            ddPrefixed.Add(new Instruction(0x34, "INC (IX+d)", 23));
            ddPrefixed.Add(new Instruction(0x35, "DEC (IX+d)", 23));
            ddPrefixed.Add(new Instruction(0x36, "LD (IX+d)", 19));
            ddPrefixed.Add(new Instruction(0x39, "ADD IX,SP", 15));

            ddPrefixed.Add(new Instruction(0x44, "LD B,IXH", 8));
            ddPrefixed.Add(new Instruction(0x45, "LD B,IXL", 8));
            ddPrefixed.Add(new Instruction(0x46, "LD B,(IX+d)", 19));
            ddPrefixed.Add(new Instruction(0x4c, "LD C,IXH", 8));
            ddPrefixed.Add(new Instruction(0x4d, "LD C,IXL", 8));
            ddPrefixed.Add(new Instruction(0x4e, "LD C,(IX+d)", 19));

            ddPrefixed.Add(new Instruction(0x54, "LD D,IXH", 8));
            ddPrefixed.Add(new Instruction(0x55, "LD D,IXL", 8));
            ddPrefixed.Add(new Instruction(0x56, "LD D,(IX+d)", 19));
            ddPrefixed.Add(new Instruction(0x5c, "LD E,IXH", 8));
            ddPrefixed.Add(new Instruction(0x5d, "LD E,IXL", 8));
            ddPrefixed.Add(new Instruction(0x5e, "LD E,(IX+d)", 19));

            ddPrefixed.Add(new Instruction(0x60, "LD IXH,B", 8));
            ddPrefixed.Add(new Instruction(0x61, "LD IXH,C", 8));
            ddPrefixed.Add(new Instruction(0x62, "LD IXH,D", 8));
            ddPrefixed.Add(new Instruction(0x63, "LD IXH,E", 8));
            ddPrefixed.Add(new Instruction(0x64, "LD IXH,IXH", 8));
            ddPrefixed.Add(new Instruction(0x65, "LD IXH,IXL", 8));
            ddPrefixed.Add(new Instruction(0x66, "LD H,(IX+d)", 19));
            ddPrefixed.Add(new Instruction(0x67, "LD IXH,A", 8));
            ddPrefixed.Add(new Instruction(0x68, "LD IXL,B", 8));
            ddPrefixed.Add(new Instruction(0x69, "LD IXL,C", 8));
            ddPrefixed.Add(new Instruction(0x6a, "LD IXL,D", 8));
            ddPrefixed.Add(new Instruction(0x6b, "LD IXL,E", 8));
            ddPrefixed.Add(new Instruction(0x6c, "LD IXL,IXH", 8));
            ddPrefixed.Add(new Instruction(0x6d, "LD IXL,IXL", 8));
            ddPrefixed.Add(new Instruction(0x6e, "LD L,(IX+d)", 19));
            ddPrefixed.Add(new Instruction(0x6f, "LD IXL,A", 8));

            ddPrefixed.Add(new Instruction(0x70, "LD (IX+d),B", 19));
            ddPrefixed.Add(new Instruction(0x71, "LD (IX+d),C", 19));
            ddPrefixed.Add(new Instruction(0x72, "LD (IX+d),D", 19));
            ddPrefixed.Add(new Instruction(0x73, "LD (IX+d),E", 19));
            ddPrefixed.Add(new Instruction(0x74, "LD (IX+d),H", 19));
            ddPrefixed.Add(new Instruction(0x75, "LD (IX+d),L", 19));
            ddPrefixed.Add(new Instruction(0x77, "LD (IX+d),A", 19));
            ddPrefixed.Add(new Instruction(0x7c, "LD A,IXH", 8));
            ddPrefixed.Add(new Instruction(0x7d, "LD A,IXL", 8));
            ddPrefixed.Add(new Instruction(0x7e, "LD A,(IX+d)", 19));

            ddPrefixed.Add(new Instruction(0x84, "ADD A,IXH", 8));
            ddPrefixed.Add(new Instruction(0x85, "ADD A,IXL", 8));
            ddPrefixed.Add(new Instruction(0x86, "ADD A,(IX+d)", 19));
            ddPrefixed.Add(new Instruction(0x8c, "ADC A,IXH", 8));
            ddPrefixed.Add(new Instruction(0x8d, "ADC A,IXL", 8));
            ddPrefixed.Add(new Instruction(0x8e, "ADC A,(IX+d)", 19));

            ddPrefixed.Add(new Instruction(0x94, "SUB IXH", 8));
            ddPrefixed.Add(new Instruction(0x95, "SUB IXL", 8));
            ddPrefixed.Add(new Instruction(0x96, "SUB (IX+d)", 19));
            ddPrefixed.Add(new Instruction(0x9c, "SBC A,IXH", 8));
            ddPrefixed.Add(new Instruction(0x9d, "SBC A,IXL", 8));
            ddPrefixed.Add(new Instruction(0x9e, "SBC A,(IX+d)", 19));

            ddPrefixed.Add(new Instruction(0xa4, "AND IXH", 8, Logical.AND_IXH));
            ddPrefixed.Add(new Instruction(0xa5, "AND IXL", 8, Logical.AND_IXL));
            ddPrefixed.Add(new Instruction(0xa6, "AND (IX+d)", 19));
            ddPrefixed.Add(new Instruction(0xac, "XOR IXH", 8));
            ddPrefixed.Add(new Instruction(0xad, "XOR IXL", 8));
            ddPrefixed.Add(new Instruction(0xae, "XOR (IX+d)", 19));

            ddPrefixed.Add(new Instruction(0xb4, "OR IXH", 8));
            ddPrefixed.Add(new Instruction(0xb5, "OR IXL", 8));
            ddPrefixed.Add(new Instruction(0xb6, "OR (IX+d)", 19));
            ddPrefixed.Add(new Instruction(0xbc, "CP IXH", 8));
            ddPrefixed.Add(new Instruction(0xbd, "CP IXL", 8));
            ddPrefixed.Add(new Instruction(0xbe, "CP (IX+d)", 19));

            ddPrefixed.Add(new Instruction(0xcb, "-- CB --", 0));           // CB Prefix

            ddPrefixed.Add(new Instruction(0xe1, "POP IX", 14));
            ddPrefixed.Add(new Instruction(0xe3, "EX (SP),IX", 23));
            ddPrefixed.Add(new Instruction(0xe5, "PUSH IX", 15));
            ddPrefixed.Add(new Instruction(0xe9, "JP (IX)", 8, Jump.JP_IX));
            ddPrefixed.Add(new Instruction(0xed, "-- ED --", 0));
            ddPrefixed.Add(new Instruction(0xfd, "-- FD --", 0));

            ddPrefixed.Add(new Instruction(0xf9, "LD SP,IX", 10));
            return ddPrefixed;
        }
        private IEnumerable<Instruction> CreateFDPrefixedInstructions()
        {
            var fdPrefixed = new List<Instruction>();

            fdPrefixed.Add(new Instruction(0x09, "ADD IY,BC", 15));

            fdPrefixed.Add(new Instruction(0x19, "ADD IY,DE", 15));

            fdPrefixed.Add(new Instruction(0x21, "LD IY,nn", 14));
            fdPrefixed.Add(new Instruction(0x22, "LD (nn),IY", 20));
            fdPrefixed.Add(new Instruction(0x23, "INC IY", 10));
            fdPrefixed.Add(new Instruction(0x24, "INC IYH", 8));
            fdPrefixed.Add(new Instruction(0x25, "DEC IYH", 8));
            fdPrefixed.Add(new Instruction(0x26, "LD IYH,n", 11));
            fdPrefixed.Add(new Instruction(0x29, "ADD IY,IY", 15));
            fdPrefixed.Add(new Instruction(0x2a, "LD IY,(nn)", 20));
            fdPrefixed.Add(new Instruction(0x2b, "DEC IY", 10));
            fdPrefixed.Add(new Instruction(0x2c, "INC IYL", 8));
            fdPrefixed.Add(new Instruction(0x2d, "DEC IYL", 8));
            fdPrefixed.Add(new Instruction(0x2e, "LD IYL,n", 11));

            fdPrefixed.Add(new Instruction(0x34, "INC (IY+d)", 23));
            fdPrefixed.Add(new Instruction(0x35, "DEC (IY+d)", 23));
            fdPrefixed.Add(new Instruction(0x36, "LD (IY+d)", 19));
            fdPrefixed.Add(new Instruction(0x39, "ADD IY,SP", 15));

            fdPrefixed.Add(new Instruction(0x44, "LD B,IYH", 8));
            fdPrefixed.Add(new Instruction(0x45, "LD B,IYL", 8));
            fdPrefixed.Add(new Instruction(0x46, "LD B,(IY+d)", 19));
            fdPrefixed.Add(new Instruction(0x4c, "LD C,IYH", 8));
            fdPrefixed.Add(new Instruction(0x4d, "LD C,IYL", 8));
            fdPrefixed.Add(new Instruction(0x4e, "LD C,(IY+d)", 19));

            fdPrefixed.Add(new Instruction(0x54, "LD D,IYH", 8));
            fdPrefixed.Add(new Instruction(0x55, "LD D,IYL", 8));
            fdPrefixed.Add(new Instruction(0x56, "LD D,(IY+d)", 19));
            fdPrefixed.Add(new Instruction(0x5c, "LD E,IYH", 8));
            fdPrefixed.Add(new Instruction(0x5d, "LD E,IYL", 8));
            fdPrefixed.Add(new Instruction(0x5e, "LD E,(IY+d)", 19));

            fdPrefixed.Add(new Instruction(0x60, "LD IYH,B", 8));
            fdPrefixed.Add(new Instruction(0x61, "LD IYH,C", 8));
            fdPrefixed.Add(new Instruction(0x62, "LD IYH,D", 8));
            fdPrefixed.Add(new Instruction(0x63, "LD IYH,E", 8));
            fdPrefixed.Add(new Instruction(0x64, "LD IYH,IYH", 8));
            fdPrefixed.Add(new Instruction(0x65, "LD IYH,IYL", 8));
            fdPrefixed.Add(new Instruction(0x66, "LD H,(IY+d)", 19));
            fdPrefixed.Add(new Instruction(0x67, "LD IYH,A", 8));
            fdPrefixed.Add(new Instruction(0x68, "LD IYL,B", 8));
            fdPrefixed.Add(new Instruction(0x69, "LD IYL,C", 8));
            fdPrefixed.Add(new Instruction(0x6a, "LD IYL,D", 8));
            fdPrefixed.Add(new Instruction(0x6b, "LD IYL,E", 8));
            fdPrefixed.Add(new Instruction(0x6c, "LD IYL,IYH", 8));
            fdPrefixed.Add(new Instruction(0x6d, "LD IYL,IYL", 8));
            fdPrefixed.Add(new Instruction(0x6e, "LD L,(IY+d)", 19));
            fdPrefixed.Add(new Instruction(0x6f, "LD IYL,A", 8));

            fdPrefixed.Add(new Instruction(0x70, "LD (IY+d),B", 19));
            fdPrefixed.Add(new Instruction(0x71, "LD (IY+d),C", 19));
            fdPrefixed.Add(new Instruction(0x72, "LD (IY+d),D", 19));
            fdPrefixed.Add(new Instruction(0x73, "LD (IY+d),E", 19));
            fdPrefixed.Add(new Instruction(0x74, "LD (IY+d),H", 19));
            fdPrefixed.Add(new Instruction(0x75, "LD (IY+d),L", 19));
            fdPrefixed.Add(new Instruction(0x77, "LD (IY+d),A", 19));
            fdPrefixed.Add(new Instruction(0x7c, "LD A,IYH", 8));
            fdPrefixed.Add(new Instruction(0x7d, "LD A,IYL", 8));
            fdPrefixed.Add(new Instruction(0x7e, "LD A,(IY+d)", 19));

            fdPrefixed.Add(new Instruction(0x84, "ADD A,IYH", 8));
            fdPrefixed.Add(new Instruction(0x85, "ADD A,IYL", 8));
            fdPrefixed.Add(new Instruction(0x86, "ADD A,(IY+d)", 19));
            fdPrefixed.Add(new Instruction(0x8c, "ADC A,IYH", 8));
            fdPrefixed.Add(new Instruction(0x8d, "ADC A,IYL", 8));
            fdPrefixed.Add(new Instruction(0x8e, "ADC A,(IY+d)", 19));

            fdPrefixed.Add(new Instruction(0x94, "SUB IYH", 8));
            fdPrefixed.Add(new Instruction(0x95, "SUB IYL", 8));
            fdPrefixed.Add(new Instruction(0x96, "SUB (IY+d)", 19));
            fdPrefixed.Add(new Instruction(0x9c, "SBC A,IYH", 8));
            fdPrefixed.Add(new Instruction(0x9d, "SBC A,IYL", 8));
            fdPrefixed.Add(new Instruction(0x9e, "SBC A,(IY+d)", 19));

            fdPrefixed.Add(new Instruction(0xa4, "AND IYH", 8, Logical.AND_IYH));
            fdPrefixed.Add(new Instruction(0xa5, "AND IYL", 8, Logical.AND_IYL));
            fdPrefixed.Add(new Instruction(0xa6, "AND (IY+d)", 19));
            fdPrefixed.Add(new Instruction(0xac, "XOR IYH", 8));
            fdPrefixed.Add(new Instruction(0xad, "XOR IYL", 8));
            fdPrefixed.Add(new Instruction(0xae, "XOR (IY+d)", 19));

            fdPrefixed.Add(new Instruction(0xb4, "OR IYH", 8));
            fdPrefixed.Add(new Instruction(0xb5, "OR IYL", 8));
            fdPrefixed.Add(new Instruction(0xb6, "OR (IY+d)", 19));
            fdPrefixed.Add(new Instruction(0xbc, "CP IYH", 8));
            fdPrefixed.Add(new Instruction(0xbd, "CP IYL", 8));
            fdPrefixed.Add(new Instruction(0xbe, "CP (IY+d)", 19));

            fdPrefixed.Add(new Instruction(0xcb, "-- CB --", 0));           // CB Prefix
            fdPrefixed.Add(new Instruction(0xdd, "-- DD --", 0));           // DD Prefix

            fdPrefixed.Add(new Instruction(0xe1, "POP IY", 14));
            fdPrefixed.Add(new Instruction(0xe3, "EX (SP),IY", 23));
            fdPrefixed.Add(new Instruction(0xe5, "PUSH IY", 15));
            fdPrefixed.Add(new Instruction(0xe9, "JP (IY)", 8, Jump.JP_IY));
            fdPrefixed.Add(new Instruction(0xed, "-- ED --", 0));

            fdPrefixed.Add(new Instruction(0xf9, "LD SP,IY", 10));
            return fdPrefixed;
        }
        private IEnumerable<Instruction> CreateDDCBPrefixedInstructions()
        {
            var ddcbPrefixed = new List<Instruction>();

            ddcbPrefixed.Add(new Instruction(0x0, "RLC (IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0x1, "RLC (IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0x2, "RLC (IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0x3, "RLC (IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0x4, "RLC (IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0x5, "RLC (IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0x6, "RLC (IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0x7, "RLC (IX+d)->A", 23));
            ddcbPrefixed.Add(new Instruction(0x8, "RRC (IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0x9, "RRC (IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0xa, "RRC (IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0xb, "RRC (IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0xc, "RRC (IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0xd, "RRC (IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0xe, "RRC (IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0xf, "RRC (IX+d)->A", 23));

            ddcbPrefixed.Add(new Instruction(0x10, "RL (IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0x11, "RL (IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0x12, "RL (IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0x13, "RL (IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0x14, "RL (IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0x15, "RL (IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0x16, "RL (IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0x17, "RL (IX+d)->A", 23));
            ddcbPrefixed.Add(new Instruction(0x18, "RR (IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0x19, "RR (IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0x1a, "RR (IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0x1b, "RR (IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0x1c, "RR (IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0x1d, "RR (IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0x1e, "RR (IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0x1f, "RR (IX+d)->A", 23));

            ddcbPrefixed.Add(new Instruction(0x20, "SLA (IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0x21, "SLA (IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0x22, "SLA (IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0x23, "SLA (IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0x24, "SLA (IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0x25, "SLA (IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0x26, "SLA (IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0x27, "SLA (IX+d)->A", 23));
            ddcbPrefixed.Add(new Instruction(0x28, "SRA (IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0x29, "SRA (IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0x2a, "SRA (IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0x2b, "SRA (IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0x2c, "SRA (IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0x2d, "SRA (IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0x2e, "SRA (IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0x2f, "SRA (IX+d)->A", 23));

            ddcbPrefixed.Add(new Instruction(0x30, "SLS (IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0x31, "SLS (IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0x32, "SLS (IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0x33, "SLS (IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0x34, "SLS (IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0x35, "SLS (IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0x36, "SLS (IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0x37, "SLS (IX+d)->A", 23));
            ddcbPrefixed.Add(new Instruction(0x38, "SRL (IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0x39, "SRL (IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0x3a, "SRL (IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0x3b, "SRL (IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0x3c, "SRL (IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0x3d, "SRL (IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0x3e, "SRL (IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0x3f, "SRL (IX+d)->A", 23));


            ddcbPrefixed.Add(new Instruction(0x40, "BIT 0,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x41, "BIT 0,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x42, "BIT 0,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x43, "BIT 0,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x44, "BIT 0,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x45, "BIT 0,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x46, "BIT 0,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x47, "BIT 0,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x49, "BIT 1,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x4a, "BIT 1,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x4b, "BIT 1,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x4c, "BIT 1,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x4d, "BIT 1,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x4e, "BIT 1,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x48, "BIT 1,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x4f, "BIT 1,(IX+d)", 20));

            ddcbPrefixed.Add(new Instruction(0x50, "BIT 2,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x51, "BIT 2,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x52, "BIT 2,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x53, "BIT 2,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x54, "BIT 2,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x55, "BIT 2,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x56, "BIT 2,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x57, "BIT 2,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x59, "BIT 3,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x5a, "BIT 3,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x5b, "BIT 3,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x5c, "BIT 3,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x5d, "BIT 3,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x5e, "BIT 3,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x58, "BIT 3,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x5f, "BIT 3,(IX+d)", 20));

            ddcbPrefixed.Add(new Instruction(0x60, "BIT 4,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x61, "BIT 4,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x62, "BIT 4,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x63, "BIT 4,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x64, "BIT 4,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x65, "BIT 4,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x66, "BIT 4,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x67, "BIT 4,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x69, "BIT 5,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x6a, "BIT 5,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x6b, "BIT 5,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x6c, "BIT 5,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x6d, "BIT 5,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x6e, "BIT 5,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x68, "BIT 5,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x6f, "BIT 5,(IX+d)", 20));

            ddcbPrefixed.Add(new Instruction(0x70, "BIT 6,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x71, "BIT 6,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x72, "BIT 6,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x73, "BIT 6,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x74, "BIT 6,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x75, "BIT 6,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x76, "BIT 6,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x77, "BIT 6,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x79, "BIT 7,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x7a, "BIT 7,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x7b, "BIT 7,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x7c, "BIT 7,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x7d, "BIT 7,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x7e, "BIT 7,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x78, "BIT 7,(IX+d)", 20));
            ddcbPrefixed.Add(new Instruction(0x7f, "BIT 7,(IX+d)", 20));

            ddcbPrefixed.Add(new Instruction(0x80, "RES 0,(IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0x81, "RES 0,(IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0x82, "RES 0,(IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0x83, "RES 0,(IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0x84, "RES 0,(IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0x85, "RES 0,(IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0x86, "RES 0,(IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0x87, "RES 0,(IX+d)->A", 23));
            ddcbPrefixed.Add(new Instruction(0x88, "RES 1,(IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0x89, "RES 1,(IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0x8a, "RES 1,(IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0x8b, "RES 1,(IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0x8c, "RES 1,(IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0x8d, "RES 1,(IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0x8e, "RES 1,(IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0x8f, "RES 1,(IX+d)->A", 23));

            ddcbPrefixed.Add(new Instruction(0x90, "RES 2,(IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0x91, "RES 2,(IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0x92, "RES 2,(IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0x93, "RES 2,(IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0x94, "RES 2,(IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0x95, "RES 2,(IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0x96, "RES 2,(IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0x97, "RES 2,(IX+d)->A", 23));
            ddcbPrefixed.Add(new Instruction(0x98, "RES 3,(IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0x99, "RES 3,(IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0x9a, "RES 3,(IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0x9b, "RES 3,(IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0x9c, "RES 3,(IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0x9d, "RES 3,(IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0x9e, "RES 3,(IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0x9f, "RES 3,(IX+d)->A", 23));

            ddcbPrefixed.Add(new Instruction(0xa0, "RES 4,(IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0xa1, "RES 4,(IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0xa2, "RES 4,(IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0xa3, "RES 4,(IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0xa4, "RES 4,(IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0xa5, "RES 4,(IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0xa6, "RES 4,(IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0xa7, "RES 4,(IX+d)->A", 23));
            ddcbPrefixed.Add(new Instruction(0xa8, "RES 5,(IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0xa9, "RES 5,(IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0xaa, "RES 5,(IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0xab, "RES 5,(IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0xac, "RES 5,(IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0xad, "RES 5,(IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0xae, "RES 5,(IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0xaf, "RES 5,(IX+d)->A", 23));

            ddcbPrefixed.Add(new Instruction(0xb0, "RES 6,(IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0xb1, "RES 6,(IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0xb2, "RES 6,(IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0xb3, "RES 6,(IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0xb4, "RES 6,(IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0xb5, "RES 6,(IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0xb6, "RES 6,(IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0xb7, "RES 6,(IX+d)->A", 23));
            ddcbPrefixed.Add(new Instruction(0xb8, "RES 7,(IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0xb9, "RES 7,(IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0xba, "RES 7,(IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0xbb, "RES 7,(IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0xbc, "RES 7,(IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0xbd, "RES 7,(IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0xbe, "RES 7,(IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0xbf, "RES 7,(IX+d)->A", 23));

            ddcbPrefixed.Add(new Instruction(0xc0, "SET 0,(IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0xc1, "SET 0,(IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0xc2, "SET 0,(IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0xc3, "SET 0,(IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0xc4, "SET 0,(IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0xc5, "SET 0,(IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0xc6, "SET 0,(IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0xc7, "SET 0,(IX+d)->A", 23));
            ddcbPrefixed.Add(new Instruction(0xc8, "SET 1,(IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0xc9, "SET 1,(IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0xca, "SET 1,(IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0xcb, "SET 1,(IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0xcc, "SET 1,(IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0xcd, "SET 1,(IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0xce, "SET 1,(IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0xcf, "SET 1,(IX+d)->A", 23));

            ddcbPrefixed.Add(new Instruction(0xd0, "SET 2,(IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0xd1, "SET 2,(IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0xd2, "SET 2,(IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0xd3, "SET 2,(IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0xd4, "SET 2,(IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0xd5, "SET 2,(IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0xd6, "SET 2,(IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0xd7, "SET 2,(IX+d)->A", 23));
            ddcbPrefixed.Add(new Instruction(0xd8, "SET 3,(IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0xd9, "SET 3,(IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0xda, "SET 3,(IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0xdb, "SET 3,(IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0xdc, "SET 3,(IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0xdd, "SET 3,(IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0xde, "SET 3,(IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0xdf, "SET 3,(IX+d)->A", 23));

            ddcbPrefixed.Add(new Instruction(0xe0, "SET 4,(IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0xe1, "SET 4,(IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0xe2, "SET 4,(IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0xe3, "SET 4,(IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0xe4, "SET 4,(IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0xe5, "SET 4,(IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0xe6, "SET 4,(IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0xe7, "SET 4,(IX+d)->A", 23));
            ddcbPrefixed.Add(new Instruction(0xe8, "SET 5,(IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0xe9, "SET 5,(IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0xea, "SET 5,(IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0xeb, "SET 5,(IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0xec, "SET 5,(IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0xed, "SET 5,(IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0xee, "SET 5,(IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0xef, "SET 5,(IX+d)->A", 23));

            ddcbPrefixed.Add(new Instruction(0xf0, "SET 6,(IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0xf1, "SET 6,(IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0xf2, "SET 6,(IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0xf3, "SET 6,(IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0xf4, "SET 6,(IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0xf5, "SET 6,(IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0xf6, "SET 6,(IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0xf7, "SET 6,(IX+d)->A", 23));
            ddcbPrefixed.Add(new Instruction(0xf8, "SET 7,(IX+d)->B", 23));
            ddcbPrefixed.Add(new Instruction(0xf9, "SET 7,(IX+d)->C", 23));
            ddcbPrefixed.Add(new Instruction(0xfa, "SET 7,(IX+d)->D", 23));
            ddcbPrefixed.Add(new Instruction(0xfb, "SET 7,(IX+d)->E", 23));
            ddcbPrefixed.Add(new Instruction(0xfc, "SET 7,(IX+d)->H", 23));
            ddcbPrefixed.Add(new Instruction(0xfd, "SET 7,(IX+d)->L", 23));
            ddcbPrefixed.Add(new Instruction(0xfe, "SET 7,(IX+d)", 23));
            ddcbPrefixed.Add(new Instruction(0xff, "SET 7,(IX+d)->A", 23));










            return ddcbPrefixed;
        }
        private IEnumerable<Instruction> CreateFDCBPrefixedInstructions()
        {
            var fdcbPrefixed = new List<Instruction>();

            fdcbPrefixed.Add(new Instruction(0x0, "RLC (IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0x1, "RLC (IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0x2, "RLC (IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0x3, "RLC (IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0x4, "RLC (IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0x5, "RLC (IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0x6, "RLC (IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0x7, "RLC (IY+d)->A", 23));
            fdcbPrefixed.Add(new Instruction(0x8, "RRC (IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0x9, "RRC (IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0xa, "RRC (IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0xb, "RRC (IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0xc, "RRC (IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0xd, "RRC (IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0xe, "RRC (IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0xf, "RRC (IY+d)->A", 23));

            fdcbPrefixed.Add(new Instruction(0x10, "RL (IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0x11, "RL (IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0x12, "RL (IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0x13, "RL (IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0x14, "RL (IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0x15, "RL (IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0x16, "RL (IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0x17, "RL (IY+d)->A", 23));
            fdcbPrefixed.Add(new Instruction(0x18, "RR (IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0x19, "RR (IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0x1a, "RR (IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0x1b, "RR (IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0x1c, "RR (IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0x1d, "RR (IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0x1e, "RR (IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0x1f, "RR (IY+d)->A", 23));

            fdcbPrefixed.Add(new Instruction(0x20, "SLA (IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0x21, "SLA (IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0x22, "SLA (IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0x23, "SLA (IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0x24, "SLA (IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0x25, "SLA (IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0x26, "SLA (IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0x27, "SLA (IY+d)->A", 23));
            fdcbPrefixed.Add(new Instruction(0x28, "SRA (IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0x29, "SRA (IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0x2a, "SRA (IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0x2b, "SRA (IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0x2c, "SRA (IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0x2d, "SRA (IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0x2e, "SRA (IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0x2f, "SRA (IY+d)->A", 23));

            fdcbPrefixed.Add(new Instruction(0x30, "SLS (IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0x31, "SLS (IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0x32, "SLS (IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0x33, "SLS (IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0x34, "SLS (IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0x35, "SLS (IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0x36, "SLS (IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0x37, "SLS (IY+d)->A", 23));
            fdcbPrefixed.Add(new Instruction(0x38, "SRL (IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0x39, "SRL (IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0x3a, "SRL (IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0x3b, "SRL (IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0x3c, "SRL (IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0x3d, "SRL (IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0x3e, "SRL (IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0x3f, "SRL (IY+d)->A", 23));


            fdcbPrefixed.Add(new Instruction(0x40, "BIT 0,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x41, "BIT 0,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x42, "BIT 0,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x43, "BIT 0,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x44, "BIT 0,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x45, "BIT 0,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x46, "BIT 0,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x47, "BIT 0,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x49, "BIT 1,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x4a, "BIT 1,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x4b, "BIT 1,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x4c, "BIT 1,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x4d, "BIT 1,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x4e, "BIT 1,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x48, "BIT 1,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x4f, "BIT 1,(IY+d)", 20));

            fdcbPrefixed.Add(new Instruction(0x50, "BIT 2,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x51, "BIT 2,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x52, "BIT 2,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x53, "BIT 2,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x54, "BIT 2,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x55, "BIT 2,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x56, "BIT 2,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x57, "BIT 2,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x59, "BIT 3,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x5a, "BIT 3,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x5b, "BIT 3,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x5c, "BIT 3,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x5d, "BIT 3,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x5e, "BIT 3,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x58, "BIT 3,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x5f, "BIT 3,(IY+d)", 20));

            fdcbPrefixed.Add(new Instruction(0x60, "BIT 4,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x61, "BIT 4,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x62, "BIT 4,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x63, "BIT 4,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x64, "BIT 4,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x65, "BIT 4,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x66, "BIT 4,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x67, "BIT 4,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x69, "BIT 5,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x6a, "BIT 5,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x6b, "BIT 5,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x6c, "BIT 5,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x6d, "BIT 5,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x6e, "BIT 5,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x68, "BIT 5,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x6f, "BIT 5,(IY+d)", 20));

            fdcbPrefixed.Add(new Instruction(0x70, "BIT 6,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x71, "BIT 6,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x72, "BIT 6,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x73, "BIT 6,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x74, "BIT 6,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x75, "BIT 6,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x76, "BIT 6,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x77, "BIT 6,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x79, "BIT 7,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x7a, "BIT 7,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x7b, "BIT 7,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x7c, "BIT 7,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x7d, "BIT 7,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x7e, "BIT 7,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x78, "BIT 7,(IY+d)", 20));
            fdcbPrefixed.Add(new Instruction(0x7f, "BIT 7,(IY+d)", 20));

            fdcbPrefixed.Add(new Instruction(0x80, "RES 0,(IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0x81, "RES 0,(IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0x82, "RES 0,(IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0x83, "RES 0,(IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0x84, "RES 0,(IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0x85, "RES 0,(IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0x86, "RES 0,(IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0x87, "RES 0,(IY+d)->A", 23));
            fdcbPrefixed.Add(new Instruction(0x88, "RES 1,(IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0x89, "RES 1,(IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0x8a, "RES 1,(IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0x8b, "RES 1,(IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0x8c, "RES 1,(IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0x8d, "RES 1,(IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0x8e, "RES 1,(IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0x8f, "RES 1,(IY+d)->A", 23));

            fdcbPrefixed.Add(new Instruction(0x90, "RES 2,(IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0x91, "RES 2,(IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0x92, "RES 2,(IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0x93, "RES 2,(IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0x94, "RES 2,(IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0x95, "RES 2,(IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0x96, "RES 2,(IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0x97, "RES 2,(IY+d)->A", 23));
            fdcbPrefixed.Add(new Instruction(0x98, "RES 3,(IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0x99, "RES 3,(IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0x9a, "RES 3,(IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0x9b, "RES 3,(IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0x9c, "RES 3,(IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0x9d, "RES 3,(IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0x9e, "RES 3,(IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0x9f, "RES 3,(IY+d)->A", 23));

            fdcbPrefixed.Add(new Instruction(0xa0, "RES 4,(IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0xa1, "RES 4,(IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0xa2, "RES 4,(IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0xa3, "RES 4,(IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0xa4, "RES 4,(IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0xa5, "RES 4,(IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0xa6, "RES 4,(IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0xa7, "RES 4,(IY+d)->A", 23));
            fdcbPrefixed.Add(new Instruction(0xa8, "RES 5,(IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0xa9, "RES 5,(IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0xaa, "RES 5,(IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0xab, "RES 5,(IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0xac, "RES 5,(IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0xad, "RES 5,(IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0xae, "RES 5,(IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0xaf, "RES 5,(IY+d)->A", 23));

            fdcbPrefixed.Add(new Instruction(0xb0, "RES 6,(IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0xb1, "RES 6,(IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0xb2, "RES 6,(IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0xb3, "RES 6,(IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0xb4, "RES 6,(IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0xb5, "RES 6,(IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0xb6, "RES 6,(IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0xb7, "RES 6,(IY+d)->A", 23));
            fdcbPrefixed.Add(new Instruction(0xb8, "RES 7,(IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0xb9, "RES 7,(IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0xba, "RES 7,(IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0xbb, "RES 7,(IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0xbc, "RES 7,(IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0xbd, "RES 7,(IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0xbe, "RES 7,(IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0xbf, "RES 7,(IY+d)->A", 23));

            fdcbPrefixed.Add(new Instruction(0xc0, "SET 0,(IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0xc1, "SET 0,(IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0xc2, "SET 0,(IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0xc3, "SET 0,(IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0xc4, "SET 0,(IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0xc5, "SET 0,(IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0xc6, "SET 0,(IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0xc7, "SET 0,(IY+d)->A", 23));
            fdcbPrefixed.Add(new Instruction(0xc8, "SET 1,(IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0xc9, "SET 1,(IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0xca, "SET 1,(IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0xcb, "SET 1,(IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0xcc, "SET 1,(IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0xcd, "SET 1,(IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0xce, "SET 1,(IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0xcf, "SET 1,(IY+d)->A", 23));

            fdcbPrefixed.Add(new Instruction(0xd0, "SET 2,(IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0xd1, "SET 2,(IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0xd2, "SET 2,(IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0xd3, "SET 2,(IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0xd4, "SET 2,(IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0xd5, "SET 2,(IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0xd6, "SET 2,(IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0xd7, "SET 2,(IY+d)->A", 23));
            fdcbPrefixed.Add(new Instruction(0xd8, "SET 3,(IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0xd9, "SET 3,(IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0xda, "SET 3,(IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0xdb, "SET 3,(IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0xdc, "SET 3,(IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0xdd, "SET 3,(IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0xde, "SET 3,(IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0xdf, "SET 3,(IY+d)->A", 23));

            fdcbPrefixed.Add(new Instruction(0xe0, "SET 4,(IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0xe1, "SET 4,(IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0xe2, "SET 4,(IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0xe3, "SET 4,(IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0xe4, "SET 4,(IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0xe5, "SET 4,(IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0xe6, "SET 4,(IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0xe7, "SET 4,(IY+d)->A", 23));
            fdcbPrefixed.Add(new Instruction(0xe8, "SET 5,(IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0xe9, "SET 5,(IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0xea, "SET 5,(IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0xeb, "SET 5,(IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0xec, "SET 5,(IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0xed, "SET 5,(IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0xee, "SET 5,(IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0xef, "SET 5,(IY+d)->A", 23));

            fdcbPrefixed.Add(new Instruction(0xf0, "SET 6,(IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0xf1, "SET 6,(IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0xf2, "SET 6,(IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0xf3, "SET 6,(IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0xf4, "SET 6,(IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0xf5, "SET 6,(IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0xf6, "SET 6,(IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0xf7, "SET 6,(IY+d)->A", 23));
            fdcbPrefixed.Add(new Instruction(0xf8, "SET 7,(IY+d)->B", 23));
            fdcbPrefixed.Add(new Instruction(0xf9, "SET 7,(IY+d)->C", 23));
            fdcbPrefixed.Add(new Instruction(0xfa, "SET 7,(IY+d)->D", 23));
            fdcbPrefixed.Add(new Instruction(0xfb, "SET 7,(IY+d)->E", 23));
            fdcbPrefixed.Add(new Instruction(0xfc, "SET 7,(IY+d)->H", 23));
            fdcbPrefixed.Add(new Instruction(0xfd, "SET 7,(IY+d)->L", 23));
            fdcbPrefixed.Add(new Instruction(0xfe, "SET 7,(IY+d)", 23));
            fdcbPrefixed.Add(new Instruction(0xff, "SET 7,(IY+d)->A", 23));
            return fdcbPrefixed;
        }
        private Instruction[] AddMissingNOPs(IEnumerable<Instruction> source)
        {
            var array = new Instruction?[256];
            foreach (var instruction in source)
            {
                array[instruction.Opcode] = instruction;
            }

            for (var i = 0; i < array.Length; i++)
            {
                array[i] ??= Instruction.Nop((byte)i);
            }
            return array!;
        }
        private Instruction[] AddMissingUnprefixed(IEnumerable<Instruction> source, Instruction[] unprefixed)
        {
            var array = new Instruction?[256];
            foreach (var instruction in source)
            {
                array[instruction.Opcode] = instruction;
            }

            for (var i = 0; i < array.Length; i++)
            {
                array[i] ??= unprefixed[i];
            }
            return array!;
        }
    }
}
