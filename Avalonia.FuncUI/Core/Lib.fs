namespace Avalonia.FuncUI.Core.Lib

module Reflection =
    open System.Reflection

    let findPropertyByName (obj: 't) (propName: string): PropertyInfo =
        obj
            .GetType()
            .GetProperty(propName)