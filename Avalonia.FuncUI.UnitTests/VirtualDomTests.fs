namespace Avalonia.FuncUI.UnitTests

open Xunit
open Avalonia.FuncUI.Core.Model
open Avalonia.FuncUI.Core.VirtualDom
open Avalonia.FuncUI
open Avalonia.FuncUI.Core
open System
open Microsoft.FSharp.Quotations
open System.Linq.Expressions
open System

module VirtualDomTests =

    module DiffTests =
        open Avalonia.Controls
        open Avalonia
        open Avalonia.Media

        [<Fact>]
        let ``Property - next and last have the same value`` () =
            let last : Attr list = [
                Property {
                    Property = Button.BackgroundProperty :> AvaloniaProperty
                    Value = SolidColorBrush(Colors.Red).ToImmutable()
                }
            ]

            let next : Attr list = [
                Property {
                    Property = Button.BackgroundProperty :> AvaloniaProperty
                    Value = SolidColorBrush(Colors.Red).ToImmutable()
                }
            ]

            let result = VirtualDom.Differ.diffAttrs last next
            Assert.True(result.IsEmpty)

        [<Fact>]
        let ``Property - next and last have a different value`` () =
            let last : Attr list = [
                Property {
                    Property = Button.BackgroundProperty :> AvaloniaProperty
                    Value = SolidColorBrush(Colors.Red).ToImmutable()
                }
            ]
                

            let next : Attr list = [
                Property {
                    Property = Button.BackgroundProperty :> AvaloniaProperty
                    Value = SolidColorBrush(Colors.Green).ToImmutable()
                }
            ]

            let result = VirtualDom.Differ.diffAttrs last next
            Assert.True(result.Length = 1)
            match result.Head with
            | Property property -> 
                Assert.True(property.Value = (SolidColorBrush(Colors.Green).ToImmutable() :> obj))
            | _ ->
                Assert.True(false)

        [<Fact>]
        let ``Property - next does not provide new value for last property`` () =
            let last : Attr list = [
                Property {
                    Property = Button.BackgroundProperty :> AvaloniaProperty
                    Value = SolidColorBrush(Colors.Red).ToImmutable()
                }
            ]

            let next : Attr list = []

            let result = VirtualDom.Differ.diffAttrs last next
            match result.Head with
            | Property property -> 
                Assert.True(property.Value = AvaloniaProperty.UnsetValue)
            | _ ->
                Assert.True(false)

        [<Fact>]
        let ``Property - last value is not present`` () =
            let last : Attr list = []

            let next : Attr list = [
                Property {
                    Property = Button.BackgroundProperty :> AvaloniaProperty
                    Value = SolidColorBrush(Colors.Red).ToImmutable()
                }
            ]

            let result = VirtualDom.Differ.diffAttrs last next
            Assert.Equal(next.Head, result.Head)

        [<Fact>]
        let ``Content Single - next and last have the same value`` () =
            let last : Attr list = [
                Attr.createContent("Content", ViewContent.Single (Some {
                    ViewType = typeof<TextBlock>
                    Attrs = [
                        Property {
                            Property = TextBlock.TextProperty :> AvaloniaProperty
                            Value = "Some Text"
                        }
                    ]                
                }))
            ]

            let next : Attr list = [
                Attr.createContent("Content", ViewContent.Single (Some {
                    ViewType = typeof<TextBlock>
                    Attrs = [
                        Property {
                            Property = TextBlock.TextProperty :> AvaloniaProperty
                            Value = "Some Text"
                        }
                    ]                
                }))
            ]

            let result = VirtualDom.Differ.diffAttrs last next
            Assert.True(result.IsEmpty)

        [<Fact>]
        let ``Content Single - next and last have a different value`` () =
            let last : Attr list = [
                Attr.createContent("Content", ViewContent.Single (Some {
                    ViewType = typeof<TextBlock>
                    Attrs = [
                        Property {
                            Property = TextBlock.TextProperty :> AvaloniaProperty
                            Value = "Some Text"
                        }
                    ]                
                }))
            ]

            let next : Attr list = [
                Attr.createContent("Content", ViewContent.Single (Some {
                    ViewType = typeof<TextBlock>
                    Attrs = [
                        Property {
                            Property = TextBlock.TextProperty :> AvaloniaProperty
                            Value = "Some other Text"
                        }
                    ]                
                }))
            ]

            let result = VirtualDom.Differ.diffAttrs last next
            Assert.Equal(next.Head, result.Head)

        [<Fact>]
        let ``Content Single - next does not provide new value for last property`` () =
            let last : Attr list = [
                Attr.createContent("Content", ViewContent.Single (Some {
                    ViewType = typeof<TextBlock>
                    Attrs = [
                        Property {
                            Property = TextBlock.TextProperty :> AvaloniaProperty
                            Value = "Some Text"
                        }
                    ]                
                }))
            ]

            let next : Attr list = []

            let result = VirtualDom.Differ.diffAttrs last next
            match result.Head with
            | Content content -> 
                match content.Content with
                | ViewContent.Single single -> 
                    Assert.True(single.IsNone)
                | _ ->
                    Assert.True(false)
            | _ ->
                Assert.True(false)

        [<Fact>]
        let ``Content Single - last value is not present`` () =
            let last : Attr list = []

            let next : Attr list = [
                Attr.createContent("Content", ViewContent.Single (Some {
                    ViewType = typeof<TextBlock>
                    Attrs = [
                        Property {
                            Property = TextBlock.TextProperty :> AvaloniaProperty
                            Value = "Some Text"
                        }
                    ]                
                }))
            ]

            let result = VirtualDom.Differ.diffAttrs last next
            Assert.Equal(next.Head, result.Head)



module FuncCompare = 

    type Msg = Increment | Decrement

    let dispatch (msg : Msg) = ()

    [<Fact>]
    let ``Comparing funcs`` () =

        let getIL (func: 'a -> 'b) =
            let t = func.GetType()
            let m = t.GetMethod("Invoke")
            let b = m.GetMethodBody()
            b.GetILAsByteArray()

        let compare (funcA: 'a -> 'b) (funcB: 'c -> 'd) : bool=
            let bytesA = getIL funcA
            let bytesB = getIL funcB
            let spanA = ReadOnlySpan(bytesA)
            let spanB = ReadOnlySpan(bytesB)
            spanA.SequenceEqual(spanB)

        let a = fun () -> dispatch Increment
        let b = fun () -> dispatch Increment

        Assert.True(compare a b)