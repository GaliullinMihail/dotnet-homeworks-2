using System.Linq.Expressions;
using System.Reflection.Metadata;
using Hw11.ErrorMessages;

namespace Hw11.MathCalculator;

public class Visitor
{
    private Dictionary<Expression, Lazy<Task<Double>>> dictionary = new();

    private void Visit(BinaryExpression node)
    {
        dictionary[node] =
            new Lazy<Task<double>>(async () =>
            {
                await Task.WhenAll(dictionary[node.Left].Value, dictionary[node.Right].Value);

                return Calculate(node, await dictionary[node.Left].Value, await dictionary[node.Right].Value);
            });
        
        Visit((dynamic) node.Left);
        Visit((dynamic) node.Right);
    }
    

    private void Visit(UnaryExpression unaryExpression)
    {
        dictionary[unaryExpression] =
            new Lazy<Task<double>>(async () =>
            {
                await Task.WhenAll(dictionary[unaryExpression.Operand].Value);
                return Calculate(unaryExpression, await dictionary[unaryExpression.Operand].Value);
            });
        Visit((dynamic) unaryExpression.Operand);
    }

    private void Visit(ConstantExpression constantExpression)
    {
        dictionary[constantExpression] =
            new Lazy<Task<double>>(async () =>
            {
                return (double) constantExpression.Value;
            });
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
        Visit((dynamic) node);
        return dictionary;
    }
}