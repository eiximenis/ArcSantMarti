﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80
{
    
    public delegate void OnTickResponder (ref Z80Pins pins);
    
    public class Z80Processor
    {
        private readonly Z80Instructions _instructions;
        public Z80Registers Registers { get; }
        private Z80Pins _pins = new();
        private OnTickResponder _onTick = (ref Z80Pins _) => { };

        public ref Z80Pins Pins => ref _pins;

        public Z80Processor()
        {
            Registers = new Z80Registers();
            _instructions = new Z80Instructions();
        }

        internal void OnTick()
        {
            _onTick(ref _pins);
        }

    
        /// <summary>
        /// Fetch an opcode from the memory. This operation
        /// **usually** takes 4 ticks:
        ///  Ticks 1 and 2 are for memory read
        ///  Ticks 3 and 4 are decoding and executing the instruction
        internal void FetchOpcode()
        {   
            // T1: Address bus is filled with PC
            //     Pins M1, MD and MREQ are set
            //     PC is increased
            _pins.Address = Registers.PC;
            _pins.ReplaceOtherPinsWith(OtherPins.M1 | OtherPins.MEMORY_READ);
            Registers.PC = (ushort)(Registers.PC + 1);
            OnTick();
            // T2: At this point Memory read has been performed and the data is available in Address Bus
            //     Instruction register is filled with the data read from memory
            //     TODO: This is the ONLY point where M1 can be stretched using WAIT states 
            Registers.InstructionRegister = _pins.Data;
            OnTick();
            // T3: Memory Refresh (1/2): MREQ and RFSH are set 
            // TODO: Check WAIT states
            _pins.ReplaceOtherPinsWith(OtherPins.MREQ | OtherPins.RFSH);
            OnTick();
            // T4: Memory Refresh (2/2): RFSH is cleared
            _pins.DeactivateOtherPins(OtherPins.MREQ);
            OnTick();
            // M1 (Opcode fetch) with 4 T-Cycles is completed
            _pins.ReplaceOtherPinsWith(OtherPins.NONE);
        }

        /// <summary>
        /// Reads specific position of rhe memory.
        /// This takes three ticks:
        /// Ticks 1 and 2 are for memory read
        /// Tick 3 is for getting the result
        /// </summary>
        /// <param name="address">address to read</param>
        /// <returns></returns>
        public byte MemoryRead(ushort address)
        {
            _pins.Address = address;
            _pins.ActivateOtherPins(OtherPins.MEMORY_READ);
            OnTick();
            OnTick();
            // TODO: Check WAIT states
            _pins.DeactivateOtherPins(OtherPins.MEMORY_READ);
            OnTick();
            return _pins.Data;
        }
        
        /// <summary>
        /// Reads the PC position of the memory and increments the PC
        /// This takes three ticks:
        /// Ticks 1 and 2 are for memory read
        /// Tick 3 is for getting the result
        /// </summary>
        public byte MemoryRead()
        {
            _pins.Address = Registers.PC;
            _pins.ActivateOtherPins(OtherPins.MEMORY_READ);
            OnTick();
            Registers.PC = (ushort)(Registers.PC + 1);
            OnTick();
            _pins.DeactivateOtherPins(OtherPins.MEMORY_READ);
            OnTick();
            return _pins.Data;
        }


        public Task RunOnce()
        {
            FetchOpcode();
            var instruction = _instructions[Registers.InstructionRegister];
            instruction.Action!(instruction, this);
            return Task.CompletedTask;
        }

        public void SetTickHandler(OnTickResponder responder)
        {
            _onTick = responder;
        }




        public byte IoRead(ushort address)
        {
            _pins.Address = address;
            _pins.ActivateOtherPins(OtherPins.IO_READ);
            OnTick();
            OnTick();
            _pins.DeactivateOtherPins(OtherPins.IO_READ);
            OnTick();
            OnTick();
            return _pins.Data;
        }
        
        public void MemoryWrite(ushort address, byte data)
        {
            _pins.Address = address;
            _pins.Data = data;
            _pins.ActivateOtherPins(OtherPins.MEMORY_WRITE);
            OnTick();
            _pins.ActivateOtherPins(OtherPins.WR);
            OnTick();
            _pins.DeactivateOtherPins(OtherPins.MEMORY_WRITE | OtherPins.WR);
            OnTick();
            
        }
        
        public void IoWrite(ushort address, byte data)
        {
            _pins.Address = address;
            _pins.Data = data;
            _pins.ActivateOtherPins(OtherPins.IO_WRITE);
            OnTick();
            _pins.ActivateOtherPins(OtherPins.WR);
            OnTick();
            _pins.DeactivateOtherPins(OtherPins.IO_WRITE | OtherPins.WR);
            OnTick();
            OnTick();
        }
        
    }
}
