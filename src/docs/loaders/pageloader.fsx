#r "../_lib/Fornax.Core.dll"

type Page = {
    title: string
    link: string
}

let loader (projectRoot: string) (siteContent: SiteContents) =
    siteContent.Add({ title = "Home"; link = "" })
    siteContent.Add({ title = "Blog"; link = "blog.html" })
    siteContent.Add({ title = "Guides"; link = "guides.html" })
    siteContent.Add({ title = "Release Notes"; link = "release-notes.html" })
    siteContent.Add({ title = "About"; link = "about.html" })

    siteContent
