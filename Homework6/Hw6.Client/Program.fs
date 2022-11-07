open System
open System.Net
open System.Net.Http
open System.Diagnostics.CodeAnalysis
open Microsoft.FSharp.Core

[<ExcludeFromCodeCoverage>]
let getResult (client : HttpClient) (uri : Uri) =
    async {
        let! response = client.GetAsync(uri) |> Async.AwaitTask
        let! result = response.Content.ReadAsStringAsync() |> Async.AwaitTask
        return result
    }

[<ExcludeFromCodeCoverage>]
[<EntryPoint>]
let main _ =
    use client = new HttpClient()
    while true do
        let input = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries)
        let uri = Uri($"http://localhost:5000/calculate?value1={input[0]}&operation={input[1]}&value2={input[2]}")
        let res = getResult client uri
        printfn $"{res |> Async.RunSynchronously}"
    0