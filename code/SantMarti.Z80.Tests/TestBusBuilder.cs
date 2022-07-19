using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Tests
{
    class TestBusBuilder
    {
        private readonly List<byte> _buffer;

        public TestBusBuilder()
        {
            _buffer = new List<byte>(1024);
        }

        public TestBusBuilder Add(byte data)
        {
            _buffer.Add(data);
            return this;
        }
        public TestBusBuilder Add(IEnumerable<byte> bytes)
        {
            _buffer.AddRange(bytes);
            return this;
        }

        public IDataBus BuildBus()
        {
            return new TestDataBus(_buffer);
        }

    }
}
