using Hw11.Dto;
using Hw11.MathCalculator;

namespace Hw11.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<double> CalculateMathExpressionAsync(string? expression)
    {

        var parsedExp = Parser.Parser.Parse(expression);
        var result = await ExpressionConverter.VisitAsync(parsedExp);
        return new CalculationMathExpressionResultDto(result).Result;
    }
}