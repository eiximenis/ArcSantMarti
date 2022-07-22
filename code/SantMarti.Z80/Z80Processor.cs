using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80
{
    public class Z80Processor
    {

        private IDataBus? _databus;
        private readonly Z80Instructions _instructions;
        public Z80Registers Registers { get; }

        public Z80Processor()
        {
            Registers = new Z80Registers();
            _databus = null;
            _instructions = new Z80Instructions();
        }

        public void ConnectToDataBus(IDataBus databus) => _databus = databus;


        internal byte FetchAt(ushort address)
        {
            var opcode = _databus.Fetch(address);
            return opcode;
        }

        public Task RunOnce()
        {
            var address = Registers.PC;
            var opcode = FetchAt(address);
            var instruction = _instructions[opcode];
            var nextAddres = instruction.Action(instruction, this, address);
            Registers.PC = nextAddres;
            return Task.CompletedTask;
        }
    }
}
