module Hw5.Program

open System
open Hw5

let success (arg1, operation, arg2) =
    printfn $"{Calculator.calculate arg1 operation arg2}"
    0

let failure message =
    printfn $"{message}"
    -1

[<EntryPoint>]
let main (args: string[]) =
    match Parser.parseCalcArguments args with
    | Ok parsedArgs -> success parsedArgs
    | Error message -> failure message