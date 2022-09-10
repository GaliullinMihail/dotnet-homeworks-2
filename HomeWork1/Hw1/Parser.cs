namespace Hw1;

public static class Parser
{
    public static void ParseCalcArguments(string[] args,
        out double val1,
        out CalculatorOperation operation,
        out double val2)
    {
        if (!IsArgLengthSupported(args)) throw new ArgumentException();
        try
        {
            val1 = double.Parse(args[0]);
            val2 = double.Parse(args[2]);
        }

        catch
        {
            throw new ArgumentException();
        }

        operation = ParseOperation(args[1]);
        if (operation == CalculatorOperation.Undefined)
            throw new ArgumentOutOfRangeException();


    }

    private static bool IsArgLengthSupported(string[] args) => args.Length == 3;

    private static CalculatorOperation ParseOperation(string arg)
    {
        switch (arg)
        {
            case "+":
                return CalculatorOperation.Plus;
            case "-":
                return CalculatorOperation.Minus; ;
            case "*":
                return CalculatorOperation.Multiply;
            case "/":
                return CalculatorOperation.Divide;
            case "_":
                return CalculatorOperation.Undefined;
            default:
                throw new InvalidOperationException();
        }
    }
}