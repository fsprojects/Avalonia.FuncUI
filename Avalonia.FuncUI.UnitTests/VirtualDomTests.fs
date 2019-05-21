namespace Avalonia.FuncUI.UnitTests

open Xunit
open Avalonia.FuncUI.Core
open Avalonia.FuncUI

module VirtualDomTests =

    module DiffTests =
        open Avalonia.Controls
        open Avalonia
        open Avalonia.Media

        [<Fact>]
        let ``Property - next and last have the same value`` () =
            let last : AttrInfo list = [
                {
                    ViewType = typeof<Button>
                    Attr = Property {
                        Property = Button.BackgroundProperty :> AvaloniaProperty
                        Value = SolidColorBrush(Colors.Red).ToImmutable()
                    }
                }
            ]

            let next : AttrInfo list = [
                {
                    ViewType = typeof<Button>
                    Attr = Property {
                        Property = Button.BackgroundProperty :> AvaloniaProperty
                        Value = SolidColorBrush(Colors.Red).ToImmutable()
                    }
                }
            ]

            let result = VirtualDom.Diff.diffAttrInfos last next
            Assert.True(result.IsEmpty)

        [<Fact>]
        let ``Property - next and last have a different value`` () =
            let last : AttrInfo list = [
                {
                    ViewType = typeof<Button>
                    Attr = Property {
                        Property = Button.BackgroundProperty :> AvaloniaProperty
                        Value = SolidColorBrush(Colors.Red).ToImmutable()
                    }
                }
            ]

            let next : AttrInfo list = [
                {
                    ViewType = typeof<Button>
                    Attr = Property {
                        Property = Button.BackgroundProperty :> AvaloniaProperty
                        Value = SolidColorBrush(Colors.Green).ToImmutable()
                    }
                }
            ]

            let result = VirtualDom.Diff.diffAttrInfos last next
            Assert.True(result.Length = 1)
            match result.Head.Attr with
            | Property property -> 
                Assert.True(property.Value = (SolidColorBrush(Colors.Green).ToImmutable() :> obj))
            | _ ->
                Assert.True(false)

        [<Fact>]
        let ``Property - next does not provide new value for last property`` () =
            let last : AttrInfo list = [
                {
                    ViewType = typeof<Button>
                    Attr = Property {
                        Property = Button.BackgroundProperty :> AvaloniaProperty
                        Value = SolidColorBrush(Colors.Red).ToImmutable()
                    }
                }
            ]

            let next : AttrInfo list = []

            let result = VirtualDom.Diff.diffAttrInfos last next
            match result.Head.Attr with
            | Property property -> 
                Assert.True(property.Value = AvaloniaProperty.UnsetValue)
            | _ ->
                Assert.True(false)

        [<Fact>]
        let ``Property - last value is not present`` () =
            let last : AttrInfo list = []

            let next : AttrInfo list = [
                {
                    ViewType = typeof<Button>
                    Attr = Property {
                        Property = Button.BackgroundProperty :> AvaloniaProperty
                        Value = SolidColorBrush(Colors.Red).ToImmutable()
                    }
                }
            ]

            let result = VirtualDom.Diff.diffAttrInfos last next
            Assert.Equal(next.Head, result.Head)