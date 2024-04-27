namespace Avalonia.FuncUI.UnitTests.DSL

open Avalonia
open Avalonia.Controls
open global.Xunit

module StyledElementTests =
    open Avalonia.FuncUI.VirtualDom
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Types
    open Avalonia.Styling

    let twoAttrs<'x, 't> (attr: 'x -> IAttr<'t>) a b =
        [ attr a :> IAttr ], [ attr b :> IAttr ]

    [<Fact>]
    let ``classes equality with string list`` () =
        let valueList() = [ "class1"; "class2" ]

        let classes1 = valueList()
        let classes2 = valueList()

        let stringList =
            (classes1, classes2)
            ||> twoAttrs StyledElement.classes
            |> Differ.diffAttributes

        Assert.Empty stringList

    [<Fact>]
    let ``classes equality with same classes instance`` () =
        let classes = Classes()
        classes.Add "class1"
        classes.Add "class2"

        let sameClassesInstance =
            (classes, classes) ||> twoAttrs StyledElement.classes |> Differ.diffAttributes

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
            (classes1, classes2) ||> twoAttrs StyledElement.classes |> Differ.diffAttributes

        Assert.Empty differentClassesInstance

    let initStyle () =
        let s = Style(fun x -> x.Is<Control>())
        s.Setters.Add(Setter(Control.TagProperty, "foo"))
        s :> IStyle

    [<Fact>]
    let ``styles equality with style list has same style instance`` () =
        let style = initStyle ()

        let styleList () = [ style ]

        let styles1 = styleList ()
        let styles2 = styleList ()

        let styleList =
            (styles1, styles2) ||> twoAttrs StyledElement.styles |> Differ.diffAttributes

        Assert.Empty styleList


    [<Fact>]
    let ``styles equality with style list has different style instance`` () =

        let style1 = initStyle ()
        let style2 = initStyle ()

        let styleList =
            ([ style1 ], [ style2 ]) ||> twoAttrs StyledElement.styles |> Differ.diffAttributes

        match Assert.Single styleList with
        | Delta.AttrDelta.Property { Accessor = InstanceProperty { Name = propName }
                                     Value = Some(:? list<IStyle> as [ value ]) } ->
            Assert.Equal("Styles", propName)
            Assert.NotEqual(style1, value)
            Assert.Equal(style2, value)

        | _ -> Assert.Fail $"Not expected delta\n{styleList}"

    [<Fact>]
    let ``styles equality with Styles property has same instance`` () =
        let style = initStyle ()

        let styles = Styles()
        styles.Add style

        let styles1 = styles
        let styles2 = styles

        let styleList =
            (styles1, styles2) ||> twoAttrs StyledElement.styles |> Differ.diffAttributes

        Assert.Empty styleList

    [<Fact>]
    let ``styles equality with Styles property has different Styles instance has same style instance`` () =
        let style = initStyle ()

        let styles1 = Styles()
        styles1.Add style
        styles1.Resources.Add("key", "value")

        let styles2 = Styles()
        styles2.Add style
        styles2.Resources.Add("key", "value")

        let styleList =
            (styles1, styles2) ||> twoAttrs StyledElement.styles |> Differ.diffAttributes

        Assert.Empty styleList

    [<Fact>]
    let ``styles equality with Styles property has different Styles instance has different style instance`` () =
        let style1 = initStyle ()
        let style2 = initStyle ()

        let styles1 = Styles()
        styles1.Add style1
        styles1.Resources.Add("key", "value")

        let styles2 = Styles()
        styles2.Add style2
        styles2.Resources.Add("key", "value")

        let styleList =
            (styles1, styles2) ||> twoAttrs StyledElement.styles |> Differ.diffAttributes

        match Assert.Single styleList with
        | Delta.AttrDelta.Property { Accessor = InstanceProperty { Name = propName }
                                     Value = Some(:? Styles as value) } ->
            Assert.Equal("Styles", propName)
            Assert.NotEqual<Styles>(styles1, value)
            Assert.Equal<Styles>(styles2, value)

        | _ -> Assert.Fail $"Not expected delta\n{styleList}"
