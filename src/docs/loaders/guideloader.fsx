#r "../_lib/Fornax.Core.dll"
#r "../_lib/Markdig.dll"

open Markdig
open Markdig.Extensions.AutoIdentifiers


type GuideConfig = {
    disableLiveRefresh: bool
}
type GuideCategory =
    | Beginner
    | Intermediate
    | Advanced
    | Uncategorized

    static member FromString(value: string) =
        match value with
        | "beginner" -> Beginner
        | "intermediate" -> Intermediate
        | "advanced" -> Advanced
        | _ -> Uncategorized
    
    member this.ToString() =
        match this with
        | Beginner      -> "Beginner"
        | Intermediate  -> "Intermediate"
        | Advanced      -> "Advanced"
        | Uncategorized -> "Uncategorized"

type Guide = {
    file: string
    link : string
    title: string
    author: string option
    tags: string list
    listOrder: int
    guideCategory: GuideCategory
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

    let file = System.IO.Path.Combine("guides", (n |> System.IO.Path.GetFileNameWithoutExtension) + ".md").Replace("\\", "/")
    let link = System.IO.Path.Combine("guides", (n |> System.IO.Path.GetFileNameWithoutExtension) + ".html").Replace("\\", "/")

    let title = config |> List.find (fun n -> n.ToLower().StartsWith "title" ) |> fun n -> n.Split(':').[1] |> trimString
    let listOrder = config |> List.find(fun n -> n.ToLower().StartsWith "list-order") |> fun n -> n.Split(':').[1] |> trimString |> int
    let category = 
        config 
        |> List.find (fun n -> n.ToLower().StartsWith "guide-category") 
        |> fun n -> n.Split(':').[1] 
        |> trimString 
        |> fun n -> n.ToLower() 
        |> GuideCategory.FromString

    let author =
        try
            config |> List.tryFind (fun n -> n.ToLower().StartsWith "author" ) |> Option.map (fun n -> n.Split(':').[1] |> trimString)
        with
        | _ -> None

    let tags =
        try
            let x =
                config
                |> List.tryFind (fun n -> n.ToLower().StartsWith "tags" )
                |> Option.map (fun n -> n.Split(':').[1] |> trimString |> fun n -> n.Split ',' |> Array.toList )
            defaultArg x []
        with
        | _ -> []

    { file = file
      link = link
      title = title
      author = author
      tags = tags
      guideCategory = category
      listOrder = listOrder
      content = content }


let loader (projectRoot: string) (siteContent: SiteContents) =
    let postsPath = System.IO.Path.Combine(projectRoot, "guides")
    System.IO.Directory.GetFiles postsPath
    |> Array.filter (fun n -> n.EndsWith ".md")
    |> Array.map loadFile
    |> Array.iter (fun p -> siteContent.Add p)

    siteContent.Add({ disableLiveRefresh = true })
    siteContent
