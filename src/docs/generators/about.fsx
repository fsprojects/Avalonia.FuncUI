#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"

open Html

type Link = { text: string; url: string }

let generate' (ctx : SiteContents) (_: string) =
  let left = [
      { text = "Avalonia Repository";  url = "https://github.com/AvaloniaUI/Avalonia" }
      { text = "Awesome Avalonia";  url = "https://github.com/AvaloniaCommunity/awesome-avalonia" }
      { text = "Avalonia Gitter";  url = "https://gitter.im/AvaloniaUI" }
      { text = "Avalonia Community";  url = "https://github.com/AvaloniaCommunity" }
  ]
  let right = [
      { text = "Avalonia.FuncUI Repository";  url = "https://github.com/AvaloniaCommunity/Avalonia.FuncUI" }
      { text = "Avalonia.FuncUI Gitter";  url = "https://gitter.im/Avalonia-FuncUI" }
      { text = "Avalonia.FuncUI .Net Templates";  url = "https://github.com/AvaloniaCommunity/Avalonia.FuncUI.ProjectTemplates" }
      { text = "Avalonia.FuncUI Examples";  url = "https://github.com/AvaloniaCommunity/Avalonia.FuncUI/tree/master/src/Examples" }
  ]
  
  let linkList links =
    ul [] [
        for link in links do
            li [Class "link-list-item"] [
                a [Href link.url] [!!link.text]
            ]
    ]

  Layout.layout ctx "About" [
    section [Class "about"] [
        header [Class "about-header"] [
            h1 [Class "title"] [ !!"About Avalonia.FuncUI" ]
            p [] [
                !!"Don't hesitate to visit the following list of resources"
            ]
        ]
        div [Class "left"] [
            linkList left
        ]
        div [Class "right"] [
            linkList right
        ]
    ]
  ]

let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
  generate' ctx page
  |> Layout.render ctx