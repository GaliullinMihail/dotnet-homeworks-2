using static Hw11.Parser.MathTokenType;
namespace Hw11.Parser;

public class OperatorPrecedence
{
    public static int GetOperatorPrecedence(MathTokenType token) =>
        token switch
        {
            OpenBracket or CloseBracket => -1,
            Plus or Minus => 0,
            Multiply or Divide => 1,
            Negate => 2,
            _ => throw new ArgumentOutOfRangeException()
        };
}