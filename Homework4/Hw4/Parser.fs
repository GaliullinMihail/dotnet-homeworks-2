module Hw4.Parser

open System
open Hw4.Calculator


type CalcOptions = {
    arg1: float
    arg2: float
    operation: CalculatorOperation
}

let isArgLengthSupported (args : string[]) = args.Length = 3

let parseOperation (arg : string) =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | "_" -> raise(ArgumentOutOfRangeException())
    | _ -> raise (ArgumentException())
    
let parseCalcArguments(args : string[]) =
    if not (isArgLengthSupported(args))
    then
        raise (ArgumentException())
    let parse1, value1 = System.Double.TryParse(args[0])
    let parse2, value2 = System.Double.TryParse(args[2])
    if not ((parse1 && parse2) = true)
    then
        raise (ArgumentException())
    let value3 = parseOperation(args[1])
    {
        arg1 = value1
        arg2 = value2
        operation = value3
    }