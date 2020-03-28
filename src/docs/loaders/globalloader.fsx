#r "../_lib/Fornax.Core.dll"

type SiteInfo = {
    title: string
    baseUrl: string
    description: string
    showSideBar: bool
}

let loader (projectRoot: string) (siteContent: SiteContents) =
    siteContent.Add(
        { title = "Avalonia.FuncUI"
          description = "Avalonia.FuncUI Website"
          showSideBar = true
          baseUrl = "/Avalonia.FuncUI/" 
        })
    siteContent
