using static Hw11.Parser.MathTokenType;
using static Hw11.ErrorMessages.MathErrorMessager;
using System.Globalization;
using System.Linq.Expressions;

namespace Hw11.Parser;

public static class Parser
{
    public static Expression Parse(string? expression)
    {
        var tokenList = Tokenizer.ParseToTokens(expression);
        CheckList(tokenList);
        var expressionStack = new Stack<Expression>();
        var operatorStack = new Stack<MathToken>();
        foreach (var token in tokenList)
        {
            if (token.Type == Number)
            {
                expressionStack.Push(Expression.Constant(double.Parse(token.Value, CultureInfo.InvariantCulture)));
                continue;
            }

            if (token.Type == OpenBracket)
            {
                operatorStack.Push(token);
                continue;
            }

            if (token.IsOperator())
            {
                if (operatorStack.Count != 0 &&
                    token.GetPrecedence() <= operatorStack.Peek().GetPrecedence())
                {
                    CalculatePrevExpression(token, expressionStack, operatorStack);
                }

                operatorStack.Push(token);
            }
            else if (token.Type == CloseBracket)
            {
                CalculateBrackets(expressionStack, operatorStack);
            }

        }

        while (operatorStack.Count != 0)
        {
            PushExpression(operatorStack.Pop(), expressionStack);
        }

        return expressionStack.Pop();
    }

    private static void CalculateBrackets(Stack<Expression> expressionsStack, Stack<MathToken> operatorStack)
    {
        while (operatorStack.Peek().Type != OpenBracket)
        {
            PushExpression(operatorStack.Pop(), expressionsStack);
        }

        operatorStack.Pop();
    }

    private static void CalculatePrevExpression(MathToken token, Stack<Expression> expressionStack,
        Stack<MathToken> operatorStack)
    {
        while (operatorStack.Count != 0 && token.GetPrecedence() <= operatorStack.Peek().GetPrecedence())
        {
            PushExpression(operatorStack.Pop(), expressionStack);
        }
    }

    private static void PushExpression(MathToken token, Stack<Expression> expressionStack)
    {
        switch (token.Type)
        {
            case Minus:
            {
                var secondValue = expressionStack.Pop();
                var firstValue = expressionStack.Pop();
                expressionStack.Push(Expression.Subtract(firstValue, secondValue));
                break;
            }
            case Plus:
            {
                expressionStack.Push(Expression.Add(expressionStack.Pop(), expressionStack.Pop()));
                break;
            }
            case Multiply:
            {
                expressionStack.Push(Expression.Multiply(expressionStack.Pop(), expressionStack.Pop()));
                break;
            }
            case Divide:
            {
                var secondValue = expressionStack.Pop();
                var firstValue = expressionStack.Pop();
                expressionStack.Push(Expression.Divide(firstValue, secondValue));
                break;
            }
            case Negate:
            {
                expressionStack.Push(Expression.Negate(expressionStack.Pop()));
                break;
            }
        }
    }

    private static void CheckList(List<MathToken> list)
    {
        if (list[0].IsBinaryOperator())
            throw new Exception(StartingWithOperation);
        if (list[^1].IsBinaryOperator())
            throw new Exception(EndingWithOperation);

        var numberOfOpenBrackets = 0;
        for (var i = 0; i < list.Count; i++)
        { 
            CheckTwoOpInRow(list, i); 
            CheckOpBeforeParen(list, i); 
            if (list[i].Type == CloseBracket)
                {
                    numberOfOpenBrackets--;
                    if (numberOfOpenBrackets < 0)
                        throw new Exception(IncorrectBracketsNumber);
                }
            
            CheckOpAfterParen(list, i);
            if (list[i].Type == OpenBracket)
            {
                numberOfOpenBrackets++;
            }


        }

        if (numberOfOpenBrackets != 0)
        {
            throw new Exception(IncorrectBracketsNumber);
        }
    }

    private static void CheckOpBeforeParen(List<MathToken> list, int position)
    {
        if (position > 0 && list[position].Type == CloseBracket && !(list[position - 1].IsNumber() ||
                                                                     list[position - 1].Type == CloseBracket ||
                                                                     list[position - 1].Type == OpenBracket))
        {
            throw new Exception(OperationBeforeParenthesisMessage(list[position - 1].Value));
        }
    }


    private static void CheckOpAfterParen(List<MathToken> list, int position)
    {
        if (position < list.Count-1 && list[position].Type == OpenBracket && list[position + 1].IsBinaryOperator())
        {
            throw new Exception(InvalidOperatorAfterParenthesisMessage(list[position + 1].Value));
        }
    }

    private static void CheckTwoOpInRow(List<MathToken> list, int position)
    {
        if (position > 0 && list[position - 1].IsBinaryOperator() && list[position].IsBinaryOperator())
        {
            throw new Exception(TwoOperationInRowMessage(list[position - 1].Value, list[position].Value));
        }
    }
}