namespace Avalonia.FuncUI

open System

module Core =
    
    module rec Domain =
       
        type Accessor =
            | Instance of string
            | Avalonia of Avalonia.AvaloniaProperty
        
        type Property =
            {
                accessor: Accessor
                value: obj
            }
        
        type Content =
            {
                accessor: Accessor
            }   
            
        type IAttr =
            interface end
              
        type IAttr<'viewType> =
            inherit IAttr
        
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
                member this.Attrs = this.attrs |> List.map (fun attr -> attr :> IAttr)
                
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