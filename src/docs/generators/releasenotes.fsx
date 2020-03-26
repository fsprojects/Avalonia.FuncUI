#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"


open Html


let generate' (ctx : SiteContents) (_: string) =
    let releaseNotes = 
        ctx.TryGetValues<Releasenotesloader.ReleaseNote> () 
        |> Option.defaultValue Seq.empty<Releasenotesloader.ReleaseNote>
        |> Seq.sortByDescending(fun p -> p.published)

    Layout.layout ctx "Release Notes" [
        article [Class "release-notes"] [
            header [Class "release-notes-header"] [
                h1 [Class "title"] [
                    !!"Release Notes"
                ]
            ]
            for releaseNote in releaseNotes do
                section [Class "release-note post"] [
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
    ]

let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
    generate' ctx page
    |> Layout.render ctx