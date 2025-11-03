namespace Avalonia.FuncUI.UnitTests.DSL

open Avalonia
open Avalonia.Controls
open global.Xunit

module ResourceDictionaryTests =
    open Avalonia.FuncUI.VirtualDom
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.VirtualDom

    module private VirtualDom =
        open Avalonia.FuncUI.Builder

        let create<'t when 't :> AvaloniaObject> (view: IView<'t>) : 't = VirtualDom.createObject view :?> 't

        let update<'t when 't :> AvaloniaObject> (target: 't) (last: IView<'t>) (next: IView<'t>) : unit =
            let diff = Differ.diff (last, next)
            Patcher.patch (target, diff)

    module private Assert =
        let containsKey (dict: ResourceDictionary) (key: obj) =
            let isFound, value = dict.TryGetResource(key, null)
            Assert.True(isFound, $"ResourceDictionary does not contain expected key: {key}.")
            value

        let notContainsKey (dict: ResourceDictionary) (key: obj) =
            let isFound, value = dict.TryGetResource(key, null)
            Assert.False(isFound, $"ResourceDictionary unexpectedly contains key: {key}. Found value: {value}")

        let containsKeyWithValueWith (dict: ResourceDictionary) (key: 'k) (expectedValue: 'v) (converter: obj -> 'v) =
            let isFound, actualValue = dict.TryGetResource(key, null)
            let expectedValue = box expectedValue

            Assert.True(isFound, $"ResourceDictionary does not contain expected key: {key}.")
            Assert.Equal(expectedValue, converter actualValue)

        let containsKeyWithValue (dict: ResourceDictionary) (key: 'k) (expectedValue: 'v) =
            containsKeyWithValueWith dict key expectedValue (fun o -> o :?> 'v)

    module ``keyValue`` =


        [<Fact>]
        let ``can set`` () =
            let initView = ResourceDictionary.create []
            let target = VirtualDom.create initView

            let key = "myKey"
            let value = "myValue"

            Assert.notContainsKey target key

            let updatedView =
                ResourceDictionary.create [ ResourceDictionary.keyValue (key, value) ]

            VirtualDom.update target initView updatedView

            Assert.containsKeyWithValue target key value

            let revertedView = ResourceDictionary.create []
            VirtualDom.update target updatedView revertedView

            Assert.notContainsKey target key

        [<Fact>]
        let ``can set null value`` =
            let initView = ResourceDictionary.create []
            let target = VirtualDom.create initView

            let key = "myKey"
            let value = null
            Assert.notContainsKey target key

            let updatedView =
                ResourceDictionary.create [ ResourceDictionary.keyValue (key, value = value) ]

            VirtualDom.update target initView updatedView
            Assert.containsKeyWithValue target key value

            VirtualDom.update target updatedView initView
            Assert.notContainsKey target key


        [<Fact>]
        let ``can update value`` () =
            let key = "myKey"

            let createView (value: obj) =
                ResourceDictionary.create [ ResourceDictionary.keyValue (key, value = value) ]

            let createViewWithIView value =
                ResourceDictionary.create [ ResourceDictionary.keyValue (key, view = value) ]

            let initialValue = "initialValue"
            let initView = createView initialValue
            let target = VirtualDom.create initView

            Assert.containsKeyWithValue target key initialValue

            List.fold
                (fun lastView acc ->
                    match acc with
                    | Choice1Of2(nextValue: obj) ->
                        let updatedView = createView nextValue
                        VirtualDom.update target lastView updatedView
                        Assert.containsKeyWithValue target key nextValue
                        updatedView
                    | Choice2Of2(nextValue, expected, converter) ->
                        let updatedView = createViewWithIView nextValue
                        VirtualDom.update target lastView updatedView
                        Assert.containsKeyWithValueWith target key expected converter
                        updatedView)
                initView
                [ Choice1Of2 42
                  Choice1Of2 "a string"
                  Choice1Of2 null
                  Choice1Of2 3.14
                  Choice2Of2(TextBlock.create [ TextBlock.text "Hello" ], "Hello", fun o -> (o :?> TextBlock).Text)
                  Choice1Of2 true
                  Choice1Of2(TextBox(Text = "Hello, World!"))
                  Choice1Of2(ContentControl(Content = Button(Content = "Click Me"))) ]


        [<Fact>]
        let ``can update key`` () =
            let initialKey = "initialKey"
            let updatedKey = "updatedKey"
            let value = "myValue"

            let initView =
                ResourceDictionary.create [ ResourceDictionary.keyValue (initialKey, value) ]

            let target = VirtualDom.create initView

            Assert.containsKeyWithValue target initialKey value

            let updatedView =
                ResourceDictionary.create [ ResourceDictionary.keyValue (updatedKey, value) ]

            VirtualDom.update target initView updatedView

            Assert.notContainsKey target initialKey
            Assert.containsKeyWithValue target updatedKey value


        [<Fact>]
        let ``can set multiple`` () =
            let key1 = "key1"
            let value1 = "value1"
            let updatedValue1 = "updatedValue1"
            let key2 = typeof<Button>
            let value2 = Styling.ControlTheme(key2)
            let updatedValue2 = Styling.ControlTheme(key2)

            let initView =
                ResourceDictionary.create
                    [ ResourceDictionary.keyValue (key1, value1)
                      ResourceDictionary.keyValue (key2, value2) ]

            let target = VirtualDom.create initView

            Assert.containsKeyWithValue target key1 value1
            Assert.containsKeyWithValue target key2 value2

            let updatedView =
                ResourceDictionary.create
                    [ ResourceDictionary.keyValue (key1, updatedValue1)
                      ResourceDictionary.keyValue (key2, updatedValue2) ]

            VirtualDom.update target initView updatedView

            Assert.containsKeyWithValue target key1 updatedValue1
            Assert.containsKeyWithValue target key2 updatedValue2

            let revertedView = ResourceDictionary.create []
            VirtualDom.update target updatedView revertedView

            Assert.notContainsKey target key1
            Assert.notContainsKey target key2

        [<Fact>]
        let ``can set with IView`` () =
            let assertResourceTextEquals dict key expectedText =
                Assert.containsKeyWithValueWith dict key expectedText (fun o -> (o :?> TextBlock).Text)

            let initView = ResourceDictionary.create []
            let target = VirtualDom.create initView

            let key = "myKey"

            Assert.notContainsKey target key

            let initText = "Hello, World!"
            let valueView = TextBlock.create [ TextBlock.text initText ]

            let updatedView =
                ResourceDictionary.create [ ResourceDictionary.keyValue (key, valueView) ]

            VirtualDom.update target initView updatedView
            assertResourceTextEquals target key initText

            let updatedText = "Goodbye, World!"
            let updatedValueView = TextBlock.create [ TextBlock.text updatedText ]

            let updatedView2 =
                ResourceDictionary.create [ ResourceDictionary.keyValue (key, updatedValueView) ]

            VirtualDom.update target updatedView updatedView2

            assertResourceTextEquals target key updatedText

            let revertedView = ResourceDictionary.create []
            VirtualDom.update target updatedView revertedView

            Assert.notContainsKey target key

        [<Fact>]
        let ``with object creates new instance on update`` () =
            let key = "myKey"
            let text = "TextBlock Text"

            let view () =
                ResourceDictionary.create [ ResourceDictionary.keyValue (key, TextBlock(Text = text)) ]

            let initialView = view ()
            let target = VirtualDom.create initialView
            let initialTextBlock = Assert.containsKey target key :?> TextBlock
            Assert.Equal(text, initialTextBlock.Text)

            let updatedView = view ()
            VirtualDom.update target initialView updatedView

            let updatedTextBlock = Assert.containsKey target key :?> TextBlock
            Assert.NotEqual(initialTextBlock, updatedTextBlock)
            Assert.Equal(text, updatedTextBlock.Text)

        [<Fact>]
        let ``with IView reuses existing instance when patchable`` () =
            let key = "myKey"
            let initialText = "Initial Text"
            let updatedText = "Updated Text"

            let view text =
                ResourceDictionary.create
                    [ ResourceDictionary.keyValue (key, TextBlock.create [ TextBlock.text text ]) ]

            let initialView = view initialText
            let target = VirtualDom.create initialView
            let initialTextBlock = Assert.containsKey target key :?> TextBlock
            Assert.Equal(initialText, initialTextBlock.Text)

            let updatedView = view updatedText
            VirtualDom.update target initialView updatedView

            let updatedTextBlock = Assert.containsKey target key :?> TextBlock
            Assert.Equal(initialTextBlock, updatedTextBlock)
            Assert.Equal(updatedText, updatedTextBlock.Text)
