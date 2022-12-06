using System.Linq.Expressions;
using Hw9.MathCalculator;
using Hw9.Parser;
using Xunit;

namespace Hw9.Tests;

public class UnitTests
{
    [Fact]
    public void TestTokenizerTokenOperatorOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Tokenizer.GetTokenOperator("123", 1));
    }

    [Fact]
    public void TestTokenizerGetTokenBracketOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Tokenizer.GetTokenBracket("123", 1));
    }

    [Fact]
    public void TestGetOperatorPrecedenceOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            OperatorPrecedence.GetOperatorPrecedence(MathTokenType.Number));
    }

    [Fact]
    public void TestMyVisitorCalculateOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new MyVisitor().Calculate(Expression.Decrement(Expression.Constant(1))));
    }
}