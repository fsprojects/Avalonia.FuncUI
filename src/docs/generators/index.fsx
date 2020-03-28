
#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"

open Markdig
open Markdig.Extensions.AutoIdentifiers
open Html


let markdownPipeline =
    MarkdownPipelineBuilder()
        .UseAutoIdentifiers(AutoIdentifierOptions.GitHub)
        .UseEmojiAndSmiley()
        .UsePipeTables()
        .UseGridTables()
        .Build()

let generate' (ctx : SiteContents) (page: string) =
  Layout.layout ctx "Home" [
    section [Class "index-content content"] [!!page]
  ]

let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
    let file = System.IO.Path.Combine(projectRoot, "index.markdown")
    let text = System.IO.File.ReadAllText file
    let content = Markdown.ToHtml(text, markdownPipeline)
    generate' ctx content
    |> Layout.render ctx