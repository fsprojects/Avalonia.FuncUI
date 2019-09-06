namespace Avalonia.FuncUI.VirtualDom

open System.Reflection

open Avalonia.Controls

open System
open Avalonia
open Avalonia.FuncUI.VirtualDom.Delta
open Avalonia.FuncUI.Core.Domain

module internal rec Patcher =
    
    let private patchProperty (view: IControl) (attr: PropertyDelta) : unit =
        match attr.accessor with
        | Accessor.Avalonia avaloniaProperty ->
            match attr.value with
            | Some value -> view.SetValue(avaloniaProperty, value);
            | None ->
                // TODO: create PR - include 'ClearValue' in interface 'IAvaloniaObject'
                (view :?> AvaloniaObject).ClearValue(avaloniaProperty);
                
        | Accessor.Instance propertyName ->
            let propertyInfo = view.GetType().GetProperty(propertyName);
            
            match attr.value with
            | Some value -> propertyInfo.SetValue(view, value)
            | None ->
                let defaultValue =
                    if propertyInfo.PropertyType.IsValueType
                    then Activator.CreateInstance(propertyInfo.PropertyType)
                    else null
                                
                propertyInfo.SetValue(view, defaultValue)
                
    let private patchContent (view: IControl) (attr: ContentDelta) : unit =
        ()
    
    let patch (view: IControl) (viewElement: ViewDelta) : unit =
        for attr in viewElement.attrs do
            match attr with
            | AttrDelta.Property property -> patchProperty view property
            | AttrDelta.Content content -> patchContent view content
