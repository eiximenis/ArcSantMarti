using SantMarti.Spectrum.Memory;
using SantMarti.Z80;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Spectrum
{
    public class Spectrum
    {
        
        private readonly Z80Processor _processor;
        private readonly SpectrumMemory _memory;
        
        private Spectrum(SpectrumMemory memory)
        {
            _processor = new Z80Processor();
            _memory = memory;
            _processor.SetTickHandler(OnTick);
        }
        
        private void OnTick(ref Z80Pins pins, int ticks)
        {

            if (pins.OthersAreSet(OtherPins.MEMORY_READ))
            {
                pins.Data = _memory.Data[pins.Address];
            }
            else if (pins.OthersAreSet(OtherPins.MEMORY_WRITE))
            {
                _memory.Data[pins.Address] = pins.Data;
            }
        }

        private static async Task<Spectrum> Create(string romFilePath, int romSize, int ramSize)
        {
            var spectrum = new Spectrum(new SpectrumMemory(romSize, ramSize));
            await spectrum._memory.LoadRomFromFile(romFilePath);
            return spectrum;
        }

        public static async Task<Spectrum> Spectrum48()
        {
            var memory = new SpectrumMemory(16, 48)
            {
                ScreenMemoryOffset = 0x4000,
                ScreenMemoryColorDataOffset = 0x5800,
                PrinterBufferOffset = 0x5b00,
                SystemVariables = 0x5c00,
                ReservedOffset = 0x5cc0,
                AvailableRamOffset = 0x5ccb,
                Reserved2Offset = 0xff58
            };
            await memory.LoadRomFromFile(Path.Combine("roms", "48e.rom"));
            return new Spectrum(memory);
        }

        
    }
}
    