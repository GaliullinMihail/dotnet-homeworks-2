module Hw6.App

open System
open System.Diagnostics.CodeAnalysis
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Hw6.MaybeBuilder
open Hw6.Parser

let calculatorHandler: HttpHandler =
    fun next ctx ->
        let result = maybe {
            let! val1 = ctx.GetQueryStringValue "value1"
            let! val2 = ctx.GetQueryStringValue "value2"
            let! operation = ctx.GetQueryStringValue "operation"
            let args = [|val1; operation; val2;|]
            let! calculated  = parseAndCalc args
            return calculated
        }

        match result with
        | Ok ok -> (setStatusCode 200 >=> text (ok.ToString())) next ctx
        | Error error ->
            match error with
            | "DivideByZero" -> (setStatusCode 200 >=> text error) next ctx
            | _ -> (setStatusCode 400 >=> text error) next ctx
                

let webApp =
    choose [
        GET >=> choose [
             route "/" >=> text "Use /calculate?value1=<VAL1>&operation=<OPERATION>&value2=<VAL2>"
             route "/calculate" >=> calculatorHandler
        ]
        setStatusCode 404 >=> text "Not Found"
    ]
    
type Startup() =
    member _.ConfigureServices (services : IServiceCollection) =
        services.AddGiraffe() |> ignore

    member _.Configure (app : IApplicationBuilder) (_ : IHostEnvironment) (_ : ILoggerFactory) =
        app.UseGiraffe webApp
[<ExcludeFromCodeCoverage>]
[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun whBuilder -> whBuilder.UseStartup<Startup>() |> ignore)
        .Build()
        .Run()
    0