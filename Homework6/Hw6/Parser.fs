module Hw6.Parser

open System.Globalization
open System
open Hw6.Calculator
open Hw6.MaybeBuilder
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2) =
    match operation with
    | Calculator.plus -> Ok (arg1,CalculatorOperation.Plus, arg2)
    | Calculator.minus -> Ok (arg1,CalculatorOperation.Minus, arg2)
    | Calculator.multiply -> Ok (arg1,CalculatorOperation.Multiply, arg2)
    | Calculator.divide -> Ok (arg1,CalculatorOperation.Divide, arg2)
    | _ -> Error $"Could not parse value '{operation}'"

let parse (value : string) =
    match Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture) with
    | true, arg -> Ok arg
    | _ ->  Error $"Could not parse value '{value}'"
let parseArgs (args: string[]) =
    let result =
        maybe {
            let! arg1 = parse(args[0])
            let! arg2 = parse(args[2])
            return (arg1, args[1], arg2)
        }
        
    result
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2) =
    match operation, arg2 with
    | CalculatorOperation.Divide, 0.0 -> Error "DivideByZero"
    | _ -> Ok (arg1, operation, arg2)
    
let parseCalcArguments (args: string[]) =
        let result = maybe{
            let! parsedArgs = args |> parseArgs
            let! parsedOperation=  parsedArgs |> isOperationSupported
            let! result = parsedOperation |> isDividingByZero
            return result
        }
        
        match result with
        | Ok parsed -> Ok $"{parsed |||> calculate}"
        | Error message -> Error $"{message}"