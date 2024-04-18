namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module IDataTemplateHost =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Controls.Templates

    module private Internals =
        let patchDataTemplates(dataTemplates: DataTemplates) (templates: IDataTemplate seq) =

            if Seq.isEmpty templates then
                dataTemplates.Clear()            
            else
                templates
                |> Seq.except dataTemplates
                |> dataTemplates.RemoveAll

                for newIndex, template in Seq.indexed templates do
                    let oldIndex = dataTemplates |> Seq.tryFindIndex template.Equals

                    match oldIndex with
                    | Some oldIndex when oldIndex = newIndex -> ()
                    | Some oldIndex -> dataTemplates.Move(oldIndex, newIndex)
                    | None -> dataTemplates.Insert(newIndex, template)

    type IDataTemplateHost with
        static member dataTemplates<'t when 't :> IDataTemplateHost>(templates: DataTemplates) : IAttr<'t> =
            let getter: 't -> DataTemplates = (fun control -> control.DataTemplates)

            let setter: ('t * DataTemplates -> unit) = (fun (control, value) -> Internals.patchDataTemplates control.DataTemplates value)

            let compare: obj * obj -> bool = (fun (a, b) ->
                let a = a :?> DataTemplates
                let b = b :?> DataTemplates
                System.Linq.Enumerable.SequenceEqual(a, b))

            let factory = fun () -> DataTemplates()

            AttrBuilder<'t>.CreateProperty<DataTemplates>("DataTemplates", templates, ValueSome getter, ValueSome setter, ValueSome compare, factory)

        static member dataTemplates<'t when 't :> IDataTemplateHost>(templates: IDataTemplate list) : IAttr<'t> =
            let getter: 't -> IDataTemplate list = (fun control -> control.DataTemplates |> Seq.toList)

            let setter: ('t * IDataTemplate list -> unit) = (fun (control, value) -> Internals.patchDataTemplates control.DataTemplates value)

            let factory = fun () -> []

            AttrBuilder<'t>.CreateProperty<IDataTemplate list>("DataTemplates", templates, ValueSome getter, ValueSome setter, ValueNone, factory)
