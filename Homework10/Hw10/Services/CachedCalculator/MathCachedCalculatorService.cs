using Hw10.DbModels;
using Hw10.Dto;
using Microsoft.EntityFrameworkCore;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly ApplicationContext _dbContext;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(ApplicationContext dbContext, IMathCalculatorService simpleCalculator)
	{
		_dbContext = dbContext;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		var alreadyCalculated =  await _dbContext.SolvingExpressions.FirstOrDefaultAsync(solving => solving.Expression == expression);
		
		if (alreadyCalculated != null)
			return new CalculationMathExpressionResultDto(alreadyCalculated.Result);

		var calcExpDto = await _simpleCalculator.CalculateMathExpressionAsync(expression);

		if (!calcExpDto.IsSuccess) return calcExpDto;
		
		_dbContext.SolvingExpressions.Add(new SolvingExpression(expression, calcExpDto.Result));

		await _dbContext.SaveChangesAsync();

		return calcExpDto;
	}
}
