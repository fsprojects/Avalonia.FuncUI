#r "_lib/Fornax.Core.dll"

open Config
open System.IO

let postPredicate (projectRoot: string, page: string) =
    let fileName = Path.Combine(projectRoot,page)
    let ext = Path.GetExtension page
    if ext = ".md" then
        let ctn = File.ReadAllText fileName
        ctn.Contains("layout: post")
    else
        false

let controlPredicate (projectRoot: string, page: string) =
    let fileName = Path.Combine(projectRoot,page)
    let ext = Path.GetExtension page
    if ext = ".md" then
        let ctn = File.ReadAllText fileName
        ctn.Contains("layout: control")
    else
        false

let guidePredicate (projectRoot: string, page: string) =
    let fileName = Path.Combine(projectRoot,page)
    let ext = Path.GetExtension page
    if ext = ".md" then
        let ctn = File.ReadAllText fileName
        ctn.Contains("layout: guide")
    else
        false

let releaseNotePredicate (projectRoot: string, page: string) =
    let fileName = Path.Combine(projectRoot,page)
    let ext = Path.GetExtension page
    if ext = ".md" then
        let ctn = File.ReadAllText fileName
        ctn.Contains("layout: release-note")
    else
        false

let staticPredicate (projectRoot: string, page: string) =
    let ext = Path.GetExtension page
    if page.Contains "_public" ||
       page.Contains "_bin" ||
       page.Contains "_lib" ||
       page.Contains "_data" ||
       page.Contains "_settings" ||
       page.Contains "_config.yml" ||
       page.Contains ".sass-cache" ||
       page.Contains ".git" ||
       page.Contains ".ionide" ||
       page.Contains ".fake" ||
       page.Contains ".config" ||
       page.Contains ".fsx.lock" ||
       page.Contains ".cmd" ||
       page.Contains ".sh" ||
       page.Contains ".gitkeep" ||
       page.Contains ".markdown" ||
       ext = ".fsx"
    then
        false
    else
        true

let config = {
    Generators = [
        { Script = "sass.fsx"; Trigger = OnFileExt ".scss"; OutputFile = ChangeExtension "css" }
        { Script = "post.fsx"; Trigger = OnFilePredicate postPredicate; OutputFile = ChangeExtension "html" }
        { Script = "guide.fsx"; Trigger = OnFilePredicate guidePredicate; OutputFile = ChangeExtension "html" }
        { Script = "control.fsx"; Trigger = OnFilePredicate controlPredicate; OutputFile = ChangeExtension "html" }
        { Script = "releasenote.fsx"; Trigger = OnFilePredicate releaseNotePredicate; OutputFile = ChangeExtension "html" }
        { Script = "staticfile.fsx"; Trigger = OnFilePredicate staticPredicate; OutputFile = SameFileName }
        { Script = "index.fsx"; Trigger = Once; OutputFile = NewFileName "index.html" }
        { Script = "about.fsx"; Trigger = Once; OutputFile = NewFileName "about.html" }
        { Script = "posts.fsx"; Trigger = Once; OutputFile = NewFileName "blog.html" }
        { Script = "guides.fsx"; Trigger = Once; OutputFile = NewFileName "guides.html" }
        { Script = "releasenotes.fsx"; Trigger = Once; OutputFile = NewFileName "release-notes.html" }
    ]
}
