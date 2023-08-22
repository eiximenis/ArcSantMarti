using SantMarti.Z80.Assembler.Tokens;
using FluentAssertions;

namespace SantMarti.Z80.Assembler.Tests;

public class AssemblerLineParserTests
{
    [Fact]
    public void Line_Starting_With_Semicolon_Should_Have_A_Single_Comment_Token()
    {
        var line = "; this is just a comment";
        var parser = new AssemblerLineParser();
        var tokenizedLine = parser.Parse(line);
        tokenizedLine.Tokens.Should().HaveCount(1);
        tokenizedLine.Tokens.Single().Type.Should().Be(TokenType.Comment);
    }

    [Fact]
    public void Line_With_Single_Opcode_Should_Have_A_Single_Opcode_Token() 
    {
        var line = "NOP";
        var parser = new AssemblerLineParser();
        var tokenizedLine = parser.Parse(line);
        tokenizedLine.Tokens.Should().HaveCount(1);
        tokenizedLine.Tokens.Single().Type.Should().Be(TokenType.Opcode);
    }

    [Fact]
    public void Line_With_Label_And_Single_Opcode_Should_Have_Both_Tokens()
    {
        var line = "here: NOP";
        var parser = new AssemblerLineParser();
        var tokenizedLine = parser.Parse(line);
        tokenizedLine.Tokens.Should().HaveCount(2);
        tokenizedLine.Tokens.First().Type.Should().Be(TokenType.Label);
        tokenizedLine.Tokens.Last().Type.Should().Be(TokenType.Opcode);
    }

    [Theory]
    [InlineData("LD A, B")]
    [InlineData("LD A,B")]
    public void Line_With_Opcode_And_Two_Register_Should_Have_Three_Tokens(string line)
    {
        var parser = new AssemblerLineParser();
        var tokenizedLine = parser.Parse(line);
        tokenizedLine.Tokens.Should().HaveCount(3);
        tokenizedLine.Tokens.Select(t => t.Type).Should().BeEquivalentTo(new[] { TokenType.Opcode, TokenType.Register, TokenType.Register });
    }

    [Theory]
    [InlineData("LD A, (HL)")]
    [InlineData("LD A,(HL)")]
    public void Line_With_Opcode_Register_And_Reference_Should_Have_Three_Tokens(string line)
    {
        var parser = new AssemblerLineParser();
        var tokenizedLine = parser.Parse(line);
        tokenizedLine.Tokens.Should().HaveCount(3);
        tokenizedLine.Tokens.Select(t => t.Type).Should().BeEquivalentTo(new[] { TokenType.Opcode, TokenType.Register, TokenType.MemoryReference });
    }

    [Theory]
    [InlineData("LD A, (IX + 2)")]
    [InlineData("LD A,(IX + 2)")]
    public void Line_With_Opcode_Register_And_Displacement_Should_Have_Three_Tokens(string line)
    {
        var parser = new AssemblerLineParser();
        var tokenizedLine = parser.Parse(line);
        tokenizedLine.Tokens.Should().HaveCount(3);
        tokenizedLine.Tokens.Select(t => t.Type).Should().BeEquivalentTo(new[] { TokenType.Opcode, TokenType.Register, TokenType.Displacement });
    }

    [Theory]
    [InlineData("LD A, 10")]
    [InlineData("LD A,10")]
    public void Line_With_Opcode_Register_And_Number_Should_Have_Three_Tokens(string line)
    {        
        var parser = new AssemblerLineParser();
        var tokenizedLine = parser.Parse(line);
        tokenizedLine.Tokens.Should().HaveCount(3);
        tokenizedLine.Tokens.Select(t => t.Type).Should().BeEquivalentTo(new[] { TokenType.Opcode, TokenType.Register, TokenType.Number });
    }

    [Fact]
    public void Line_Ending_With_A_Comment_Should_Have_Comment_As_Last_Token()
    {
        var line = "NOP        ; Nothing to do";
        var parser = new AssemblerLineParser();
        var tokenizedLine = parser.Parse(line);
        tokenizedLine.Tokens.Should().HaveCount(2);
        tokenizedLine.Tokens.First().Type.Should().Be(TokenType.Opcode);
        tokenizedLine.Tokens.Last().Type.Should().Be(TokenType.Comment);
    }
}