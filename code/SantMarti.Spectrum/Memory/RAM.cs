using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanMarti.Spectrum.Memory
{
    public class RAM
    {
        private readonly byte[] _buffer;

        public int Kib { get; }

        public RAM(int kib)
        {
            Kib = kib;
            _buffer = new byte[kib * 1024];
        }
    }
}
