using System.Linq.Expressions;
using Hw9.ErrorMessages;
using System.Collections;

namespace Hw9.MathCalculator;

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
                await Task.Yield();
                
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
                await Task.Yield();
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
    

    private double Calculate(Expression expression, params double[] operands) =>
        expression.NodeType switch
        {
            ExpressionType.Add => operands[0] + operands[1],
            ExpressionType.Subtract => operands[0] - operands[1],
            ExpressionType.Multiply => operands[0] * operands[1],
            ExpressionType.Divide =>
                operands[1] >= 1e-6 
                    ? operands[0] / operands[1] 
                    : throw new DivideByZeroException(MathErrorMessager.DivisionByZero),
            ExpressionType.Negate => -operands[0],
            ExpressionType.Constant => (double)((ConstantExpression)expression).Value
        };

    public Dictionary<Expression, Lazy<Task<Double>>> VisitWith(Expression? node)
    {
        base.Visit(node);
        return Dictionary;
    }
}