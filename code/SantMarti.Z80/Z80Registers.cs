using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80
{
    /// <summary>
    /// Flags of the Z80
    /// http://www.z80.info/z80sflag.htm
    /// </summary>
    [Flags]
    public enum Z80Flags : byte
    {
        /// <summary>
        /// Carry: Set if the result did not fit in the register
        /// Also called C
        /// </summary>
        Carry = 1 << 0,
        /// <summary>
        /// Subtract: Set if the last operation was a subtraction
        /// Also called N
        /// </summary>
        Substract = 1 << 1,
        /// <summary>
        /// Parity or Overflow: 
        /// Parity set if even number of bits set 
        /// Overflow set if the 2-complement result does not fit in the register
        /// Also called P/V
        /// </summary>
        ParityOrOverflow = 1 << 2,
        /// <summary>
        /// // Undocumented (Copy of bit 3)
        /// </summary>
        F3 = 1 << 3,
        /// <summary>
        /// Half Carry: Carry from bit 3 to bit 4
        /// Also called H
        /// </summary>
        HalfCarry = 1 << 4,
        /// <summary>
        /// Undocumented (Copy of bit 5)
        /// </summary>
        F5 = 1 << 5,
        /// <summary>
        /// Set if the value is zero
        /// Also called Z
        /// </summary>
        Zero = 1 << 6,
        /// <summary>
        /// Set if the 2-complement value is negative (copy of MSB)
        /// Also called S
        /// </summary>
        Sign = 1 << 7,
        /// <summary>
        /// Both undocumented flags F3 and F5 mask
        /// </summary>
        F3F5 = F3 | F5
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Z80IRRegister
    {
        [FieldOffset(0)] public byte I;
        [FieldOffset(1)] public byte R;
        [FieldOffset(0)] public ushort IR;
    }


    /// <summary>
    /// Implements the Z80 Registers.
    /// References: 
    /// - Z80 Architecture: http://www.z80.info/z80arki.htm
    /// - Z80 Registers: http://z80-heaven.wikidot.com/the-registers-and-memory
    /// </summary>
    /// 
    [StructLayout(LayoutKind.Explicit)]
    public struct Z80GenericRegisters
    {
        // Byte B,C and Word BC
        [FieldOffset(0)]
        public byte C;
        [FieldOffset(1)]
        public byte B;
        [FieldOffset(0)]
        public ushort BC;
        // Byte D,E and Word DE
        [FieldOffset(4)]
        public byte E;
        [FieldOffset(5)]
        public byte D;
        [FieldOffset(4)]
        public ushort DE;
        // Byte H,L and Word HL
        [FieldOffset(8)]
        public byte L;
        [FieldOffset(9)]
        public byte H;
        [FieldOffset(8)]
        public ushort HL;
        // Flags, Accumulator and Word AF
        [FieldOffset(12)]
        public Z80Flags F;
        [FieldOffset(13)]
        public byte A;
        [FieldOffset(12)]
        public ushort AF;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void CopyF3F5FlagsFrom(byte data)
        {
            F |= (Z80Flags)(data & (byte)Z80Flags.F3F5);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasFlag(Z80Flags flag)
        {
            return (F & flag) != 0x0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetFlag(Z80Flags flag)
        {
            F |= flag;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearFlag(Z80Flags flag)
        {
            F &= ~flag;
        }

        /// <summary>
        /// Sets a flag if value is greater than 0. Clears otherwise
        /// <para
        /// <param name="flag">Flag to set/clear</param>
        /// <param name="value">Value used to set the flag (if greater than 0) or clear it (if is 0)</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetFlagIf(Z80Flags flag, int value)
        {
            if (value != 0) { SetFlag(flag); }
            else { ClearFlag(flag); }
        }

        /// <summary>
        /// Sets or clears a flag based on a bool parameter
        /// </summary>
        /// <param name="flag">Flag to set/clear</param>
        /// <param name="set">If true flag is set. Otherwise is cleared</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetFlagIf(Z80Flags flag, bool set)
        {
            if (set) { SetFlag(flag); }
            else { ClearFlag(flag); }
        }

        public void SetParityFor(byte result)
        {            
            var parity = true;
            while (result > 0)
            {
                if ((result & 0x01) == 1)
                {
                    parity = !parity;
                }
                result = (byte)(result >> 1);
            }
            SetFlagIf(Z80Flags.ParityOrOverflow,parity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetSignFor(byte result) => SetFlagIf(Z80Flags.Sign, (result & (byte)0x80) != 0);

        public void SetOverflowForComparison(byte value, byte diff)
        {
            // Overflow in comparison is done when:
            //  A and value are of different sign  
            //  AND
            //  value and (A - value) are of the same sign.
            if ((((A ^ value) & 0x80) != 0)              
                && (((value ^ diff) & 0x80) == 0)) 
            {
                SetFlag(Z80Flags.ParityOrOverflow);
            }
            else
            {
                ClearFlag(Z80Flags.ParityOrOverflow);
            }
        }
    }

    public class Z80Registers
    {

        private Z80GenericRegisters _main;
        private Z80GenericRegisters _alternate;
        

        
        // Hidden WZ register.
        // This register is not available to the developer and is used internally by the CPU
        // Also known as MEMPTR (https://gist.github.com/drhelius/8497817)
        public byte W { get; set; }
        public byte Z { get; set; }
        public ushort WZ
        {
            get => (ushort)((W << 8) | Z);
            set => (W, Z) = ((byte)(value >> 8), (byte)(value & 0xFF));
        }
        
        // Generic registers (B,C,D,E,H,L,A,F)
        public ref Z80GenericRegisters Main { get => ref _main; }
        // Generic alternate registers (B',C',D',E',H',L', A',F')
        public ref Z80GenericRegisters Alternate { get => ref _alternate; }
        
        // 8 bit instruction register (note that this register is never available to developer)
        public byte InstructionRegister { get; set; }

        // 16 bit general purpose registers
        public ushort IX { get; set; }
        public ushort IY { get; set; }

        public byte IXH
        {
            get => (byte)(IX >> 8);
            set => IX = (ushort)((IX & 0x00FF) | (value << 8));
        }

        public byte IXL
        {
            get => (byte)(IX  & 0xFF);
            set => IX = (ushort)((IX & 0xFF00) | value);
        }
        
        public byte IYH
        {
            get => (byte)(IY >> 8);
            set => IY = (ushort)((IX & 0x00FF) | (value << 8));
        }

        public byte IYL
        {
            get => (byte)(IY & 0xFF);
            set => IY = (ushort)((IY & 0xFF00) | value);
        }

        // 16 bits specific registers
        public ushort PC { get; set; }          // Program Counter
        public ushort SP { get; set; }          // Stack Pointer
        
        // IR register
        public Z80IRRegister IR { get; } = new(); // Instruction Register
        
        

    }
}
