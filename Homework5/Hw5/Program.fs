module Hw5.Program

open System
open Hw5

let success (arg1, operation, arg2) =
    printfn $"{Calculator.calculate arg1 operation arg2}"
    Message.SuccessfulExecution
    


let failure (message) =
    printfn $"{message}"
    message

   
let main (args: string[]) =
    match Parser.parseCalcArguments args with
    | Ok parsedArgs -> success parsedArgs
    | Error message -> failure message