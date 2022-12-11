using System.Linq.Expressions;

namespace Hw10.MathCalculator;

public static class ExpressionConverter
{
    public static async Task<double> VisitAsync(Expression expression)
    {
        var visitor = new MyVisitor();
        var result = visitor.VisitWith(expression);
        return await result[expression].Value;
    }
    
}
