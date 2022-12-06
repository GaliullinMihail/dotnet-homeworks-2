using System.Linq.Expressions;

namespace Hw11.MathCalculator;

public class ExpressionConverter
{
    public static async Task<double> VisitAsync(Expression expression)
    {
        var visitor = new Visitor();
        var result = visitor.VisitWith(expression);
        return await result[expression].Value;
    }
}