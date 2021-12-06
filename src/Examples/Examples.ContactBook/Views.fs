module Examples.ContactBook.Views

open System
open Avalonia.Controls
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Avalonia.FuncUI

let contactListView (contacts: ITap<Contact list>, selectedId: IWire<Guid option>, filter: IWire<string option>) =
    Component.create ("contact list", fun ctx ->
        let contacts = ctx.useTap contacts
        let filter = ctx.useWire filter
        let selectedId = ctx.useWire selectedId

        DockPanel.create [
            DockPanel.width 300.0
            DockPanel.lastChildFill true
            DockPanel.children [
                TextBox.create [
                    TextBox.dock Dock.Top
                    TextBox.watermark "search.."
                    TextBox.text (Option.defaultValue String.Empty filter.CurrentSignal)
                    TextBox.onTextChanged (fun text ->
                        if String.IsNullOrEmpty(text)
                        then filter.Send None
                        else filter.Send (Some text)
                    )
                ]

                ListBox.create [
                    ListBox.dock Dock.Top
                    ListBox.dataItems contacts.CurrentSignal
                    ListBox.itemTemplate (
                        DataTemplateView.create<_, _>(fun data ->
                            TextBlock.create [
                                TextBlock.text data.FullName
                            ]
                        )
                    )
                    ListBox.onSelectedIndexChanged (fun idx ->
                        contacts.CurrentSignal
                        |> List.mapi (fun i v -> i, v)
                        |> Map.ofList
                        |> Map.tryFind idx
                        |> Option.map (fun c -> c.Id)
                        |> selectedId.Send
                    )
                    ListBox.selectedItem (
                        contacts.CurrentSignal
                        |> List.tryFind (fun c -> Some c.Id = selectedId.CurrentSignal)
                        |> Option.map (fun c -> c :> obj)
                        |> Option.defaultValue null
                    )
                ]
            ]
        ]
        :> IView
    )

let contactEditor (contact: IWire<Contact>) =
    Component.create ("contact-editor", fun ctx ->
        let contact = ctx.useWire contact

        StackPanel.create [
            StackPanel.orientation Orientation.Vertical
            StackPanel.spacing 5.0
            StackPanel.children [

                TextBox.create [
                    TextBox.dock Dock.Top
                    TextBox.watermark "Full Name"
                    TextBox.useFloatingWatermark true
                    TextBox.text contact.CurrentSignal.FullName
                    TextBox.onTextChanged (fun text ->
                        contact.Send { contact.CurrentSignal with FullName = text }
                    )
                ]

                TextBox.create [
                    TextBox.dock Dock.Top
                    TextBox.watermark "Mail"
                    TextBox.useFloatingWatermark true
                    TextBox.text contact.CurrentSignal.Mail
                    TextBox.onTextChanged (fun text ->
                        contact.Send { contact.CurrentSignal with Mail = text }
                    )
                ]

                TextBox.create [
                    TextBox.dock Dock.Top
                    TextBox.watermark "Phone"
                    TextBox.useFloatingWatermark true
                    TextBox.text contact.CurrentSignal.Phone
                    TextBox.onTextChanged (fun text ->
                        contact.Send { contact.CurrentSignal with Phone = text }
                    )
                ]

            ]
        ]
        :> IView
    )

let contactView (contact: ITap<Contact>) =
    Component.create ("contact-view", fun ctx ->
        let contact = ctx.useTap contact
        let image = ctx.useAsync Api.randomImage

        StackPanel.create [
            StackPanel.orientation Orientation.Vertical
            StackPanel.spacing 5.0
            StackPanel.children [

                match image.CurrentSignal with
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
                    TextBlock.text contact.CurrentSignal.FullName
                ]

                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.foreground "#3498db"
                    TextBlock.text "E-Mail"
                ]

                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.text contact.CurrentSignal.Mail
                ]

                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.foreground "#3498db"
                    TextBlock.text "Phone"
                ]

                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.text contact.CurrentSignal.Phone
                ]
            ]
        ]
        :> IView
    )

let contactDetailsView (contact: IWire<Contact option>) =
    Component.create ($"details-{contact.CurrentSignal |> Option.map (fun i -> i.Id)}", fun ctx ->
        let contact = ctx.useWire contact
        let editing = ctx.usePort false

        match contact.CurrentSignal with
        | Some contact' when editing.CurrentSignal ->
            Component.create ("edit-selected", fun ctx ->
                let contact' = ctx.usePort contact'

                StackPanel.create [
                    StackPanel.margin 10.0
                    StackPanel.dock Dock.Top
                    StackPanel.width 500.0
                    StackPanel.spacing 10.0
                    StackPanel.children [
                        contactEditor contact'

                        StackPanel.create [
                            StackPanel.orientation Orientation.Horizontal
                            StackPanel.horizontalAlignment HorizontalAlignment.Right
                            StackPanel.spacing 5.0
                            StackPanel.children [
                                Button.create [
                                    Button.dock Dock.Top
                                    Button.content "Save"
                                    Button.onClick (fun _ ->
                                        editing.Send false
                                        contact.Send (Some contact'.CurrentSignal)
                                    )
                                ]

                                Button.create [
                                    Button.dock Dock.Top
                                    Button.content "Cancel"
                                    Button.onClick (fun _ ->
                                        editing.Send false
                                    )
                                ]
                            ]
                        ]
                    ]
                ]
                :> IView
            ) :> IView

        | Some contact' ->
            Component.create ("show-selected", fun ctx ->
                let contact' = ctx.usePort contact'
                let editing = ctx.useWire editing

                StackPanel.create [
                    StackPanel.margin 10.0
                    StackPanel.dock Dock.Top
                    StackPanel.width 500.0
                    StackPanel.spacing 10.0
                    StackPanel.children [
                        contactView contact'

                        StackPanel.create [
                            StackPanel.spacing 5.0
                            StackPanel.horizontalAlignment HorizontalAlignment.Right
                            StackPanel.orientation Orientation.Horizontal
                            StackPanel.children [

                                Button.create [
                                    Button.dock Dock.Bottom
                                    Button.content "edit"
                                    Button.isVisible (not editing.CurrentSignal)
                                    Button.onClick (fun _ ->
                                        editing.Send true
                                    )
                                ]

                                Button.create [
                                    Button.dock Dock.Bottom
                                    Button.content "delete"
                                    Button.isVisible (not editing.CurrentSignal)
                                    Button.background "#c0392b"
                                    Button.onClick (fun _ ->
                                        contact.Send None
                                    )
                                ]
                            ]
                        ]
                    ]
                ]
                :> IView
            ) :> IView
        | None ->
            TextBlock.create [
                DockPanel.dock Dock.Top
                TextBlock.dock Dock.Right
                TextBlock.text "-"
            ] :> IView
    )

let mainView () =
    Component (fun ctx ->
        let contacts = ctx.useWire ContactStore.shared.Contacts
        let selectedId = ctx.usePort (None: Guid option)
        let filter = ctx.usePort None

        let selectedContact =
            contacts
            |> Wire.tryFindBy (fun c -> Some c.Id) selectedId
            |> ctx.useWire

        let filteredContacts =
            contacts
            |> Wire.filter filter (fun contact filter ->
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
                contactListView (filteredContacts, selectedId, filter)
                contactDetailsView selectedContact
            ]
        ]
        :> IView
    )