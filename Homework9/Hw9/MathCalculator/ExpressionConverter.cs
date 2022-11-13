using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Hw9.ErrorMessages;

namespace Hw9.MathCalculator;

public static class ExpressionConverter
{
    public static async Task<double> VisitAsync(Expression expression)
    {
        var visitor = new MyVisitor();
        var b = visitor.VisitWith(expression);
        return await b[expression].Value;
    }
    
}