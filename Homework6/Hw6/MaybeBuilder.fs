module Hw6.MaybeBuilder

type MaybeBuilder() =
    member builder.Bind(a, f) =
        match a with
        | Ok res -> f res
        | Error message ->  Error message
    member builder.Return x = Ok x
let maybe = MaybeBuilder()

