namespace Avalonia.FuncUI

open System
open FSharp.Data.Adaptive

module rec Types =

    type PropertyAttr =
        {
            Name : string
            Value : obj
        }

    type AdaptivePropertyAttr =
        {
            Name : string
            Value : aval<obj>
        }

    [<CustomEquality; NoComparison>]
    type AttachedPropertyAttr =
        {
            Name : string
            Value : obj
            Handler : obj * (obj option) -> unit
        }
        override this.GetHashCode() = 
            (this.Name, this.Value).GetHashCode()

        override this.Equals other =
            this.GetHashCode() = other.GetHashCode()

    [<CustomEquality; NoComparison>]
    type EventAttr =
        {
            Name : string
            Value : Delegate
        }
        override this.GetHashCode() = 
            this.Name.GetHashCode()

        override this.Equals other =
            // TODO: find a better way to do this
            false // always set event

    type ContentAttr =
        {
            Name : string
            Content : ViewContent
        }

    type AdaptiveContentAttr =
        {
            Name : string
            Content : AdaptiveViewContent
        }

    type ViewContent = 
        | Single of View option
        | Multiple of View list

    type AdaptiveViewContent = 
        | Single of AdaptiveView option
        | Multiple of alist<AdaptiveView>

    type Lifecycle = OnCreate | OnUpdate

    [<CustomEquality; NoComparison>]
    type LifecycleAttr =
        {
            Lifecylce : Lifecycle
            Func : obj -> unit
        }
        override this.GetHashCode() = 
            this.Lifecylce.GetHashCode()

        override this.Equals other =
            this.GetHashCode() = other.GetHashCode()

    type AdaptiveLifecylceAttr = LifecycleAttr
            
    type Attr =
        | Property of PropertyAttr
        | AttachedProperty of AttachedPropertyAttr
        | Event of EventAttr
        | Content of ContentAttr
        | Lifecycle of LifecycleAttr

    [<RequireQualifiedAccess>]
    type AdaptiveAttr =
        | Property of AdaptivePropertyAttr
        | AttachedProperty of AttachedPropertyAttr
        | Event of EventAttr
        | Content of AdaptiveContentAttr
        | Lifecycle of AdaptiveLifecylceAttr

    type View =
        {
            ViewType: Type
            Attrs: Attr list
        }  

    [<RequireQualifiedAccess>]
    type AdaptiveView =
        {
            ViewType: aval<Type>
            Attrs: alist<Attr>
        }  