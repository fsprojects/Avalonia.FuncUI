namespace Avalonia.FuncUI.UnitTests.DSL

open Avalonia
open Avalonia.Controls
open Avalonia.Headless.XUnit
open global.Xunit

module StyledElementTests =
    open Avalonia.Styling
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Types

    module StyledElement =
        // create function here for tests
        let create (attrs: IAttr<StyledElement> list) : IView<StyledElement> =
            ViewBuilder.Create<StyledElement>(attrs)

    let twoAttrs<'x, 't> (attr: 'x -> IAttr<'t>) a b =
        [ attr a :> IAttr ], [ attr b :> IAttr ]

    module ``classes`` =
        open Avalonia.FuncUI.VirtualDom

        [<AvaloniaFact>]
        let ``equality with string list`` () =
            let valueList () = [ "class1"; "class2" ]

            let classes1 = valueList ()
            let classes2 = valueList ()

            let stringList =
                (classes1, classes2) ||> twoAttrs StyledElement.classes |> Differ.diffAttributes

            Assert.Empty stringList

        [<AvaloniaFact>]
        let ``equality with same classes instance`` () =
            let classes = Classes()
            classes.Add "class1"
            classes.Add "class2"

            let sameClassesInstance =
                (classes, classes) ||> twoAttrs StyledElement.classes |> Differ.diffAttributes

            Assert.Empty sameClassesInstance

        [<AvaloniaFact>]
        let ``equality with different classes instance`` () =
            let classes1 = Classes()
            classes1.Add "class1"
            classes1.Add "class2"

            let classes2 = Classes()
            classes2.Add "class1"
            classes2.Add "class2"

            let differentClassesInstance =
                (classes1, classes2) ||> twoAttrs StyledElement.classes |> Differ.diffAttributes

            Assert.Empty differentClassesInstance


    module ``styles`` =
        open Avalonia.FuncUI.VirtualDom

        let initStyle () =
            let s = Style(fun x -> x.Is<Control>())
            s.Setters.Add(Setter(Control.TagProperty, "foo"))
            s :> IStyle

        [<AvaloniaFact>]
        let ``equality with style list has same style instance`` () =
            let style = initStyle ()

            let styleList () = [ style ]

            let styles1 = styleList ()
            let styles2 = styleList ()

            let styleList =
                (styles1, styles2) ||> twoAttrs StyledElement.styles |> Differ.diffAttributes

            Assert.Empty styleList


        [<AvaloniaFact>]
        let ``equality with style list has different style instance`` () =

            let style1 = initStyle ()
            let style2 = initStyle ()

            let styleList =
                ([ style1 ], [ style2 ])
                ||> twoAttrs StyledElement.styles
                |> Differ.diffAttributes

            match Assert.Single styleList with
            | Delta.AttrDelta.Property { Accessor = InstanceProperty { Name = propName }
                                         Value = Some(:? list<IStyle> as [ value ]) } ->
                Assert.Equal("Styles", propName)
                Assert.NotEqual(style1, value)
                Assert.Equal(style2, value)

            | _ -> Assert.Fail $"Not expected delta\n{styleList}"

        [<AvaloniaFact>]
        let ``equality with Styles property has same instance`` () =
            let style = initStyle ()

            let styles = Styles()
            styles.Add style

            let styles1 = styles
            let styles2 = styles

            let styleList =
                (styles1, styles2) ||> twoAttrs StyledElement.styles |> Differ.diffAttributes

            Assert.Empty styleList

        [<AvaloniaFact>]
        let ``equality with Styles property has different Styles instance has same style instance`` () =
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

        [<AvaloniaFact>]
        let ``equality with Styles property has different Styles instance has different style instance`` () =
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

    module ``resources`` =
        let assertResource (target: StyledElement) key : 't =
            let isFound, resValue = target.TryGetResource(key)
            Assert.True isFound
            Assert.IsType<'t>(resValue)

        [<AvaloniaFact>]
        let ``con set empty dictionary`` () =
            let initView =
                StyledElement.create [ StyledElement.resources (ResourceDictionary.create []) ]

            let target = VirtualDom.create initView

            Assert.False target.Resources.HasResources

        [<AvaloniaFact>]
        let ``can set dictionary with values`` () =
            let initView =
                StyledElement.create
                    [ StyledElement.resources (
                          ResourceDictionary.create
                              [ ResourceDictionary.keyValue ("key1", "value1")
                                ResourceDictionary.keyValue ("key2", 42) ]
                      ) ]

            let target = VirtualDom.create initView

            let assertResource key = assertResource target key

            Assert.True target.Resources.HasResources

            Assert.Equal("value1", assertResource "key1")
            Assert.Equal(42, assertResource "key2")

        [<AvaloniaFact>]
        let ``can update dictionary`` () =
            let initView =
                StyledElement.create
                    [ StyledElement.resources (
                          ResourceDictionary.create [ ResourceDictionary.keyValue ("key1", "value1") ]
                      ) ]

            let updatedView =
                StyledElement.create
                    [ StyledElement.resources (ResourceDictionary.create [ ResourceDictionary.keyValue ("key2", 42) ]) ]

            let target = VirtualDom.create initView
            let assertResource key = assertResource target key

            Assert.True target.Resources.HasResources
            Assert.Equal("value1", assertResource "key1")

            VirtualDom.update target initView updatedView

            Assert.Equal(42, assertResource "key2")

    module ``onResourceObservable`` =

        [<AvaloniaFact>]
        let ``observes resource changes with multiple types`` () =
            let mutable observedValue: obj option = Some(box "no value changed.")
            let callback v = observedValue <- v

            let key = "testKey"

            let mutable currentView =
                StyledElement.create [ StyledElement.onResourceObservable (key, callback) ]

            let target = VirtualDom.create currentView

            Assert.Equal(None, observedValue)

            let updateValueAndAssert value =
                let value = box value

                let updatedView =
                    StyledElement.create
                        [ StyledElement.onResourceObservable (key, callback)
                          StyledElement.resources (
                              ResourceDictionary.create [ ResourceDictionary.keyValue (key, value) ]
                          ) ]

                VirtualDom.update target currentView updatedView

                Assert.Equal(Some value, observedValue)

                currentView <- updatedView

            updateValueAndAssert "Hello, World!"
            updateValueAndAssert 12345
            updateValueAndAssert [ 1; 2; 3; 4; 5 ]
