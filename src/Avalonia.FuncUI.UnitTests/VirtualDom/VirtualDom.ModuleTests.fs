namespace Avalonia.FuncUI.UnitTests.VirtualDom

module ModuleTests =
    open Avalonia.FuncUI.VirtualDom
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI
    open Avalonia.Controls
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