using SanMarti.Spectrum.Memory;
using SanMarti.Z80;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanMarti.Spectrum
{
    public class Spectrum
    {
        private readonly Z80Processor _processor;
        private readonly ROM _rom;
        private readonly RAM _ram;

        private Spectrum(RAM ram, ROM rom)
        {
            _processor = new Z80Processor();
            _ram = ram;
            _rom = rom;
        }

        public static async Task<Spectrum> Create(string romFilePath, int ramSizeInKib)
        {
            var rom = await ROM.CreateFromFile(romFilePath);
            var ram = new RAM(ramSizeInKib);
            var spectrum = new Spectrum(ram, rom);
            return spectrum;
        }

        public static Task<Spectrum> Spectrum48() => Create(Path.Combine("roms", "48e.rom"), 48);

        
    }
}
