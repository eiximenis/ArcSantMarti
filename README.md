# ArcSanMarti

An attempt to create a tick-based emulator of Z80 and related computers (starting with ZX Spectrum).

## What means a tick-based emulator?

Some emulators works at "instruction level". That means that the emulated processor runs an entire instruction (like a `LD A,(HL)`) in a single atomic operation. When the instruction finishes, the control can return to the caller.

In a tick-based emulator, the "tick" is the smallest operation. An emulated processor instruction can last (and usually do) for more than one tick. For example, in Z80 all instructions last **at minimum 4 ticks**: 2 ticks for reading the next opcode from memory and 2 ticks more to decode the opcode. If more memory accesses and other I/O operations are necessary, then the instruction can take more ticks to complete. In a tick-based emulator, the control is returned to the caller at each tick, that means **you can revert or change the state of the emulated system in the middle of an instruction execution**.

In this emulator the communication between the processor (the emulated Z80) and other peripherals (like memory) is modelled by the processor pins (as happen in the real device). So, **the only external observable state of the processor are the value of its pins**. All peripherals are expected to interact with the Z80 by reading and writing its pins.

## Which is the current state of the project

WIP, WIP, WIP, WIP. Everything is on WIP

## When a first release is expected?

Who knows... This is a pet project, developed in my spare time, so, to be honest, I don't know :)


