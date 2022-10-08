open System
open Hw4
open Hw4.Parser
open Hw4.Calculator
let args = [|"1"; "+"; "3"|]
let main args =
    let calc = parseCalcArguments args
    let result = calculate calc.arg1 calc.operation calc.arg2
    
    printfn $"{result}"
    0