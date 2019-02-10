module WpfMath.Tests.ApprovalTestUtils

open System.Reflection

open ApprovalTests
open ApprovalTests.Reporters
open Newtonsoft.Json
open Newtonsoft.Json.Converters
open Newtonsoft.Json.Serialization

open WpfMath

[<assembly: UseReporter(typeof<DiffReporter>)>]
do ()

type private InnerPropertyContractResolver() =
    inherit DefaultContractResolver()
    member private __.DoCreateProperty(p, ms) =
        let prop = base.CreateProperty(p, ms)
        prop.Readable <- true
        prop

    override this.CreateProperties(``type``, memberSerialization) =
        // override that to serialize internal properties, too
        upcast ResizeArray(
            ``type``.GetProperties(BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.Instance)
            |> Seq.filter (fun p -> Array.isEmpty <| p.GetIndexParameters()) // no indexers
            |> Seq.map (fun p -> this.DoCreateProperty(p, memberSerialization))
        )

let private jsonSettings = JsonSerializerSettings(ContractResolver = InnerPropertyContractResolver(),
                                                  Formatting = Formatting.Indented,
                                                  Converters = [| StringEnumConverter() |])

let private serialize o =
    JsonConvert.SerializeObject(o, jsonSettings)

let approvalTestParseResult(formulaText : string) : unit =
    let parser = TexFormulaParser()
    let formula = parser.Parse formulaText
    let result = serialize formula
    Approvals.Verify result