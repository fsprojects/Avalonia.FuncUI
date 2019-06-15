namespace Avalonia.FuncUI

open System

module rec Types =

    type PropertyAttr =
        {
            Name : string
            Value : obj
        }

    [<CustomEquality; NoComparison>]
    type AttachedPropertyAttr =
        {
            Name : string
            Value : obj
            Handler : obj * (obj option) -> unit
        }
        with 
            override this.GetHashCode() = 
                (this.Name, this.Value).GetHashCode()

            override this.Equals other =
                this.GetHashCode() = other.GetHashCode()

    type EventAttr =
        {
            Name : string
            Value : Delegate
        }

    type ContentAttr =
        {
            Name : string
            Content : ViewContent
        }

    type ViewContent = 
        | Single of View option
        | Multiple of View list

    type Lifecycle = OnCreate | OnUpdate

    [<CustomEquality; NoComparison>]
    type LifecylceAttr =
        {
            Lifecylce : Lifecycle
            Func : obj -> unit
        }
        with 
            override this.GetHashCode() = 
                this.Lifecylce.GetHashCode()

            override this.Equals other =
                this.GetHashCode() = other.GetHashCode()
            
    type Attr =
        | Property of PropertyAttr
        | AttachedProperty of AttachedPropertyAttr
        | Event of EventAttr
        | Content of ContentAttr
        | Lifecycle of LifecylceAttr

    type View =
        {
            ViewType: Type
            Attrs: Attr list
        }  