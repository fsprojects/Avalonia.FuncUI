namespace Avalonia.FuncUI.DSL
open Avalonia.Controls
open Avalonia.Controls.Primitives

[<AutoOpen>]
module StyledElement =  
    open Avalonia
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Styling
    open Avalonia.LogicalTree
    open System.Threading
    open System

    module internal ClassesInternals =
        open System.Linq

        /// pseudoclass is beginning with a ':' character.
        let isPseudoClass (s: string) = s.StartsWith(':')

        /// <summary>
        /// Update `Classes`'s standard classes with new values.
        /// </summary>
        /// 
        /// <remarks>
        /// `Classes` is mixed standard classes and pseudoclasses(beginning with a ':' character). 
        ///
        /// pseudoclasses may only setting by the control's protected <see cref="StyledElement.PseudoClasses"/> property itself.
        /// If set by external, it will be throw exception.
        /// Therefore, when updating from the external, it is necessary to avoid setting the pseudoclasses directly.
        /// </remarks>
        let patchStandardClasses (classes: Classes) (newValues: string seq) =

            let (|PseudoClass|_|) (s: string) =
                if isPseudoClass s then
                    Some PseudoClass
                else
                    None
            
            let newValues = newValues |> Seq.toList

            if List.isEmpty newValues then
                classes.Clear()
            else
                classes
                |> Seq.filter (isPseudoClass >> not)
                |> Seq.except newValues
                |> classes.RemoveAll

                if classes.Count = 0 then
                    classes.AddRange newValues
                else
                    /// Update Classes to minimize event triggers while taking pseudoclasse into account.
                    let rec loop insertIndex newValues =
                        let current = Seq.tryItem insertIndex classes

                        match current, newValues with
                        | _, [] ->
                            // If there are no more values, update finished.
                            ()
                        | None, _ ->
                            // If there are no more classes in the current classes, add the new values.
                            classes.AddRange(newValues)
                        | Some PseudoClass, _ ->
                            // If the current class is a pseudo class, skip it.
                            loop (insertIndex + 1) newValues
                        | Some current, newClass :: rest when current = newClass ->
                            // If the current class is the same as the new class, skip it.
                            loop (insertIndex + 1) rest
                        | Some _, newClass :: rest ->
                            // Search for the new class in the current classes.
                            let oldIndex = classes |> Seq.tryFindIndex ((=) newClass)

                            match oldIndex with
                            
                            | Some oldIndex when oldIndex = insertIndex ->
                                // If oldIndex is the same as insertIndex, do nothing.
                                ()
                            | Some oldIndex ->
                                // If oldIndex is different from insertIndex, move the class to the right position.
                                classes.Move(oldIndex, insertIndex)
                            | None ->
                                // If the class is not in the current classes, insert it.
                                classes.Insert(insertIndex, newClass)

                            // Continue with the next class.
                            loop (insertIndex + 1) rest

                    loop 0 newValues

        /// Compare two sequences of standard classes.
        let compareClasses<'e when 'e :> seq<string> > (a: obj, b: obj) : bool =
            let setup (o: obj) =
                o :?> 'e |> Seq.filter (isPseudoClass >> not)

            let a = setup a
            let b = setup b

            Enumerable.SequenceEqual(a, b)

    type StyledElement with
        static member onAttachedToLogicalTree<'t when 't :> StyledElement>(func:LogicalTreeAttachmentEventArgs -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.AttachedToLogicalTree
            let factory: AvaloniaObject * (LogicalTreeAttachmentEventArgs -> unit) * CancellationToken -> unit =
                (fun (control, func, token) ->
                    let control = control :?> 't
                    let disposable = control.AttachedToLogicalTree.Subscribe(func)

                    token.Register(fun () -> disposable.Dispose()) |> ignore)

            AttrBuilder<'t>.CreateSubscription<LogicalTreeAttachmentEventArgs>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onDetachedFromLogicalTree<'t when 't :> StyledElement>(func:LogicalTreeAttachmentEventArgs -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.DetachedFromLogicalTree
            let factory: AvaloniaObject * (LogicalTreeAttachmentEventArgs -> unit) * CancellationToken -> unit =
                (fun (control, func, token) ->
                    let control = control :?> 't
                    let disposable = control.DetachedFromLogicalTree.Subscribe(func)

                    token.Register(fun () -> disposable.Dispose()) |> ignore)

            AttrBuilder<'t>.CreateSubscription<LogicalTreeAttachmentEventArgs>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onDataContextChanged<'t when 't :> StyledElement>(func: 't -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.DataContextChanged
            let factory: AvaloniaObject * ('t -> unit) * CancellationToken -> unit =
                (fun (control, func, token) ->
                    let control = control :?> 't
                    let hander = EventHandler(fun s e -> func (s :?> 't))
                    let event = control.DataContextChanged
                    
                    event.AddHandler(hander)
                    token.Register(fun () -> event.RemoveHandler(hander)) |> ignore)

            AttrBuilder<'t>.CreateSubscription<'t>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onInitialized<'t when 't :> StyledElement>(func: 't -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.Initialized
            let factory: AvaloniaObject * ('t -> unit) * CancellationToken -> unit =
                (fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler(fun s e -> func (s :?> 't))
                    let event = control.Initialized

                    event.AddHandler(handler)
                    token.Register(fun () -> event.RemoveHandler(handler)) |> ignore)
            
            AttrBuilder<'t>.CreateSubscription<'t>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onResourcesChanged<'t when 't :> StyledElement>(func: 't -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.ResourcesChanged
            let factory: AvaloniaObject * ('t -> unit) * CancellationToken -> unit =
                (fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler<ResourcesChangedEventArgs>(fun s e -> func (s :?> 't))
                    let event = control.ResourcesChanged

                    event.AddHandler(handler)
                    token.Register(fun () -> event.RemoveHandler(handler)) |> ignore)

            AttrBuilder<'t>.CreateSubscription<'t>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onActualThemeVariantChanged<'t when 't :> StyledElement>(func: 't -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.ActualThemeVariantChanged
            let factory: AvaloniaObject * ('t -> unit) * CancellationToken -> unit =
                (fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler(fun s e -> func (s :?> 't))
                    let event = control.ActualThemeVariantChanged

                    event.AddHandler(handler)
                    token.Register(fun () -> event.RemoveHandler(handler)) |> ignore)

            AttrBuilder<'t>.CreateSubscription<'t>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member dataContext<'t when 't :> StyledElement>(dataContext: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(StyledElement.DataContextProperty, dataContext, ValueNone)
            
        static member name<'t when 't :> StyledElement>(name: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(StyledElement.NameProperty, name, ValueNone)
            
        static member templatedParent<'t when 't :> StyledElement>(template: TemplatedControl) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<AvaloniaObject>(StyledElement.TemplatedParentProperty, template, ValueNone)
        
        static member theme<'t when 't :> StyledElement>(theme: ControlTheme) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ControlTheme>(StyledElement.ThemeProperty, theme, ValueNone)

        static member classes<'t when 't :> StyledElement>(value: Classes) : IAttr<'t> =
            let getter: ('t -> Classes) = (fun control -> control.Classes)

            let setter: ('t * Classes -> unit) =
                (fun (control, value) ->
                    ClassesInternals.patchStandardClasses control.Classes value)

            let compare = ClassesInternals.compareClasses<Classes>

            let factory = (fun () -> Classes())

            AttrBuilder<'t>.CreateProperty<Classes>("Classes", value, ValueSome getter, ValueSome setter, ValueSome compare, factory)

        static member classes<'t when 't :> StyledElement>(classes: string list) : IAttr<'t> =
            let getter: ('t -> (string list)) = (fun control -> Seq.toList control.Classes)

            let setter: ('t * string list -> unit) =
                (fun (control, values) -> ClassesInternals.patchStandardClasses control.Classes values)

            let compare = ClassesInternals.compareClasses<string list>

            let factory = fun () -> []

            AttrBuilder<'t>.CreateProperty<string list>("Classes", classes, ValueSome getter, ValueSome setter, ValueSome compare, factory)

        /// Use 'classes' instead when possible.
        static member styles<'t when 't :> StyledElement>(value: Styles) : IAttr<'t> =
            let getter: ('t -> Styles) = (fun control -> control.Styles)

            let setter: ('t * Styles -> unit) =
                (fun (control, value) ->
                    Setters.avaloniaList control.Styles value
                    control.Styles.Resources <- value.Resources)

            let compare: (obj * obj -> bool) =
                fun (a, b) ->
                    match a, b with
                    | (:? Styles as a), (:? Styles as b) ->
                        System.Linq.Enumerable.SequenceEqual(a, b)
                        && System.Linq.Enumerable.SequenceEqual(a.Resources, b.Resources)
                        && System.Linq.Enumerable.SequenceEqual(
                            a.Resources.MergedDictionaries,
                            b.Resources.MergedDictionaries
                        )
                        && System.Linq.Enumerable.SequenceEqual(
                            a.Resources.ThemeDictionaries,
                            b.Resources.ThemeDictionaries
                        )
                    | _ -> a = b

            let factory = fun () -> Styles()

            AttrBuilder<'t>.CreateProperty<Styles>("Styles", value, ValueSome getter, ValueSome setter, ValueSome compare, factory)

        /// Use 'classes' instead when possible.
        static member styles<'t when 't :> StyledElement>(styles: IStyle list) : IAttr<'t> =
            let getter: ('t -> (IStyle list)) = (fun control -> control.Styles |> Seq.toList)

            let setter: ('t * IStyle list -> unit) =
                (fun (control, value) -> Setters.avaloniaList control.Styles value)

            let compare: (obj * obj -> bool) =
                fun (a, b) ->
                    match a, b with
                    | (:? list<IStyle> as a), (:? list<IStyle> as b) -> System.Linq.Enumerable.SequenceEqual(a, b)
                    | _ -> a = b

            let factory = fun () -> []

            AttrBuilder<'t>.CreateProperty<IStyle list>("Styles", styles, ValueSome getter, ValueSome setter, ValueSome compare, factory)


        static member resources<'t when 't :> StyledElement>(value: IResourceDictionary) : IAttr<'t> =
            let getter : ('t -> IResourceDictionary) = (fun control -> control.Resources)
            let setter : ('t * IResourceDictionary -> unit) = (fun (control, value) -> control.Resources <- value)
            let factory = fun () -> ResourceDictionary() :> IResourceDictionary
            
            AttrBuilder<'t>.CreateProperty<IResourceDictionary>("Resources", value, ValueSome getter, ValueSome setter, ValueNone, factory)
            
        // Attached properties related to text input
        
        static member contentType<'t when 't :> StyledElement>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(Avalonia.Input.TextInput.TextInputOptions.ContentTypeProperty, value, ValueNone)
            
        static member returnKeyType<'t when 't :> StyledElement>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(Avalonia.Input.TextInput.TextInputOptions.ReturnKeyTypeProperty, value, ValueNone)
            
        static member multiline<'t when 't :> StyledElement>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(Avalonia.Input.TextInput.TextInputOptions.MultilineProperty, value, ValueNone)
            
        static member autoCapitalization<'t when 't :> StyledElement>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(Avalonia.Input.TextInput.TextInputOptions.AutoCapitalizationProperty, value, ValueNone)
            
        static member isSensitive<'t when 't :> StyledElement>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(Avalonia.Input.TextInput.TextInputOptions.IsSensitiveProperty, value, ValueNone)
            
        static member uppercase<'t when 't :> StyledElement>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(Avalonia.Input.TextInput.TextInputOptions.UppercaseProperty, value, ValueNone)
            
        static member lowercase<'t when 't :> StyledElement>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(Avalonia.Input.TextInput.TextInputOptions.LowercaseProperty, value, ValueNone)
