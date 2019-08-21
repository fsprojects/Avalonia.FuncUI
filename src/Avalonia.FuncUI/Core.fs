namespace Avalonia.FuncUI

open SharpDX.Direct2D1
open System

module Core =
    
    module rec Domain =
       
        /// used to get / set attributes
        type Accessor =
            // maybe replace by something the does not depend on reflection 
            | Instance of string
            | Avalonia of Avalonia.AvaloniaProperty
            
        type PropertyType = Direct | Attached
        
        type Property =
            {
                accessor: Accessor
                propertyType: PropertyType
                value: obj
            }
            
        module Property =
            
            /// create a direct property attr
            let createDirect (accessor: Accessor, value: #obj) =
                {
                    Property.accessor = accessor;
                    Property.propertyType = PropertyType.Direct;
                    Property.value = value
                }
                
            /// create an attached property attr
            let createAttached (accessor: Accessor, value: #obj) =
                {
                    Property.accessor = accessor;
                    Property.propertyType = PropertyType.Direct;
                    Property.value = value
                }
        
        type Content =
            {
                accessor: Accessor
                content: ViewContent
            }
            
        type ViewContent =
            | Single of IView option
            | Multiple of IView list
                     
        module Content =
            
            /// create single content attr
            let createSingle (accessor: Accessor, content: IView option) =
                {
                    Content.accessor = accessor;
                    Content.content = ViewContent.Single content;
                }
                
            /// create single content attr
            let createMultiple (accessor: Accessor, content: IView list) =
                {
                    Content.accessor = accessor;
                    Content.content = ViewContent.Multiple content;
                }
                     
        type IAttr =
            abstract member Property : Property option
            abstract member Content : Content option
            
        type IAttr<'viewType> =
            inherit IAttr
                     
        type Attr<'viewType> =
            | Property of Property
            | Content of Content
            | Event
        
        module Attr =
            
            /// create property attr
            let createProperty<'viewType>(property: Property) =
                Attr<'viewType>.Property property
                
            // create content attr
            let createContent<'viewType>(content: Content) =
                Attr<'viewType>.Content content
     
        type IView =
            abstract member ViewType: Type with get
            abstract member Attrs: IAttr list with get
            
        type IView<'viewType> =
            inherit IView
            abstract member Attrs: IAttr<'viewType> list with get
            
        type View<'viewType> =
            {
                viewType: Type
                attrs: IAttr<'viewType> list
            }
            
            interface IView with
                member this.ViewType =  this.viewType
                member this.Attrs =
                    this.attrs |> List.map (fun attr -> attr :> IAttr)
                
            interface IView<'viewType> with
                member this.Attrs = this.attrs
                
        module View =
            
            /// create a new strongly typed View
            let create<'viewType>(attrs: #IAttr<'viewType> list) : IView<'viewType> =
                {
                    View.viewType = typeof<'viewType>
                    View.attrs = attrs
                }
                :> IView<'viewType>
                
            /// create a new loosely typed View
            let create'<'viewType>(attrs: #IAttr<'viewType> list) : IView =
                {
                    View.viewType = typeof<'viewType>
                    View.attrs = attrs
                }
                :> IView