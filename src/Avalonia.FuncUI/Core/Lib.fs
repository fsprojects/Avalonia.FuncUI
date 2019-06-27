namespace Avalonia.FuncUI.Lib

open System.Reflection
open System.Collections.Generic

module Reflection =
    open System.Reflection

    let findPropertyByName (obj: 't) (propName: string): PropertyInfo =
        obj
            .GetType()
            .GetProperty(propName)