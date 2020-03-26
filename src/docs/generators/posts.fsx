#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"


open Html


let generate' (ctx : SiteContents) (_: string) =
    let siteInfo = ctx.TryGetValue<Globalloader.SiteInfo> ()
    match siteInfo with
    | Some siteInfo ->
        ctx.Add({ siteInfo with showSideBar = false })
    | None -> ()
    let posts = 
        ctx.TryGetValues<Postloader.Post> () 
        |> Option.defaultValue Seq.empty<Postloader.Post>
        |> Seq.sortByDescending(fun p -> p.published)

    Layout.layout ctx "Blog" [
        article [Class "blog"] [
            header [Class "blog-header"] [
                h1 [Class "title"] [
                    !!"Avalonia.FuncUI Blog"
                ]
            ]
            for post in posts do
                section [Class "box post"] [
                    header [Class "post-header"] [
                        h2 [Class "title"] [
                            a [Href post.link] [!!post.title]
                        ]
                        h3 [Class "subtitle"] [
                            match post.author with 
                            | Some author ->
                                !!("Published by: " + author)
                            | None  -> 
                                !!"Published by: Avalonia Community"
                        ]
                        div [Class "tags"][
                            for tag in post.tags do 
                                span [Class "tag is-link is-light"] [!!tag]
                        ]
                        match post.published with 
                        | Some published ->
                            small [] [
                                !!published.ToLongDateString()
                            ]
                        | None -> !!""
                    ]
                    div [Class "content post-content"] [!!post.content]
                ]
        ]
    ]

let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
    generate' ctx page
    |> Layout.render ctx