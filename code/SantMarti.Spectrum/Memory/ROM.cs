using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanMarti.Spectrum.Memory
{
    public class ROM
    {
        private readonly byte[] _memory;

        public ROM(byte[] contents)
        {
            _memory = contents;
        }

        public static async Task<ROM> CreateFromFile(string filePath)
        {
            var romFileInfo = new FileInfo(filePath);
            var romSize= romFileInfo.Length;

            var buffer= await File.ReadAllBytesAsync(filePath);
            return new ROM(buffer);
        }
    }
}
