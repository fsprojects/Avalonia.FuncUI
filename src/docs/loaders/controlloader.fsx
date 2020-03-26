#r "../_lib/Fornax.Core.dll"
#r "../_lib/Markdig.dll"

open Markdig
open Markdig.Extensions.AutoIdentifiers


type ControlConfig = {
    disableLiveRefresh: bool
}

type ControlGroup =
    | Controls
    | Primitives
    | Uncategorized

    static member FromString(value: string) =
        match value with
        | "controls" -> Controls
        | "primitives" -> Primitives
        | _ -> Uncategorized
    
    member this.ToString() =
        match this with
        | Controls      -> "Controls"
        | Primitives  -> "Primitives"
        | Uncategorized -> "Uncategorized"

type Control = {
    file: string
    link : string
    name: string
    group: ControlGroup option
    content: string
}

let markdownPipeline =
    MarkdownPipelineBuilder()
        .UseAutoIdentifiers(AutoIdentifierOptions.GitHub)
        .UseEmojiAndSmiley()
        .UsePipeTables()
        .UseGridTables()
        .Build()

let isSeparator (input : string) =
    input.StartsWith "---"

///returns content of config that should be used for the page
let getConfig (fileContent : string) =
    let fileContent = fileContent.Split '\n'
    let fileContent = fileContent |> Array.skip 1 //First line must be ---
    let indexOfSeperator = fileContent |> Array.findIndex isSeparator
    fileContent
    |> Array.splitAt indexOfSeperator
    |> fst
    |> String.concat "\n"

///`fileContent` - content of page to parse. Usually whole content of `.md` file
///returns HTML version of content of the page
let getContent (fileContent : string) =
    let fileContent = fileContent.Split '\n'
    let fileContent = fileContent |> Array.skip 1 //First line must be ---
    let indexOfSeperator = fileContent |> Array.findIndex isSeparator
    let _, content = fileContent |> Array.splitAt indexOfSeperator

    let content = content |> Array.skip 1 |> String.concat "\n"
    Markdown.ToHtml(content, markdownPipeline)

let trimString (str : string) =
    str.Trim().TrimEnd('"').TrimStart('"')


let loadFile n =
    let text = System.IO.File.ReadAllText n

    let config = (getConfig text).Split( '\n') |> List.ofArray

    let content = getContent text

    let file = System.IO.Path.Combine("controls", (n |> System.IO.Path.GetFileNameWithoutExtension) + ".md").Replace("\\", "/")
    let link = System.IO.Path.Combine("controls", (n |> System.IO.Path.GetFileNameWithoutExtension) + ".html").Replace("\\", "/")

    let name = 
        config 
        |> List.find (fun n -> n.ToLower().StartsWith "name" ) 
        |> fun n -> n.Split(':').[1] |> trimString
    let group = 
        try
            config 
            |> List.tryFind(fun n -> n.ToLower().StartsWith "group") 
            |> Option.map
                (fun n -> 
                    n.Split(':').[1] 
                    |> trimString 
                    |> ControlGroup.FromString)
        with
        | _ -> None

    { file = file
      link = link
      name = name
      group = group
      content = content }


let loader (projectRoot: string) (siteContent: SiteContents) =
    let postsPath = System.IO.Path.Combine(projectRoot, "controls")
    System.IO.Directory.GetFiles postsPath
    |> Array.filter (fun n -> n.EndsWith ".md")
    |> Array.map loadFile
    |> Array.iter (fun p -> siteContent.Add p)

    siteContent.Add({ disableLiveRefresh = true })
    siteContent
