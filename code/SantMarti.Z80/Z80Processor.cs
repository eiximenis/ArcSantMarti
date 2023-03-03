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
        internal byte Fetch()
        {
            _pins.SetOthers(OtherPins.M1 | OtherPins.MREQ | OtherPins.RD);
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

        public Task RunOnce()
        {
            var opcode = Fetch();
            var instruction = _instructions[opcode];
            var nextAddres = instruction.Action(instruction, this, address);
            Registers.PC = nextAddres;
            return Task.CompletedTask;
        }

        public void SetTickHandler(OnTickResponder responder)
        {
            _onTick = responder;
        }
    }
}
