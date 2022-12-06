using static Hw11.Parser.MathTokenType;
namespace Hw11.Parser;

public class MathToken
{
    public readonly MathTokenType Type;
    
    public string Value;

    public MathToken(MathTokenType type, string value)
    {
        Type = type;
        Value = value;
    }

    public bool IsBinaryOperator() =>
        Type switch
        {
            Plus => true,
            Minus => true,
            Multiply => true,
            Divide => true,
            _ => false
        };

    public bool IsUnaryOperator() => Type == Negate;

    public bool IsOperator() => IsBinaryOperator() || IsUnaryOperator();
    

    public bool IsNumber() => Type == Number;

    public int GetPrecedence() => OperatorPrecedence.GetOperatorPrecedence(Type);
}