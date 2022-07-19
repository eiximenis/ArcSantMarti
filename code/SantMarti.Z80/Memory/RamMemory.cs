﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Memory
{
    public class RamMemory
    {
        private readonly byte[] _buffer;
        public int Kib { get; }
        public RamMemory(int kib)
        {
            Kib = kib;
            _buffer = new byte[kib * 1024];
        }


    }
}
