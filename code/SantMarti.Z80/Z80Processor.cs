using System;
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

        private void OnTick()
        {
            _onTick(ref _pins);
        }

    
        /// <summary>
        /// Fetch an opcode from the memory. This operation
        /// **usually** takes 4 ticks:
        ///  Ticks 1 and 2 are for memory read
        ///  Ticks 3 and 4 are for decoding the instruction
        internal byte FetchOpcode()
        {
            _pins.SetOthers(OtherPins.M1 | OtherPins.MEMORY_READ);
            _pins.Address = Registers.PC;
            OnTick();
            OnTick();
            // TODO: Check WAIT states
            _pins.ClearOthers(OtherPins.M1 | OtherPins.RD);
            var opcode = _pins.Data;
            OnTick();
            _pins.ClearOthers(OtherPins.MREQ);
            OnTick();
            return opcode;
        }
        
        /// <summary>
        /// Fetch a byte from the memory. This operation
        /// usually takes 3 ticks:
        /// Ticks 1 and 2 are for memory read
        /// Tick 3 is for getting the result
        /// </summary>
        /// <returns></returns>
        internal byte ReadMemoryAddress(ushort address)
        {
            _pins.SetOthers(OtherPins.M1 | OtherPins.MEMORY_READ);
            _pins.Address = address;
            OnTick();
            OnTick();
            // TODO: Check WAIT states
            _pins.ClearOthers(OtherPins.M1 | OtherPins.RD);
            var value = _pins.Data;
            _pins.ClearOthers(OtherPins.MREQ);
            OnTick();
            return value;
        }

        public Task RunOnce()
        {
            var opcode = FetchOpcode();
            var instruction = _instructions[opcode];
            instruction.Action(instruction, this);
            return Task.CompletedTask;
        }

        public void SetTickHandler(OnTickResponder responder)
        {
            _onTick = responder;
        }

        public byte MemoryRead(ushort address)
        {
            _pins.Address = address;
            _pins.SetOthers(OtherPins.MEMORY_READ);
            OnTick();
            OnTick();
            _pins.ClearOthers(OtherPins.MEMORY_READ);
            OnTick();
            return _pins.Data;
        }

        public byte IoRead(ushort address)
        {
            _pins.Address = address;
            _pins.SetOthers(OtherPins.IO_READ);
            OnTick();
            OnTick();
            _pins.ClearOthers(OtherPins.IO_READ);
            OnTick();
            OnTick();
            return _pins.Data;
        }
        
        public void MemoryWrite(ushort address, byte data)
        {
            _pins.Address = address;
            _pins.Data = data;
            _pins.SetOthers(OtherPins.MEMORY_WRITE);
            OnTick();
            _pins.SetOthers(OtherPins.WR);
            OnTick();
            _pins.ClearOthers(OtherPins.MEMORY_WRITE | OtherPins.WR);
            OnTick();
            
        }
        
        public void IoWrite(ushort address, byte data)
        {
            _pins.Address = address;
            _pins.Data = data;
            _pins.SetOthers(OtherPins.IO_WRITE);
            OnTick();
            _pins.SetOthers(OtherPins.WR);
            OnTick();
            _pins.ClearOthers(OtherPins.IO_WRITE | OtherPins.WR);
            OnTick();
            OnTick();
        }
        
    }
}
