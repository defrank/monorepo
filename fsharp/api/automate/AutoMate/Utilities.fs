module AutoMate.Utilities

[<RequireQualifiedAccess>]
module Str =
  open System

  let whitespace = "\t\n\v\f\r "

  let toLower (s: string) = s.ToLower()
  let toUpper (s: string) = s.ToUpper()

  let startsWith (prefix: string) (s: string) = s.StartsWith prefix
  let endsWith (suffix: string) (s: string) = s.EndsWith suffix

  let split (separators: string) (s: string) =
    s.Split(separators.ToCharArray()) |> List.ofArray

  let splitWhitespace = split whitespace

  let splitMax (max: int) (separators: string) (s: string) =
    s.Split(separators.ToCharArray(), max) |> List.ofArray

  let splitWhitespaceMax (max: int) = splitMax max whitespace

  let splitWord (word: string) (s: string) = s.Split word |> List.ofArray

  let splitWordMax (max: int) (word: string) (s: string) =
    s.Split(word, max) |> List.ofArray

  let join = String.concat

[<RequireQualifiedAccess>]
module Opt =
  let wantSome errorMessage =
    function
    | Some value -> value
    | None -> failwith errorMessage

[<RequireQualifiedAccess>]
module Json =
  open FSharp.Json
  open Validus

  type Field = JsonField

  let config = JsonConfig.create (jsonFieldNaming = Json.snakeCase)

  let serialize data = Json.serializeEx config data

  let deserialize<'T> json =
    try
      Json.deserializeEx<'T> config json |> Ok
    with ex ->
      Error ex

  let deserializeValidator<'T> : Validator<string, 'T> =
    fun (field: string) (input: string) ->
      input
      |> Check.String.notEmpty field
      |> Result.bind (
        deserialize<'T>
        >> function
          | Ok v -> Ok v
          | Error e -> Error <| ValidationErrors.create field [ e.Message ]
      )

[<RequireQualifiedAccess>]
module Env =
  open System

  let get name =
    match Environment.GetEnvironmentVariable name with
    | null -> None
    | value -> Some value

  let getDefault fallback name =
    match get name with
    | Some value -> value
    | None -> fallback

  let want name =
    get name |> Opt.wantSome $"Missing environment variable - ${name}"

  let set name value =
    Environment.SetEnvironmentVariable(name, value)
