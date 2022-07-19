namespace SantMarti.Z80.Tests
{
    internal class TestDataBus : IDataBus
    {

        private readonly byte[] _buffer;

        public TestDataBus(IEnumerable<byte> buffer)
        {
            _buffer = buffer.ToArray();
        }

        public byte Fetch(ushort address)
        {
            return _buffer[address];
        }
    }
}