module Hw5.Parser

open System
open Hw5.Calculator

let isArgLengthSupported (args:string[]) =
    match args.Length with
    | 3 -> Ok args
    | _ -> Error Message.WrongArgLength
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2) =
    match operation with
    | Calculator.plus -> Ok (arg1,CalculatorOperation.Plus, arg2)
    | Calculator.minus -> Ok (arg1,CalculatorOperation.Minus, arg2)
    | Calculator.multiply -> Ok (arg1,CalculatorOperation.Multiply, arg2)
    | Calculator.divide -> Ok (arg1,CalculatorOperation.Divide, arg2)
    | _ -> Error Message.WrongArgFormatOperation
let parseArgs (args: string[]) =
    let bool1, arg1 = Double.TryParse(args[0])
    let bool2, arg2 = Double.TryParse(args[2])
    match bool1 && bool2 with
    | true -> isOperationSupported(arg1, args[1], arg2)
    | _ -> Error Message.WrongArgFormat

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2) =
    match operation, arg2 with
    | CalculatorOperation.Divide, 0.0 -> Error Message.DivideByZero
    | _ -> Ok (arg1, operation, arg2)
    
let parseCalcArguments (args: string[]) =
    MaybeBuilder.maybe
        {
            let! lengthArgs = args |> isArgLengthSupported
            let! parsedArgs = lengthArgs |> parseArgs
            let! divideZeroChecker = parsedArgs |> isDividingByZero
            return divideZeroChecker
        }