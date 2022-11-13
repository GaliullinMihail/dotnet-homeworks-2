using System.Linq.Expressions;

namespace Hw10.MathCalculator;

public static class ExpressionConverter
{
    public static async Task<double> VisitAsync(Expression expression)
    {
        var visitor = new MyVisitor();
        var b = visitor.VisitWith(expression);
        return await b[expression].Value;
    }
    
}
