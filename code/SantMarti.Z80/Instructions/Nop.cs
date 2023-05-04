namespace SantMarti.Z80.Instructions;

public static class Nop
{
    public static void NOP(Instruction instruction, Z80Processor processor)
    {
    }

    public static void HALT(Instruction instruction, Z80Processor processor)
    {
        processor.Pins.ReplaceOtherPinsWith(OtherPins.HALT);
    }
}