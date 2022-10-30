using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public ActionResult<double> Calculate([FromServices] ICalculator calculator,
        string val1,
        string operation,
        string val2)
    {
        double result;
        (double, Operation, double) parsedData;
        try
        {
            parsedData = Parser.Parse(val1, operation, val2);
            result = calculator.Calculate(parsedData.Item1, parsedData.Item2, parsedData.Item3);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }

        return Ok(result);

    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(Plus, Minus, Multiply, Divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}
