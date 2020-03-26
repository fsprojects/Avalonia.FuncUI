#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"

open Html


let generate' (ctx : SiteContents) (page: string) =
    let releaseNote =
        ctx.TryGetValues<Releasenotesloader.ReleaseNote> ()
        |> Option.defaultValue Seq.empty
        |> Seq.find(fun n -> n.file = page)

    Layout.layout ctx releaseNote.title [
        article [Class "release-note box"] [
            header [Class "release-note-header"] [
                h1 [Class "title"] [!!releaseNote.title]
                match releaseNote.codename with 
                    | Some codename -> h4 [Class "subtitle"] [ !!codename ]
                    | None -> !!""
                match releaseNote.version with 
                | Some version -> small [] [!! ("v" + version) ]
                | None -> !!""

                match releaseNote.published with 
                | Some date -> small [] [!! (" Published at:" + date.ToLongDateString()) ]
                | None -> !!""
            ]
            section [Class "release-note-content content"] [!!releaseNote.content]
        ]
    ]

let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
    generate' ctx page
    |> Layout.render ctx