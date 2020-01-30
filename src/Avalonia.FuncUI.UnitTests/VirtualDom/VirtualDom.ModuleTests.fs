namespace Avalonia.FuncUI.UnitTests.VirtualDom

open Avalonia.Controls
open Avalonia.FuncUI.Types
open Avalonia.Media
open Avalonia.Media.Immutable

module ModuleTests =
    open Avalonia.FuncUI.VirtualDom
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI
    open Xunit
    open System

    [<Fact>]
    let ``integration test 1`` () =
        let view =
            StackPanel.create [
                StackPanel.children [
                    CheckBox.create [
                        CheckBox.content "1"
                        CheckBox.isChecked true
                    ]
                    CheckBox.create [
                        CheckBox.content "2"
                        CheckBox.isChecked true
                    ]
                ]
            ]

        let view' =
            StackPanel.create [
                StackPanel.children [
                    CheckBox.create [
                        CheckBox.content "1"
                        CheckBox.isChecked false
                    ]
                    CheckBox.create [
                        CheckBox.content "2"
                        CheckBox.isChecked false
                    ]
                    CheckBox.create [
                        CheckBox.content "3"
                        CheckBox.isChecked true
                    ]
                ]
            ]

        let stackpanel = StackPanel()

        VirtualDom.Patcher.patch(stackpanel, VirtualDom.Delta.ViewDelta.From view)
        Assert.Equal(2, stackpanel.Children.Count)
        Assert.Equal("1" :> obj, (stackpanel.Children.[0] :?> CheckBox).Content)
        Assert.Equal(Nullable(true), (stackpanel.Children.[0] :?> CheckBox).IsChecked)
        Assert.Equal("2" :> obj, (stackpanel.Children.[1] :?> CheckBox).Content)
        Assert.Equal(Nullable(true), (stackpanel.Children.[1] :?> CheckBox).IsChecked)

        VirtualDom.Patcher.patch(stackpanel, VirtualDom.Differ.diff(view, view'))
        Assert.Equal(3, stackpanel.Children.Count)
        Assert.Equal("1" :> obj, (stackpanel.Children.[0] :?> CheckBox).Content)
        Assert.Equal(Nullable(false), (stackpanel.Children.[0] :?> CheckBox).IsChecked)
        Assert.Equal("2" :> obj, (stackpanel.Children.[1] :?> CheckBox).Content)
        Assert.Equal(Nullable(false), (stackpanel.Children.[1] :?> CheckBox).IsChecked)
        Assert.Equal("3" :> obj, (stackpanel.Children.[2] :?> CheckBox).Content)
        Assert.Equal(Nullable(true), (stackpanel.Children.[2] :?> CheckBox).IsChecked)
        ()

    [<Fact>]
    let ``integration test 2`` () =
        let view =
            StackPanel.create [
                StackPanel.children [
                    CheckBox.create [
                        CheckBox.content "1"
                        CheckBox.isChecked true
                    ]
                    CheckBox.create [
                        CheckBox.content "2"
                        CheckBox.isChecked true
                    ]
                ]
            ]

        let view' =
            StackPanel.create [
                StackPanel.children [
                    CheckBox.create [
                        CheckBox.content "new"
                        CheckBox.isChecked false
                    ]
                    CheckBox.create [
                        CheckBox.content "1"
                        CheckBox.isChecked false
                    ]
                    CheckBox.create [
                        CheckBox.content "2"
                        CheckBox.isChecked true
                    ]

                ]
            ]

        let view'' =
            StackPanel.create [
                StackPanel.children [
                    CheckBox.create [
                        CheckBox.content "new"
                        CheckBox.isChecked true
                    ]
                    CheckBox.create [
                        CheckBox.content "1"
                        CheckBox.isChecked false
                    ]
                    CheckBox.create [
                        CheckBox.content "2"
                        CheckBox.isChecked false
                    ]

                ]
            ]

        let stackpanel = StackPanel()

        VirtualDom.Patcher.patch(stackpanel, VirtualDom.Delta.ViewDelta.From view)
        Assert.Equal(2, stackpanel.Children.Count)
        Assert.Equal("1" :> obj, (stackpanel.Children.[0] :?> CheckBox).Content)
        Assert.Equal(Nullable(true), (stackpanel.Children.[0] :?> CheckBox).IsChecked)
        Assert.Equal("2" :> obj, (stackpanel.Children.[1] :?> CheckBox).Content)
        Assert.Equal(Nullable(true), (stackpanel.Children.[1] :?> CheckBox).IsChecked)

        VirtualDom.Patcher.patch(stackpanel, VirtualDom.Differ.diff(view, view'))
        Assert.Equal(3, stackpanel.Children.Count)
        Assert.Equal("new" :> obj, (stackpanel.Children.[0] :?> CheckBox).Content)
        Assert.Equal(Nullable(false), (stackpanel.Children.[0] :?> CheckBox).IsChecked)
        Assert.Equal("1" :> obj, (stackpanel.Children.[1] :?> CheckBox).Content)
        Assert.Equal(Nullable(false), (stackpanel.Children.[1] :?> CheckBox).IsChecked)
        Assert.Equal("2" :> obj, (stackpanel.Children.[2] :?> CheckBox).Content)
        Assert.Equal(Nullable(true), (stackpanel.Children.[2] :?> CheckBox).IsChecked)

        VirtualDom.Patcher.patch(stackpanel, VirtualDom.Differ.diff(view', view''))
        Assert.Equal(3, stackpanel.Children.Count)
        Assert.Equal("new" :> obj, (stackpanel.Children.[0] :?> CheckBox).Content)
        Assert.Equal(Nullable(true), (stackpanel.Children.[0] :?> CheckBox).IsChecked)
        Assert.Equal("1" :> obj, (stackpanel.Children.[1] :?> CheckBox).Content)
        Assert.Equal(Nullable(false), (stackpanel.Children.[1] :?> CheckBox).IsChecked)
        Assert.Equal("2" :> obj, (stackpanel.Children.[2] :?> CheckBox).Content)
        Assert.Equal(Nullable(false), (stackpanel.Children.[2] :?> CheckBox).IsChecked)
        ()

    [<Fact>]
    let ``integration test 'VirtualDom.updateRoot' (from null to Button to TextBlock)`` () =
        let view : IView =
            Button.create [
                Button.content "I'm a button"
                Button.background "green"
            ] |> generalize

        let view': IView =
            TextBlock.create [
                TextBlock.text "I'm a text block"
                TextBlock.background "green"
            ] |> generalize

        let host = ContentControl()

        Assert.Equal(null, host.Content)

        VirtualDom.updateRoot (host, None, Some view)

        Assert.IsType(typeof<Button>, host.Content)
        Assert.Equal("I'm a button", (host.Content :?> Button).Content :?> string)
        Assert.Equal("green" |> Color.Parse |> ImmutableSolidColorBrush |> (fun a -> a :> IBrush), (host.Content :?> Button).Background.ToImmutable())

        VirtualDom.updateRoot (host, Some view, Some view')

        Assert.IsType(typeof<TextBlock>, host.Content)
        Assert.Equal("I'm a text block", (host.Content :?> TextBlock).Text)
        Assert.Equal("green" |> Color.Parse |> ImmutableSolidColorBrush |> (fun a -> a :> IBrush), (host.Content :?> TextBlock).Background.ToImmutable())
        ()


    [<Fact>]
    let ``integration test 'VirtualDom.updateRoot' (from null to Button to null)`` () =
        let view : IView =
            Button.create [
                Button.content "I'm a button"
                Button.background "green"
            ] |> generalize

        let host = ContentControl()

        Assert.Equal(null, host.Content)

        VirtualDom.updateRoot (host, None, Some view)

        Assert.IsType(typeof<Button>, host.Content)
        Assert.Equal("I'm a button", (host.Content :?> Button).Content :?> string)
        Assert.Equal("green" |> Color.Parse |> ImmutableSolidColorBrush |> (fun a -> a :> IBrush), (host.Content :?> Button).Background.ToImmutable())

        VirtualDom.updateRoot (host, Some view, None)

        Assert.Equal(null, host.Content)
        ()

    [<Fact>]
    let ``integration test 'VirtualDom.updateRoot' (reuse/patch button)`` () =
        let view : IView =
            Button.create [
                Button.content "I'm a button"
                Button.background "green"
            ] |> generalize

        let view': IView =
            Button.create [
                Button.content "I'm still a button"
                Button.background "red"
            ] |> generalize

        let host = ContentControl()

        Assert.Equal(null, host.Content)

        VirtualDom.updateRoot (host, None, Some view)

        let buttonRef = host.Content
        Assert.IsType(typeof<Button>, host.Content)
        Assert.Equal("I'm a button", (host.Content :?> Button).Content :?> string)
        Assert.Equal("green" |> Color.Parse |> ImmutableSolidColorBrush |> (fun a -> a :> IBrush), (host.Content :?> Button).Background.ToImmutable())

        VirtualDom.updateRoot (host, Some view, Some view')

        Assert.IsType(typeof<Button>, host.Content)
        Assert.Equal("I'm still a button", (host.Content :?> Button).Content :?> string)
        Assert.Equal("red" |> Color.Parse |> ImmutableSolidColorBrush |> (fun a -> a :> IBrush), (host.Content :?> Button).Background.ToImmutable())
        Assert.True(Object.ReferenceEquals(buttonRef, host.Content), "Button is still the same instance")