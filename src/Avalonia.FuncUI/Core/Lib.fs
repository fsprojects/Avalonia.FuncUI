namespace Avalonia.FuncUI.Lib

type MutableList<'t> = System.Collections.Generic.List<'t>
type MutableDict<'key, 'value> = System.Collections.Generic.Dictionary<'key, 'value>
type CuncurrentDict<'key, 'value> = System.Collections.Concurrent.ConcurrentDictionary<'key, 'value>

module Reflection =
    open System.Reflection

    let findPropertyByName (obj: 't) (propName: string): PropertyInfo =
        obj
            .GetType()
            .GetProperty(propName)

    let findStaticFieldByName (obj: 't) (propName: string): FieldInfo =
        obj
            .GetType()
            .GetField(propName, BindingFlags.Public ||| BindingFlags.Static |||BindingFlags.FlattenHierarchy)