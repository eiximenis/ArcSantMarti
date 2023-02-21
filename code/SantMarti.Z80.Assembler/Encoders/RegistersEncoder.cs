using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SantMarti.Z80.Assembler.Tokens;

namespace SantMarti.Z80.Assembler.Encoders
{
    public static class RegistersEncoder
    {
        public static byte ByteRegisterNameToBinaryValue(string reg) => reg switch
        {
            "A" => 0b111,
            "B" => 0b000,
            "C" => 0b001,
            "D" => 0b010,
            "E" => 0b011,
            "H" => 0b100,
            "L" => 0b101,
            _ => 0b110
        };

        public static byte WordRegisterNameToBinaryValue(string reg) => reg switch
        {
            "BC" => 0b00,
            "DE" => 0b01,
            "HL" => 0b10,
            "SP" => 0b11
        };
    }

}

