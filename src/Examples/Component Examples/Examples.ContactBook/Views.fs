namespace Examples.ContactBook

open System
open System.Runtime.CompilerServices
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Avalonia.FuncUI
open Avalonia.Media

[<AutoOpen>]
module CustomHooks =

    [<RequireQualifiedAccess>]
    type Deferred<'t> =
        | NotStartedYet
        | Pending
        | Resolved of 't
        | Failed of exn

    type IComponentContext with

        member this.useAsync<'signal>(init: Async<'signal>, [<CallerLineNumber>] ?identity: int) : IWritable<Deferred<'signal>> =
            let identity = Option.get identity
            let state = this.useState (Deferred.NotStartedYet, true, identity)

            this.useEffect (
                handler = (fun _ ->
                    match state.Current with
                    | Deferred.NotStartedYet ->
                        state.Set Deferred.Pending

                        Async.StartImmediate (
                            async {
                                let! result = Async.Catch init

                                match result with
                                | Choice1Of2 value -> state.Set (Deferred.Resolved value)
                                | Choice2Of2 exn -> state.Set (Deferred.Failed exn)
                            }
                        )

                    | _ ->
                        ()
                ),
                triggers = [ EffectTrigger.AfterInit ],
                callerLineNumber = identity
            )

            state

[<RequireQualifiedAccess>]
type EditContactResult =
    | Cancel
    | Update of Contact

[<AbstractClass; Sealed>]
type Views =

    static member contactListView
      ( contacts: IReadable<Contact list>,
        selectedId: IWritable<Guid option>,
        filter: IWritable<string option> ) =

        Component.create ("contact list", fun ctx ->
            let contacts = ctx.usePassedRead contacts
            let filter = ctx.usePassed filter
            let selectedId = ctx.usePassed selectedId

            DockPanel.create [
                DockPanel.width 300.0
                DockPanel.lastChildFill true
                DockPanel.children [
                    TextBox.create [
                        TextBox.dock Dock.Top
                        TextBox.watermark "search.."
                        TextBox.text (Option.defaultValue String.Empty filter.Current)
                        TextBox.onTextChanged (fun text ->
                            if Some text <> filter.Current then
                                if String.IsNullOrEmpty text
                                then filter.Set None
                                else filter.Set (Some text)
                            else
                                ()
                        )
                    ]

                    ListBox.create [
                        ListBox.dock Dock.Top
                        ListBox.dataItems contacts.Current
                        ListBox.itemTemplate (
                            DataTemplateView.create<_, _>(fun data ->
                                TextBlock.create [
                                    TextBlock.text data.FullName
                                ]
                            )
                        )
                        ListBox.onSelectedIndexChanged (fun idx ->
                            contacts.Current
                            |> List.mapi (fun i v -> i, v)
                            |> Map.ofList
                            |> Map.tryFind idx
                            |> Option.map (fun c -> c.Id)
                            |> selectedId.Set
                        )
                        ListBox.selectedItem (
                            contacts.Current
                            |> List.tryFind (fun c -> Some c.Id = selectedId.Current)
                            |> Option.map (fun c -> c :> obj)
                            |> Option.defaultValue null
                        )
                    ]
                ]
            ]
            :> IView
        )

    static member contactEditor (contact: IWritable<Contact>) =
        Component.create ("contact-editor", fun ctx ->
            let contact = ctx.usePassed contact

            StackPanel.create [
                StackPanel.orientation Orientation.Vertical
                StackPanel.spacing 5.0
                StackPanel.children [

                    TextBox.create [
                        TextBox.dock Dock.Top
                        TextBox.watermark "Full Name"
                        TextBox.useFloatingWatermark true
                        TextBox.text contact.Current.FullName
                        TextBox.onTextChanged (fun text ->
                            contact.Set { contact.Current with FullName = text }
                        )
                    ]

                    TextBox.create [
                        TextBox.dock Dock.Top
                        TextBox.watermark "Mail"
                        TextBox.useFloatingWatermark true
                        TextBox.text contact.Current.Mail
                        TextBox.onTextChanged (fun text ->
                            contact.Set { contact.Current with Mail = text }
                        )
                    ]

                    TextBox.create [
                        TextBox.dock Dock.Top
                        TextBox.watermark "Phone"
                        TextBox.useFloatingWatermark true
                        TextBox.text contact.Current.Phone
                        TextBox.onTextChanged (fun text ->
                            contact.Set { contact.Current with Phone = text }
                        )
                    ]

                ]
            ]
            :> IView
        )

    static member contactView (contact: IReadable<Contact>) =
        Component.create ("contact-view", fun ctx ->
            let contact = ctx.usePassedRead contact
            let image = ctx.useAsync Api.randomImage

            StackPanel.create [
                StackPanel.orientation Orientation.Vertical
                StackPanel.spacing 5.0
                StackPanel.children [

                    match image.Current with
                    | Deferred.Resolved bitmap ->
                        Image.create [
                            Image.source bitmap
                            Image.height 200.0
                        ]
                    | Deferred.Failed e ->
                        TextBlock.create [
                            TextBlock.text $"{e.Message}"
                        ]
                    | Deferred.Pending | Deferred.NotStartedYet ->
                        ProgressBar.create [
                            ProgressBar.isEnabled true
                            ProgressBar.isIndeterminate true
                        ]

                    TextBlock.create [
                        TextBlock.dock Dock.Top
                        TextBlock.foreground "#3498db"
                        TextBlock.text "Full Name"
                    ]

                    TextBlock.create [
                        TextBlock.dock Dock.Top
                        TextBlock.text contact.Current.FullName
                    ]

                    TextBlock.create [
                        TextBlock.dock Dock.Top
                        TextBlock.foreground "#3498db"
                        TextBlock.text "E-Mail"
                    ]

                    TextBlock.create [
                        TextBlock.dock Dock.Top
                        TextBlock.text contact.Current.Mail
                    ]

                    TextBlock.create [
                        TextBlock.dock Dock.Top
                        TextBlock.foreground "#3498db"
                        TextBlock.text "Phone"
                    ]

                    TextBlock.create [
                        TextBlock.dock Dock.Top
                        TextBlock.text contact.Current.Phone
                    ]
                ]
            ]
            :> IView
        )

    static member contactDetailsEditView
      ( contact: Contact,
        callback: EditContactResult -> unit ) =

        Component.create ($"edit-selected-contact-{contact.Id}", fun ctx ->
            let contact = ctx.useState contact

            StackPanel.create [
                StackPanel.margin 10.0
                StackPanel.dock Dock.Top
                StackPanel.width 500.0
                StackPanel.spacing 10.0
                StackPanel.children [
                    Views.contactEditor contact

                    StackPanel.create [
                        StackPanel.orientation Orientation.Horizontal
                        StackPanel.horizontalAlignment HorizontalAlignment.Right
                        StackPanel.spacing 5.0
                        StackPanel.children [
                            Button.create [
                                Button.dock Dock.Top
                                Button.content "Save"
                                Button.onClick (fun _ ->
                                    callback (EditContactResult.Update contact.Current)
                                )
                            ]

                            Button.create [
                                Button.dock Dock.Top
                                Button.content "Cancel"
                                Button.onClick (fun _ ->
                                    callback EditContactResult.Cancel
                                )
                            ]
                        ]
                    ]
                ]
            ]
            :> IView
        ) :> IView

    static member contactDetailsReadOnlyView
      ( contact: Contact,
        onEdit: unit -> unit,
        onDelete: unit -> unit ) =

        Component.create ($"show-selected-contact-{contact.Id}", fun ctx ->
            let contact = ctx.useState contact

            StackPanel.create [
                StackPanel.margin 10.0
                StackPanel.dock Dock.Top
                StackPanel.width 500.0
                StackPanel.spacing 10.0
                StackPanel.children [
                    Views.contactView contact

                    StackPanel.create [
                        StackPanel.spacing 5.0
                        StackPanel.horizontalAlignment HorizontalAlignment.Right
                        StackPanel.orientation Orientation.Horizontal
                        StackPanel.children [

                            Button.create [
                                Button.dock Dock.Bottom
                                Button.content "edit"
                                Button.onClick (ignore >> onEdit)
                            ]

                            Button.create [
                                Button.dock Dock.Bottom
                                Button.content "delete"
                                Button.background "#c0392b"
                                Button.onClick (ignore >> onDelete)
                            ]
                        ]
                    ]
                ]
            ]
            :> IView
        ) :> IView

    static member contactDetailsView (contact: IWritable<Contact option>) =
        Component.create ($"details", fun ctx ->
            let contact = ctx.usePassed contact
            let editing = ctx.useState false

            DockPanel.create [
                DockPanel.dock Dock.Top
                DockPanel.lastChildFill true
                DockPanel.children [
                    TextBlock.create [
                        TextBox.dock Dock.Top
                        TextBlock.text $"{contact.Current}"
                    ]

                    match contact.Current with
                    | Some value when editing.Current ->
                        Views.contactDetailsEditView (
                            contact = value,
                            callback = (fun result ->
                                match result with
                                | EditContactResult.Cancel ->
                                    editing.Set false
                                | EditContactResult.Update newValue ->
                                    contact.Set (Some newValue)
                                    editing.Set false
                            )
                        )

                    | Some value ->
                        Views.contactDetailsReadOnlyView (
                            contact = value,
                            onEdit = (fun _ -> editing.Set true),
                            onDelete = (fun _ -> contact.Set None)
                        )
                    | None ->
                        TextBlock.create [
                            DockPanel.dock Dock.Top
                            TextBlock.dock Dock.Right
                            TextBlock.text "-"
                        ] :> IView
                ]
            ] :> IView

        )

    static member mainView () =
        Component (fun ctx ->
            let contacts = ctx.usePassed ContactStore.shared.Contacts
            let selectedId = ctx.useState (None: Guid option)
            let filter = ctx.useState (None: string option)

            let selectedContact =
                contacts
                |> State.tryFindByKey (fun c -> Some c.Id) selectedId
                |> ctx.usePassed

            ctx.useEffect(
                handler = (fun _ ->
                    match Application.Current.ApplicationLifetime with
                    | :? IClassicDesktopStyleApplicationLifetime as lifetime ->
                        match selectedContact.Current with
                        | Some contact ->
                            lifetime.MainWindow.Title <- $"ContactBook - {contact.FullName}"
                        | None ->
                            lifetime.MainWindow.Title <- "ContactBook"
                    | _ ->
                        ()

                ),
                triggers = [
                    EffectTrigger.AfterChange selectedId
                    EffectTrigger.AfterChange selectedContact
                ]
            )

            //ctx.useEffect(
            //    handler = (fun _ ->
            //        printfn "filter changed"
            //    ),
            //    deps = [ filter.Any() ]
            //)
    //
            //ctx.useEffect(
            //    handler = (fun _ ->
            //        printfn "selection changed | filter changed"
            //    ),
            //    deps = [ filter.Any(); selectedId.Any() ]
            //)

            //ctx.useEffect(
            //    handler = (fun _ ->
            //        printfn "⏸ --- called after render"
            //    ),
            //    deps = [ ctx.OnRender.Any() ]
            //)

            let filteredContacts =
                contacts
                |> State.readFilter filter (fun contact filter ->
                    match filter with
                    | Some filter ->
                        contact.FullName.Contains(filter : string) ||
                        contact.Mail.Contains(filter : string) ||
                        contact.Phone.Contains(filter : string)
                    | None ->
                        true
                )

            DockPanel.create [
                DockPanel.dock Dock.Top
                DockPanel.children [
                    Views.contactListView (filteredContacts, selectedId, filter)
                    Views.contactDetailsView selectedContact

                    TextBlock.create [
                        TextBlock.dock Dock.Top
                        TextBlock.foreground Brushes.Orange
                        TextBlock.text $"{selectedId.Current} {selectedContact.Current}"
                    ]
                ]
            ]
            :> IView
        )