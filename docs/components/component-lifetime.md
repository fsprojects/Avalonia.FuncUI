# Component lifetime

Components internally use a Virtual DOM to efficiently re-render themselves. As components can be nested it is important to understand how the Virtual DOM identifies a component.&#x20;

## Component Identity - Location

If the location of a component (for example in a list of children) changes it is considered a new component.&#x20;

In the example below we conditionally hide a button. If the button is hidden the location of the random color component below changes. Because the random color component is not considered the same, it is newly created. Therefor the color changes.&#x20;

<div align="center">

<img src="../.gitbook/assets/CleanShot 2022-03-26 at 18.16.03.gif" alt="">

</div>

```fsharp
type Views () =

    static let random = Random()
    
    static let randomColor () =
        String.Format("#{0:X6}", random.Next(0x1000000))
    
    static member randomColorView () =
        Component.create ("randomColorView", fun ctx ->
            let color = ctx.useState (randomColor())
            
            TextBlock.create [
                TextBlock.background color.Current
                TextBlock.text $"Color {color.Current}"
            ]
        )
        
    static member mainView () =
        Component (fun ctx ->
            let isHidden = ctx.useState false

            DockPanel.create [
                DockPanel.lastChildFill true
                DockPanel.children [
                    
                    if not isHidden.Current then
                        Button.create [
                            Button.content "hide button"
                            Button.onClick (fun _ -> isHidden.Set true)
                        ]
                    
                    Views.randomColorView ()
                ]
            ]
        )
```

Instead of removing the `Button` from the Virtual DOM we can also just set `isVisible` accordingly. This does not change the location of the `randomColorView` and will keep it's identity.

```fsharp
..    

    static member mainView () =
        Component (fun ctx ->
            let isHidden = ctx.useState false

            DockPanel.create [
                DockPanel.lastChildFill true
                DockPanel.children [
                    
                    Button.create [
                        Button.content "hide button"
                        Button.onClick (fun _ -> isHidden.Set true)
                        Button.isVisible (not isHidden.Current)
                    ]
                    
                    Views.randomColorView ()
                ]
            ]
        )
```

![](<../.gitbook/assets/CleanShot 2022-03-26 at 18.27.40.gif>)

## Component Identity - Key

A Components identity can be explicitly changed by changing it's key. This is useful when the location is stable, but you still want to get a new component in some cases.&#x20;

```fsharp
type Views () =
    static member editorView 
      ( person: Person, 
        onSave: Person -> unit, 
        onDelete: unit -> unit ) =
        
        Component.create ($"edit-person-{person.Id}", fun ctx ->
            let person = ctx.useState person
            
            StackPanel.create [
                StackPanel.orientation Orientation.Vertical
                StackPanel.spacing 5
                StackPanel.children [
                    TextBox.create [
                        TextBox.watermark "First Name"
                        TextBox.text person.Current.FirstName
                        TextBox.onTextChanged (fun value ->
                             person.Set { person.Current with FirstName = value }
                        )
                    ]
                    
                    TextBox.create [
                        TextBox.watermark "Last Name"
                        TextBox.text person.Current.LastName
                        TextBox.onTextChanged (fun value ->
                             person.Set { person.Current with LastName = value }
                        )
                    ]
                    
                    StackPanel.create [
                        StackPanel.orientation Orientation.Horizontal
                        StackPanel.spacing 5
                        StackPanel.children [
                            Button.create [
                                Button.content "save"
                                Button.onClick (fun _ -> onSave person.Current)
                            ]
                            
                            Button.create [
                                Button.content "delete"
                                Button.onClick (fun _ -> onDelete ())
                            ]                            
                        ]
                    ]
                ]
            ]
        )
         
    static member mainView () =
        Component (fun ctx ->
            let people = ctx.usePassed State.people
            let selectedId = ctx.useState (None: Guid option)
            let selectedPerson =
                people
                |> State.tryFindByKey (fun person -> Some person.Id) selectedId
            
            DockPanel.create [
                DockPanel.lastChildFill true
                DockPanel.children [
                    
                    ListBox.create [
                        ListBox.dataItems people.Current
                        ListBox.itemTemplate (
                            DataTemplateView<Person>.create(fun person -> 
                                TextBlock.create [
                                    TextBlock.text 
                                        $"{person.FirstName} {person.LastName}"
                                ]
                            )
                        )
                        ListBox.onSelectedItemChanged (fun item ->
                            match item with
                            | null -> selectedId.Set (None: Guid option)
                            | item -> selectedId.Set (Some (item :?> Person).Id) 
                        )
                    ]
                    
                    ContentControl.create [
                        ContentControl.dock Dock.Right
                        ContentControl.padding 5
                        ContentControl.content (
                            match selectedPerson.Current with
                            | Some person ->
                                Views.editorView (
                                    person = person,
                                    onSave = (Some >> selectedPerson.Set),
                                    onDelete = (fun () -> selectedPerson.Set None)
                                ) :> IView
                            | None ->
                                TextBlock.create [
                                    TextBlock.padding (Thickness 5)
                                    TextBlock.text "No person selected"
                                ] :> IView
                        )
                    ]
                ]
            ]
        )
```

![](<../.gitbook/assets/CleanShot 2022-03-26 at 22.26.20.gif>)

If the key of \`personEditorView\` would not change (see example below) the component would never get re-created.

```fsharp
// BUG - component is never re-created
Component.create ("edit-person", fun ctx -> ..)
// WORKS - component is re-created when person changes
Component.create ($"edit-person-{person.Id}", fun ctx -> ..)
```

This is how it looks like if the component key does not change.&#x20;

![](<../.gitbook/assets/CleanShot 2022-03-26 at 22.31.45.gif>)
