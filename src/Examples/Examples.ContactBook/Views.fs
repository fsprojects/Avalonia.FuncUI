module Examples.ContactBook.Views

open System
open Avalonia.Controls
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Avalonia.FuncUI

let contactListView (contacts: IReadable<Contact list>, selectedId: IWritable<Guid option>, filter: IWritable<string option>) =
    Component.create ("contact list", fun ctx ->
        let contacts = ctx.usePassedReadOnly contacts
        let filter = ctx.usePassedValue filter
        let selectedId = ctx.usePassedValue selectedId

        DockPanel.create [
            DockPanel.width 300.0
            DockPanel.lastChildFill true
            DockPanel.children [
                TextBox.create [
                    TextBox.dock Dock.Top
                    TextBox.watermark "search.."
                    TextBox.text (Option.defaultValue String.Empty filter.Current)
                    TextBox.onTextChanged (fun text ->
                        if String.IsNullOrEmpty(text)
                        then filter.Set None
                        else filter.Set (Some text)
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

let contactEditor (contact: IWritable<Contact>) =
    Component.create ("contact-editor", fun ctx ->
        let contact = ctx.usePassedValue contact

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

let contactView (contact: IReadable<Contact>) =
    Component.create ("contact-view", fun ctx ->
        let contact = ctx.usePassedReadOnly contact
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

let contactDetailsView (contact: IWritable<Contact option>) =
    Component.create ($"details-{contact.Current |> Option.map (fun i -> i.Id)}", fun ctx ->
        let contact = ctx.usePassedValue contact
        let editing = ctx.useValue false

        match contact.Current with
        | Some contact' when editing.Current ->
            Component.create ("edit-selected", fun ctx ->
                let contact' = ctx.useValue contact'

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
                                        editing.Set false
                                        contact.Set (Some contact'.Current)
                                    )
                                ]

                                Button.create [
                                    Button.dock Dock.Top
                                    Button.content "Cancel"
                                    Button.onClick (fun _ ->
                                        editing.Set false
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
                let contact' = ctx.useValue contact'
                let editing = ctx.usePassedValue editing

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
                                    Button.isVisible (not editing.Current)
                                    Button.onClick (fun _ ->
                                        editing.Set true
                                    )
                                ]

                                Button.create [
                                    Button.dock Dock.Bottom
                                    Button.content "delete"
                                    Button.isVisible (not editing.Current)
                                    Button.background "#c0392b"
                                    Button.onClick (fun _ ->
                                        contact.Set None
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
        let contacts = ctx.usePassedValue ContactStore.shared.Contacts
        let selectedId = ctx.useValue (None: Guid option)
        let filter = ctx.useValue None

        let selectedContact =
            contacts
            |> Value.tryFindByKey (fun c -> Some c.Id) selectedId
            |> ctx.usePassedValue

        let filteredContacts =
            contacts
            |> Value.filter filter (fun contact filter ->
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