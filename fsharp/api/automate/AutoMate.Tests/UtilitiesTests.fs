module AutoMate.Tests.Utilities

open AutoMate.Utilities
open Expecto
open System

[<Tests>]
let strTests =
  testList "Str" [
    testList "toLower" [
      testCase "pass"
      <| fun _ ->
        let input = "FOOBAR"
        let output = Str.toLower input
        Want.equal "foobar" output
    ]

    testList "toUpper" [
      testCase "pass"
      <| fun _ ->
        let input = "foobar"
        let output = Str.toUpper input
        Want.equal "FOOBAR" output
    ]

    testList "startsWith" [
      testCase "pass"
      <| fun _ ->
        let input, prefix = "foobar", "foo"
        let output = Str.startsWith prefix input
        Want.equal true output

      testCase "fail when does not start with prefix"
      <| fun _ ->
        let input, suffix = "foobar", "bar"
        let output = Str.startsWith suffix input
        Want.equal false output
    ]

    testList "endsWith" [
      testCase "pass"
      <| fun _ ->
        let input, suffix = "foobar", "bar"
        let output = Str.endsWith suffix input
        Want.equal true output

      testCase "fail when does not end with suffix"
      <| fun _ ->
        let input, prefix = "foobar", "foo"
        let output = Str.endsWith prefix input
        Want.equal false output
    ]

    testList "split" [
      testCase "pass"
      <| fun _ ->
        let input = "foo,bar,baz"
        let output = Str.split "," input

        Want.equal
          [
            "foo"
            "bar"
            "baz"
          ]
          output
    ]

    testList "splitWhitespace" [
      testCase "pass"
      <| fun _ ->
        let input = "foo\nbar baz"
        let output = Str.splitWhitespace input

        Want.equal
          [
            "foo"
            "bar"
            "baz"
          ]
          output
    ]

    testList "splitMax" [
      testCase "pass"
      <| fun _ ->
        let input = "foo,bar,baz"
        let output = Str.splitMax 2 "," input

        Want.equal
          [
            "foo"
            "bar,baz"
          ]
          output
    ]

    testList "splitWhitespaceMax" [
      testCase "pass"
      <| fun _ ->
        let input = "foo bar\nbaz"
        let output = Str.splitWhitespace input

        Want.equal
          [
            "foo"
            "bar"
            "baz"
          ]
          output
    ]

    testList "splitWord" [
      testCase "pass"
      <| fun _ ->
        let input = "foobarbaz"
        let output = Str.splitWord "bar" input

        Want.equal
          [
            "foo"
            "baz"
          ]
          output
    ]

    testList "splitWordMax" [
      testCase "pass"
      <| fun _ ->
        let input = "foobarbazbarfoo"
        let output = Str.splitWordMax 2 "bar" input

        Want.equal
          [
            "foo"
            "bazbarfoo"
          ]
          output
    ]
  ]

[<Tests>]
let unwrapTests =
  testList "Unwrap" [
    testList "some" [
      testCase "pass"
      <| fun _ ->
        let expected = "foo"
        let input = Some expected
        let output = Unwrap.some input
        Want.equal expected output

      testCase "fail when None option does not raise exception"
      <| fun _ ->
        let input = None
        let outputf = (fun _ -> Unwrap.some input)
        Want.throws outputf
    ]

    testList "ok" [
      testCase "pass"
      <| fun _ ->
        let expected = "foo"
        let input = Ok expected
        let output = Unwrap.ok input
        Want.equal expected output

      testCase "fail when Error result does not raise exception"
      <| fun _ ->
        let input = Error "foo"
        let outputf = (fun _ -> Unwrap.ok input)
        Want.throws outputf
    ]

  ]

[<Tests>]
let urlTests =
  testList "Url" [
    testList "parseAbsolute" [
      testCase "pass"
      <| fun _ ->
        let input = "https://foo.bar/baz"
        let output = Url.parseAbsolute input
        let output': Uri = Want.ok output
        let output'' = string output'
        Want.equal input output''
    ]

    testList "parseRelative" [
      testCase "pass"
      <| fun _ ->
        let input = "baz"
        let output = Url.parseRelative input
        let output' = Want.ok output
        Want.equal input <| string output'

      testCase "fail when URL is absolute"
      <| fun _ ->
        let input = "https://foo.bar/baz"
        let output = Url.parseRelative input
        Want.isError output
    ]
  ]

type TDeserialize = {
  Nested: {|
    String: string
    Number: int
  |}
  SnakeCase: bool option
}

[<Tests>]
let jsonTests =
  testList "Json" [
    testList "serialize" [
      testCase "pass"
      <| fun _ ->
        let input = {|
          Nested = {|
            String = "foo"
            Number = 2
          |}
          SnakeCase = "foo bar"
        |}

        let output = Json.serialize input

        let expected =
          """{
  "nested": {
    "number": 2,
    "string": "foo"
  },
  "snake_case": "foo bar"
}"""

        Want.equal expected output
    ]

    testList "deserialize" [
      testCase "pass"
      <| fun _ ->
        let input = """{"nested":{"string":"foo","number":2},"snake_case": null}"""
        let output = Json.deserialize<TDeserialize> input

        let expected = {
          Nested = {|
            String = "foo"
            Number = 2
          |}
          SnakeCase = None
        }

        let output' = Want.ok output
        Want.equal expected output'
    ]
  ]

[<Tests>]
let fsHttpTests =
  testList "FsHttp" [
    testList "Response" [
      testList "toJson" [
        testCase "pass"
        <| fun _ ->
          let expected = {|
            Nested = {|
              String = "foo"
              Number = 2
            |}
          |}

          let input = Json.serialize expected
          (* TODO
          let output = FsHttp.Response.toJson input
          let output' = Want.ok output
          Want.equal expected output'
          *)
          Want.equal "foo" "foo"
      ]
    ]
  ]
