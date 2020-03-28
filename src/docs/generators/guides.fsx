#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"

open Html


let generate' (ctx : SiteContents) (_: string) =
    let siteInfo = ctx.TryGetValue<Globalloader.SiteInfo> ()
    match siteInfo with
    | Some siteInfo ->
        ctx.Add({ siteInfo with showSideBar = false })
    | None -> ()
    let groups = 
        ctx.TryGetValues<Guideloader.Guide> () 
        |> Option.defaultValue Seq.empty<Guideloader.Guide>
        |> Seq.sortBy(fun g -> g.listOrder)
        |> Seq.groupBy(fun g -> g.guideCategory)

    Layout.layout ctx "Guides" [
        article [Class "guides box"] [
            header [Class "guides-header"] [
                h1 [Class "title"] [
                    !!"Avalonia.FuncUI Guides"
                ]
            ]
            section [] [
                for (category, guides) in groups do
                    h2 [] [!!(category.ToString() + " guides")]
                    ul [] [
                        for guide in guides do
                            li [] [
                                a [Href guide.link] [!!guide.title]
                            ]
                    ]
            ]
        ]
    ]

let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
    generate' ctx page
    |> Layout.render ctx