using System.Linq.Expressions;
using Hw10.ErrorMessages;

namespace Hw10.MathCalculator;

public class MyVisitor :ExpressionVisitor
{
    public Dictionary<Expression, Lazy<Task<Double>>> Dictionary = new();
    
    protected override Expression VisitBinary(BinaryExpression node)
    {
        Dictionary[node] =
            new Lazy<Task<double>>(async () =>
            {
                await Task.Delay(1000);
                await Task.WhenAll(Dictionary[node.Left].Value, Dictionary[node.Right].Value);

                return Calculate(node, await Dictionary[node.Left].Value, await Dictionary[node.Right].Value);
            });
        return base.VisitBinary(node);
    }
    
    protected override Expression VisitUnary(UnaryExpression node)
    {
        Dictionary[node] =
            new Lazy<Task<double>>(async () =>
            { 
                await Task.Delay(1000);
                await Task.WhenAll(Dictionary[node.Operand].Value);
                return Calculate(node, await Dictionary[node.Operand].Value);
            });
        return base.VisitUnary(node);
    }
    
    protected override Expression VisitConstant(ConstantExpression node)
    {
        Dictionary[node] =
            new Lazy<Task<double>>(async () =>
            {
                return (double) node.Value;
            });
        return base.VisitConstant(node);
    }
    

    public double Calculate(Expression expression, params double[] values) =>
        expression.NodeType switch
        {
            ExpressionType.Add => values[0] + values[1],
            ExpressionType.Subtract => values[0] - values[1],
            ExpressionType.Multiply => values[0] * values[1],
            ExpressionType.Divide =>
                values[1] >= 1e-6 
                    ? values[0] / values[1] 
                    : throw new DivideByZeroException(MathErrorMessager.DivisionByZero),
            ExpressionType.Negate => -values[0],
            _ => throw new ArgumentOutOfRangeException()
        };

    public Dictionary<Expression, Lazy<Task<Double>>> VisitWith(Expression? node)
    {
        base.Visit(node);
        return Dictionary;
    }
}
