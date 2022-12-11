using Hw10.DbModels;
using Hw10.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly IMemoryCache _cache;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(IMemoryCache cache, IMathCalculatorService simpleCalculator)
	{
		_cache = cache;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		var alreadyCalculated = _cache.Get<double?>(expression);

		if (alreadyCalculated != null)
		{
			return new CalculationMathExpressionResultDto(alreadyCalculated.Value);
		}

		var calcExpDto = await _simpleCalculator.CalculateMathExpressionAsync(expression);

		if (!calcExpDto.IsSuccess) return calcExpDto;

		_cache.Set(expression, calcExpDto.Result);

		return calcExpDto;
	}
}
