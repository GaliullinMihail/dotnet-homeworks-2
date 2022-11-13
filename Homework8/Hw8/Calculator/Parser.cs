using System.Globalization;
using static System.Double;

namespace Hw8.Calculator;

public static class Parser  
{
    public static (double parsedValue1, Operation parsedOperation, double parsedValue2) Parse(string value1, string operation, string value3)
    {
        var val1 = ParseDouble(value1);
        var val2 = ParseDouble(value3);
        var parsedOperation = ParseOperation(operation);
        return (val1, parsedOperation, val2);
    }

    private static double ParseDouble(string value)
    {
        var boolean = TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result);
        
        return  boolean? result: throw new ArgumentException(Messages.InvalidNumberMessage);
    }

    private static Operation ParseOperation(string value) => value switch
    {
        "Plus" => Operation.Plus,
        "Minus" => Operation.Minus,
        "Multiply" => Operation.Multiply,
        "Divide" => Operation.Divide,
        _ => throw new InvalidOperationException(Messages.InvalidOperationMessage)
    };
}