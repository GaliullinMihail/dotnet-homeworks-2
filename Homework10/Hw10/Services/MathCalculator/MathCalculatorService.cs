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
            
            var parsedExp = Parser.Parser.Parse(expression);
            var result =  await ExpressionConverter.VisitAsync(parsedExp);
            return new CalculationMathExpressionResultDto(result);
        }
        catch (Exception e)
        {
            return new CalculationMathExpressionResultDto(e.Message);
        }
    }
}
