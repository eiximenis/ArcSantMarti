namespace SantMarti.Z80.Assembler.Tokens.Parsers;


[Flags]
public enum ParsersEnabled
{
    None = 0x0,
    Label = 0x1,
    Opcode = 0x2,
    Register = 0x4,
    Number = 0x8,
    Displacement = 0x10,
    MemoryReference = 0x20,
    FlagReference = 0x40,
    // Combinations
    FirstValidToken = Label | Opcode, // Lines can only start with a label, opcode
    OpcodeOrParameters = All & ~Label, // All except Label
    ParametersToken = All & ~Opcode & ~Label, // All except Label, Opcode
    All = 0xFF
}

public class AnyParser
{
    public static BaseToken ParseToken(string token,ParsersEnabled parsersToUse = ParsersEnabled.All)
    {
        if ((parsersToUse & ParsersEnabled.Label) != 0)
        {
            var label = LabelParser.TryGetLabel(token);
            if (label.HasValue) return label.ParsedToken!;
        }
        
        if ((parsersToUse & ParsersEnabled.Opcode) != 0)
        {
            var opcode = OpcodeParser.TryGetLabel(token);
            if (opcode.HasValue) return opcode.ParsedToken!;
        }
        
        if ((parsersToUse & ParsersEnabled.Number) != 0)
        {
            var number = NumericParser.TryGetNumber(token);
            if (number.HasValue) return number.ParsedToken!;
        }

        if ((parsersToUse & ParsersEnabled.Register) != 0)
        {
            var register = RegisterParser.TryGetRegister(token);
            if (register.HasValue) return register.ParsedToken!;
        }

        if ((parsersToUse & ParsersEnabled.MemoryReference) != 0)
        {
            var memref = MemoryReferenceParser.TryGetMemoryReference(token);
            if (memref.HasValue) return memref.ParsedToken!;
        }
        
        if ((parsersToUse & ParsersEnabled.Displacement) != 0)
        {
            var displacement = DisplacementParser.TryGetDisplacement(token);
            if (displacement.HasValue) return displacement.ParsedToken!;
        }

        if ((parsersToUse & ParsersEnabled.FlagReference) != 0)
        {
            var fref = FlagReferenceParser.TryGetFlagReference(token);
            if (fref.HasValue) return fref.ParsedToken!;
        }
        
        return new GenericLabel(token);
    }
}