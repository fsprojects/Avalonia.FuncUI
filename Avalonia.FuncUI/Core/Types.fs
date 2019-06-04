namespace Avalonia.FuncUI.Core

open System

module rec Types =

    type PropertyAttr =
        {
            Name : string
            Value : obj
        }

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

    type Attr =
        | Property of PropertyAttr
        | Event of EventAttr
        | Content of ContentAttr

    type View =
        {
            ViewType: Type
            Attrs: Attr list
        }  