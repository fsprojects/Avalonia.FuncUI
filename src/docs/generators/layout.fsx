#r "../_lib/Fornax.Core.dll"
#if !FORNAX
#load "../loaders/postloader.fsx"
#load "../loaders/guideloader.fsx"
#load "../loaders/releasenotesloader.fsx"
#load "../loaders/controlloader.fsx"
#load "../loaders/pageloader.fsx"
#load "../loaders/globalloader.fsx"
#endif

open Html

let injectWebsocketCode (webpage:string) =
    let websocketScript =
        """
        <script type="text/javascript">
          var wsUri = "ws://localhost:8080/websocket";
      function init()
      {
        websocket = new WebSocket(wsUri);
        websocket.onclose = function(evt) { onClose(evt) };
      }
      function onClose(evt)
      {
        console.log('closing');
        websocket.close();
        document.location.reload();
      }
      window.addEventListener("load", init, false);
      </script>
        """
    let head = "<head>"
    let index = webpage.IndexOf head
    webpage.Insert ( (index + head.Length + 1),websocketScript)

let layout (ctx : SiteContents) active bodyCnt =
    let pages = 
        ctx.TryGetValues<Pageloader.Page> () 
        |> Option.defaultValue Seq.empty

    let releaseNotes = 
        ctx.TryGetValues<Releasenotesloader.ReleaseNote> () 
        |> Option.defaultValue Seq.empty

    let controls = 
        ctx.TryGetValues<Controlloader.Control> () 
        |> Option.defaultValue Seq.empty
        |> Seq.sortBy(fun c -> c.name)
        |> Seq.groupBy(fun c -> c.group)

    let guides = 
        ctx.TryGetValues<Guideloader.Guide> () 
        |> Option.defaultValue Seq.empty<Guideloader.Guide>
        |> Seq.sortBy(fun g -> g.listOrder)

    let siteInfo = 
        ctx.TryGetValue<Globalloader.SiteInfo> ()

    let ttl =
      siteInfo
      |> Option.map (fun si -> si.title)
      |> Option.defaultValue ""

    let baseurl = 
        siteInfo
        |> Option.map (fun si -> si.baseUrl)
        |> Option.defaultValue "/"

    let showSidebar = 
        siteInfo
        |> Option.map (fun si -> si.showSideBar)
        |> Option.defaultValue true

    let menuEntries =
      pages
      |> Seq.map (fun p ->
        let cls = if p.title = active then "navbar-item is-active is-light" else "navbar-item is-light"
        a [Class cls; Href p.link] [!! p.title ])
      |> Seq.toList

    let groupMenu (name: string) (controls: Controlloader.Control seq) = 
        [ a [] [!!name]
          ul [] [
              for control in controls do 
                  li [] [
                      a [Href control.link] [!!control.name]
                  ]
          ]
        ]

    let menuContent =
        [
            p [Class "menu-label"] [
                !!"Guides"
            ]
            ul [Class "menu-list"] [
                for guide in guides do
                    li [] [
                        a [Href guide.link] [
                            !!guide.title
                        ]
                    ]
            ]
            p [Class "menu-label"] [
                !!"Documentation"
            ]
            ul [Class "menu-list"] [
                for (group, controls) in controls do 
                    li [] [
                        match group with 
                        | None -> yield! groupMenu "" controls
                        | Some group -> yield! groupMenu (group.ToString()) controls
                    ]
            ]
            p [Class "menu-label"] [
                !!"Release Notes"
            ]
            ul [Class "menu-list"] [
                for releaseNote in releaseNotes do
                    li [] [
                        a [Href releaseNote.link] [
                            !!releaseNote.title
                        ]
                    ]
            ]
        ]

    html [Class "has-navbar-fixed-top"] [
        head [] [
            meta [CharSet "utf-8"]
            meta [Name "viewport"; Content "width=device-width, initial-scale=1"]
            title [] [!! ttl]
            link [Rel "icon"; Type "image/png"; Sizes "32x32"; Href (baseurl + "images/favicon.png")]
            link [Rel "stylesheet"; Href "https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css"]
            link [Rel "stylesheet"; Href "https://fonts.googleapis.com/css?family=Open+Sans"]
            link [Rel "stylesheet"; Href "https://unpkg.com/bulma@0.8.0/css/bulma.min.css"]
            link [Rel "stylesheet"; Href "https://cdnjs.cloudflare.com/ajax/libs/prism/1.19.0/themes/prism-coy.min.css"]
            link [Rel "stylesheet"; Type "text/css"; Href (baseurl + "style/style.css")]
            ``base`` [Href baseurl]
        ]
        body [] [
            nav [Class "navbar is-funcui is-fixed-top"] [
                div [Class "container"] [
                    div [Class "navbar-brand"] [
                        a [Class "navbar-item brand-link"; Href "/"] [
                            img [Src "https://raw.githubusercontent.com/AvaloniaCommunity/Avalonia.FuncUI/master/github/img/logo/FuncUI.png"; Alt "Logo"]
                        ]
                        span [Class "navbar-burger burger"; Custom ("data-target", "navbarMenu")] [
                            span [] []
                            span [] []
                            span [] []
                        ]
                    ]
                    div [Id "navbarMenu"; Class "navbar-menu"] menuEntries
                ]
            ]
            main [Class "main-content"] [
                if showSidebar then
                    aside [Class "menu main-menu"] menuContent
                div [Class "page-content"] [yield! bodyCnt]
            ]
            footer [Class "main-footer footer"] [
                    aside [Class "menu"] menuContent
                    section [Class "content has-text-centered"] [
                        p [] [
                            !!"Avalonia.FuncUI"
                        ]
                    ]
            ]
            script [Src (baseurl + "js/index.js")] []
            script [Src "https://cdnjs.cloudflare.com/ajax/libs/prism/1.19.0/prism.min.js"] []
            script [Src "https://cdnjs.cloudflare.com/ajax/libs/prism/1.19.0/plugins/autoloader/prism-autoloader.min.js"] []
        ]
    ]

let render (ctx : SiteContents) cnt =
  let disableLiveRefresh = ctx.TryGetValue<Postloader.PostConfig> () |> Option.map (fun n -> n.disableLiveRefresh) |> Option.defaultValue false
  cnt
  |> HtmlElement.ToString
  |> fun n -> if disableLiveRefresh then n else injectWebsocketCode n