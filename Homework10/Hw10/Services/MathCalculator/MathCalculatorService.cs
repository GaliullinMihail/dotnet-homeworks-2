using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Hw10.Dto;
using Hw10.MathCalculator;

namespace Hw10.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        try
        {
            
            var b = Parser.Parser.Parse(expression);
            var c =  await ExpressionConverter.VisitAsync(b);
            return new CalculationMathExpressionResultDto(c);
        }
        catch (Exception e)
        {
            return new CalculationMathExpressionResultDto(e.Message);
        }
    }
}
