using Hw1;

double arg1;
CalculatorOperation operation;
double arg2;

Parser.ParseCalcArguments(new string[]{args[0], args[1], args[2]},out arg1, out operation, out arg2);

Console.WriteLine(Calculator.Calculate(arg1, operation, arg2));