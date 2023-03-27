using System.Runtime.InteropServices;
using FluentAssertions;

namespace SantMarti.Z80.Tests;

public class TestTickHandlerTests
{
    
    [Theory]
    [InlineData(0)]
    [InlineData(0xff)]
    [InlineData(0x11aa)]
    public void OnReadMemory_Should_Return_Zero_For_EveryRead_If_No_Handlers_Are_Configured(ushort address)
    {
        byte expectedValue = 0x0;
        var processor = new Z80Processor();
        var testTickHandler = new TestTickHandler(processor);
        var data = processor.MemoryRead(address);
        data.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(0xff)]
    [InlineData(0x11aa)]
    public void OnReadMemory_Should_Call_Default_Read_Handler_If_There_Are_No_Any_Specific_Read_Handler(ushort address)
    {
        byte expectedValue = 0x12;
        var processor = new Z80Processor();
        var testTickHandler = new TestTickHandler(processor);
        testTickHandler.OnDefaultMemoryRead(_ => expectedValue);
        var data = processor.MemoryRead(address);
        data.Should().Be(expectedValue);
    }
        
    
    [Fact]
    public void OnReadMemory_Should_Call_Specific_Read_Handler_If_Specific_Read_Handler_Is_Found()
    {
        var addressToRead=(ushort)0x1234;
        byte expectedValue = 0x12;
        var processor = new Z80Processor();
        var testTickHandler = new TestTickHandler(processor);
        testTickHandler.OnMemoryRead(addressToRead, () => expectedValue);
        var data = processor.MemoryRead(addressToRead);
        data.Should().Be(expectedValue);
    }
    
    [Fact]
    public void OnReadMemory_Should_Call_Default_Read_Handler_If_Specific_Read_Handler_Is_Not_Found()
    {
        var specificAddress=(ushort)0x1234;
        var addressToRead = (ushort)0xffaa;
        byte expectedValue = 0x12;
        byte specificValue = 0xff;
        var processor = new Z80Processor();
        var testTickHandler = new TestTickHandler(processor);
        testTickHandler.OnDefaultMemoryRead(_ => expectedValue);
        testTickHandler.OnMemoryRead(specificAddress, () => specificValue);
        var data = processor.MemoryRead(addressToRead);
        data.Should().Be(expectedValue);
    }
    
    
}