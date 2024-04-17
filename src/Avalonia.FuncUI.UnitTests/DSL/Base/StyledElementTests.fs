namespace Avalonia.FuncUI.UnitTests.DSL

open Avalonia.Controls
open global.Xunit

module StyledElementTests =
    open Avalonia.FuncUI.VirtualDom
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Types

    let twoAttrs<'x, 't> (attr: 'x -> IAttr<'t>) a b =
        [ attr a :> IAttr ], [ attr b :> IAttr ]

    [<Fact>]
    let ``classes equality with string list`` () =
        let valueList() = [ "class1"; "class2" ]

        let classes1 = valueList()
        let classes2 = valueList()

        let stringList =
            (classes1, classes2)
            ||> twoAttrs Control.classes
            |> Differ.diffAttributes

        Assert.Empty stringList

    [<Fact>]
    let ``classes equality with same classes instance`` () =
        let classes = Classes()
        classes.Add "class1"
        classes.Add "class2"

        let sameClassesInstance =
            (classes, classes) ||> twoAttrs Control.classes |> Differ.diffAttributes

        Assert.Empty sameClassesInstance

    [<Fact>]
    let ``classes equality with different classes instance`` () =
        let classes1 = Classes()
        classes1.Add "class1"
        classes1.Add "class2"

        let classes2 = Classes()
        classes2.Add "class1"
        classes2.Add "class2"

        let differentClassesInstance =
            (classes1, classes2) ||> twoAttrs Control.classes |> Differ.diffAttributes

        Assert.Empty differentClassesInstance
