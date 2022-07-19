using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler.Encoders
{
    static class RegistersEncoder
    {
        public static byte RegisterNameToBinaryValue(string reg) => reg switch
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
    }

}

