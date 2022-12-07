using static Hw10.ErrorMessages.MathErrorMessager;
using static Hw10.Parser.MathTokenType;
namespace Hw10.Parser;

public static class Tokenizer
{
    public static List<MathToken> ParseToTokens(string? expression) {
        if (string.IsNullOrEmpty(expression))
            throw new Exception(EmptyString);

        var parts = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var result = new List<MathToken>();
        
        foreach (var part in parts) {
            var position = 0;
            while (position < part.Length && (IsOperator(part[position]) || IsBracket(part[position])))
            {
                if (IsOperator(part[position]))
                {
                    result.Add(GetTokenOperator(part, position));
                }
                else if (IsBracket(part[position]))
                    result.Add(GetTokenBracket(part,position));

                position++;
            }

            if (position < part.Length)
            {
                if (char.IsDigit(part[position]))
                {
                    result.Add(GetTokenDigit(part, ref position));
                }
                
                else
                {
                    throw new Exception(UnknownCharacterMessage(part[position]));
                }
            }

            while (position < part.Length && IsBracket(part[position]))
            {
                result.Add(GetTokenBracket(part, position));
                position++;
            }

        }

        return result;
    }

    private static MathToken GetTokenDigit(string expression, ref int position)
    {
        var previousPos = position;
        while (position < expression.Length && char.IsDigit(expression[position]))
        {
            position++;
        }

        if (position < expression.Length && expression[position] == '.')
        {
            position++;
            while (position < expression.Length && char.IsDigit(expression[position]))
            {
                position++;
            }
        }

        if (position < expression.Length && !IsBracket(expression[position]))
            throw new Exception(NotNumberMessage(expression));

        return new MathToken(Number, expression.Substring(previousPos, position - previousPos));
    }

    public static MathToken GetTokenOperator(string part, int position)
        => part[position] switch
        {
            '+' => new MathToken(Plus , "+"),
            '-' => GetTokenMinusOrNegate(part),
            '/' => new MathToken(Divide, "/"),
            '*' => new MathToken(Multiply, "*"),
            _ => throw new ArgumentOutOfRangeException()
        };

    public static MathToken GetTokenBracket(string part, int position)
        => part[position] switch
        {
            '(' => new MathToken(OpenBracket, "("),
            ')' => new MathToken(CloseBracket, ")"),
            _ => throw new ArgumentOutOfRangeException()
        };

    private static MathToken GetTokenMinusOrNegate(string part) => 
        part.Length == 1 ? new MathToken(Minus, "-") : new MathToken(Negate, "-");

    private static bool IsBracket(char someChar) =>
        someChar switch
        {
            '(' => true,
            ')' => true,
            _ => false
        };

    private static bool IsOperator(char someChar) => 
        someChar switch
        {
            '+' => true,
            '-' => true,
            '*' => true,
            '/' => true,
            _ => false
        };

}
