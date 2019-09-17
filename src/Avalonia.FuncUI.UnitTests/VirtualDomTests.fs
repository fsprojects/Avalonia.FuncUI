namespace Avalonia.FuncUI.UnitTests

open Avalonia.Controls
open Avalonia.FuncUI.Core.Domain
open Avalonia.FuncUI.DSL.Extensions
open Xunit
open System



module VirtualDomTests =
    open Avalonia.FuncUI.VirtualDom
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI
    
    open Avalonia.Layout
    open Avalonia.Controls
    open Avalonia.Controls.Primitives
    open Avalonia.Media

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

    module DifferTests = 

        [<Fact>]
        let ``Diff Properties`` () =
            let last =
                TextBlock.create [
                    TextBlock.background "green"
                    TextBlock.text "some text"
                    TextBlock.fontStyle FontStyle.Italic
                ]

            let next =
                TextBlock.create [
                    TextBlock.background "green" // keep the same
                    TextBlock.text "some other text" // change
                    // don't include last attr
                ]

            let delta : Delta.ViewDelta = 
                {
                    viewType = typeof<TextBlock>
                    attrs = [
                        Delta.AttrDelta.Property {
                            accessor = Accessor.Avalonia TextBlock.TextProperty
                            value = Some ("some other text" :> obj)
                        };
                        Delta.AttrDelta.Property {
                            accessor = Accessor.Avalonia TextBlock.FontStyleProperty
                            value = None
                        };                       
                    ]
                }

            let result = Differ.diff(last, next)
        
            Assert.Equal(delta, result)

            // just to make sure the types are actually comparable
            Assert.True(not (delta <> result))

        [<Fact>]
        let ``Diff Properties (only if types match)`` () =
            let last =
                StackPanel.create [
                    StackPanel.orientation Orientation.Horizontal
                ]

            let next =
               TextBlock.create [
                    TextBlock.text "some other text"
                ]

            let delta : Delta.ViewDelta = 
                {
                    viewType = typeof<TextBlock>
                    attrs = [
                        Delta.AttrDelta.Property {
                            accessor = Accessor.Avalonia TextBlock.TextProperty
                            value = Some ("some other text" :> obj)
                        };
                    ]
                }

            let result = Differ.diff(last, next)
        
            Assert.Equal(delta, result)

            // just to make sure the types are actually comparable
            Assert.True(not (delta <> result))

        [<Fact>]
        let ``Diff Content Single`` () =
                let last =
                    Button.create [
                        Button.background "green"
                        Button.content (
                            TextBlock.create [
                                TextBlock.text "some text"
                                TextBlock.fontSize 14.0
                            ]
                        )
                    ]

                let next =
                    Button.create [
                        Button.background "red"
                        Button.content (
                            TextBlock.create [
                                TextBlock.text "some other text"
                                TextBlock.fontSize 15.0
                            ]
                        )
                    ]

                let delta : Delta.ViewDelta = 
                    {
                        viewType = typeof<Button>
                        attrs = [
                            Delta.AttrDelta.Property {
                                accessor = Accessor.Avalonia Button.BackgroundProperty
                                value = Some ((Avalonia.Media.SolidColorBrush.Parse("red").ToImmutable()) :> obj)
                            };
                            Delta.AttrDelta.Content {
                                accessor = Accessor.Avalonia Button.ContentProperty
                                content = Delta.ViewContentDelta.Single
                                    ( Some {
                                        viewType = typeof<TextBlock>
                                        attrs = [
                                            Delta.AttrDelta.Property {
                                                accessor = Accessor.Avalonia TextBlock.TextProperty
                                                value = Some ("some other text" :> obj)
                                            };
                                            Delta.AttrDelta.Property {
                                                accessor = Accessor.Avalonia TextBlock.FontSizeProperty
                                                value = Some (15.0 :> obj)
                                            };
                                        ]
                                    }
                                )
                            };
                        ]
                    }

                let result = Differ.diff(last, next)
            
                Assert.Equal(delta, result)

                // just to make sure the types are actually comparable
                Assert.True(not (delta <> result))
                
        [<Fact>]
        let ``Diff Content Multiple`` () =
                let last =
                    StackPanel.create [
                        StackPanel.orientation Orientation.Horizontal
                        StackPanel.children [
                            TextBlock.create [
                                TextBlock.text "some text"
                                TextBlock.fontSize 14.0
                            ]
                            Button.create [
                                Button.background "green"
                            ]
                        ]
                    ]

                let next =
                    StackPanel.create [
                        StackPanel.orientation Orientation.Vertical
                        StackPanel.children [
                            TextBlock.create [
                                TextBlock.text "some other text"
                                TextBlock.fontSize 15.0
                            ]
                            Button.create [
                                Button.background "red"
                            ]
                        ]
                    ]

                let delta : Delta.ViewDelta = 
                    {
                        viewType = typeof<StackPanel>
                        attrs = [
                            Delta.AttrDelta.Property {
                                accessor =  Accessor.Avalonia StackPanel.OrientationProperty
                                value = Some (Orientation.Vertical :> obj)
                            };
                            Delta.AttrDelta.Content {
                                accessor = Accessor.Instance "Children"
                                content = Delta.ViewContentDelta.Multiple [
                                    {
                                        viewType = typeof<TextBlock>
                                        attrs = [
                                            Delta.AttrDelta.Property {
                                                accessor = Accessor.Avalonia TextBlock.TextProperty
                                                value = Some ("some other text" :> obj)
                                            };
                                            Delta.AttrDelta.Property {
                                                accessor = Accessor.Avalonia TextBlock.FontSizeProperty
                                                value = Some (15.0 :> obj)
                                            };
                                        ]
                                    };
                                    {
                                        viewType = typeof<Button>
                                        attrs = [
                                            Delta.AttrDelta.Property {
                                                accessor = Accessor.Avalonia Button.BackgroundProperty
                                                value = Some ((Avalonia.Media.SolidColorBrush.Parse("red").ToImmutable()) :> obj)
                                            };
                                        ]
                                    };
                                ]
                            };
                        ]
                    }

                let result = Differ.diff(last, next)
                
                Assert.Equal(delta, result)

                // just to make sure the types are actually comparable
                Assert.True(not (delta <> result))

        
        [<Fact>]
        let ``Diff Content Multiple (insert item into homogenous list - tail)`` () =
                let last =
                    StackPanel.create [
                        StackPanel.orientation Orientation.Horizontal
                        StackPanel.children [
                            CheckBox.create [
                                CheckBox.content "some text 1"
                                CheckBox.isChecked true
                            ]
                            CheckBox.create [
                                CheckBox.content "some text 2"
                                CheckBox.isChecked false
                            ]
                        ]
                    ]

                let next =
                    StackPanel.create [
                        StackPanel.orientation Orientation.Vertical
                        StackPanel.children [
                            CheckBox.create [
                                CheckBox.content "some text [new]"
                                CheckBox.isChecked false
                            ]
                            CheckBox.create [
                                CheckBox.content "some text 1"
                                CheckBox.isChecked true
                            ]
                            CheckBox.create [
                                CheckBox.content "some text 2"
                                CheckBox.isChecked false
                            ]
                        ]
                    ]

                let delta : Delta.ViewDelta = 
                    {
                        viewType = typeof<StackPanel>
                        attrs = [
                            Delta.AttrDelta.Property {
                                accessor = Accessor.Avalonia StackPanel.OrientationProperty
                                value = Some (Orientation.Vertical :> obj)
                            };
                            Delta.AttrDelta.Content {
                                accessor = Accessor.Instance "Children"
                                content = Delta.ViewContentDelta.Multiple [
                                    {
                                        viewType = typeof<CheckBox>
                                        attrs = [
                                            Delta.AttrDelta.Property {
                                                accessor = Accessor.Avalonia CheckBox.ContentProperty
                                                value = Some ("some text [new]" :> obj)
                                            };
                                            Delta.AttrDelta.Property {
                                                accessor = Accessor.Avalonia CheckBox.IsCheckedProperty
                                                value = Some (false :> obj)
                                            };
                                        ]
                                    };
                                    {
                                        viewType = typeof<CheckBox>
                                        attrs = [
                                            Delta.AttrDelta.Property {
                                                accessor = Accessor.Avalonia CheckBox.ContentProperty
                                                value = Some ("some text 1" :> obj)
                                            };
                                            Delta.AttrDelta.Property {
                                                accessor = Accessor.Avalonia CheckBox.IsCheckedProperty
                                                value = Some (true :> obj)
                                            };
                                        ]
                                    };
                                    {
                                        viewType = typeof<CheckBox>
                                        attrs = [
                                            Delta.AttrDelta.Property {
                                                accessor = Accessor.Avalonia CheckBox.ContentProperty
                                                value = Some ("some text 2" :> obj)
                                            };
                                            Delta.AttrDelta.Property {
                                                accessor = Accessor.Avalonia CheckBox.IsCheckedProperty
                                                value = Some (false :> obj)
                                            };
                                        ]
                                    };
                                ]
                            };
                        ]
                    }

                let result = Differ.diff(last, next)
                
                Assert.Equal(delta, result)

                // just to make sure the types are actually comparable
                Assert.True(not (delta <> result))

        (*
        [<Fact>]
        let ``Diff Content Multiple (insert iten into homogenous list - head)`` () =
                let last =
                    StackPanel.create [
                        Attrs.orientation Orientation.Horizontal
                        StackPanel.children [
                            CheckBox.create [
                                Attrs.content "some text 1"
                                Attrs.isChecked true
                            ]
                            CheckBox.create [
                                Attrs.content "some text 2"
                                Attrs.isChecked false
                            ]
                        ]
                    ]

                let next =
                    StackPanel.create [
                        Attrs.orientation Orientation.Vertical
                        StackPanel.children [
                            CheckBox.create [
                                Attrs.content "some text 1"
                                Attrs.isChecked true
                            ]
                            CheckBox.create [
                                Attrs.content "some text 2"
                                Attrs.isChecked false
                            ]
                            CheckBox.create [
                                Attrs.content "some text [new]"
                                Attrs.isChecked true
                            ]
                        ]
                    ]

                let delta = 
                    {
                        ViewType = typeof<StackPanel>
                        Attrs = [
                            AttrDelta.PropertyDelta {
                                Name = "Orientation"
                                Value = Some (Orientation.Vertical :> obj)
                            };
                            AttrDelta.ContentDelta {
                                Name = "Children"
                                Content = ViewContentDelta.Multiple [
                                    {
                                        ViewType = typeof<CheckBox>
                                        Attrs = []
                                    };
                                    {
                                        ViewType = typeof<CheckBox>
                                        Attrs = []
                                    };
                                    {
                                        ViewType = typeof<CheckBox>
                                        Attrs = [
                                            AttrDelta.PropertyDelta {
                                                Name = "Content"
                                                Value = Some ("some text [new]" :> obj)
                                            };
                                            AttrDelta.PropertyDelta {
                                                Name = "IsChecked"
                                                Value = Some (true :> obj)
                                            };
                                        ]
                                    };
                                ]
                            };
                        ]
                    }

                let result = Differ.diff(last, next)
                
                Assert.Equal(delta, result)

                // just to make sure the types are actually comparable
                Assert.True(not (delta <> result))

    module PatcherTests = 

        [<Fact>]
        let ``Patch Properties`` () =

            let delta = 
                {
                    ViewType = typeof<TextBlock>
                    Attrs = [
                        AttrDelta.PropertyDelta {
                            Name = "Text"
                            Value = Some ("some text" :> obj)
                        };
                        AttrDelta.PropertyDelta {
                            Name = "FontSize"
                            Value = Some (14.0 :> obj)
                        };
                    ]
                }

            let control = TextBlock()

            Patcher.patch (control, delta)

            Assert.Equal("some text", control.Text)
            Assert.Equal(14.0, control.FontSize)

        [<Fact>]
        let ``Patch Content Single`` () =

            let delta = 
                 {
                     ViewType = typeof<Button>
                     Attrs = [
                         AttrDelta.ContentDelta {
                             Name = "Content"
                             Content = ViewContentDelta.Single
                                 ( Some {
                                     ViewType = typeof<TextBlock>
                                     Attrs = [
                                         AttrDelta.PropertyDelta {
                                             Name = "Text"
                                             Value = Some ("some text" :> obj)
                                         };
                                         AttrDelta.PropertyDelta {
                                             Name = "FontSize"
                                             Value = Some (15.0 :> obj)
                                         };
                                     ]
                                 }
                             )
                         };
                     ]
                 }

            let control = Button()

            Patcher.patch (control, delta)

            Assert.IsType(typeof<TextBlock>,  control.Content)
            
            let textblock = control.Content :?> TextBlock

            Assert.Equal("some text", textblock.Text)
            Assert.Equal(15.0, textblock.FontSize)

        [<Fact>]
        let ``Patch Content Multiple`` () =

            let delta = 
                {
                    ViewType = typeof<StackPanel>
                    Attrs = [
                        AttrDelta.ContentDelta {
                            Name = "Children"
                            Content = ViewContentDelta.Multiple [
                                {
                                    ViewType = typeof<TextBlock>
                                    Attrs = [
                                        AttrDelta.PropertyDelta {
                                            Name = "Text"
                                            Value = Some ("some text" :> obj)
                                        };
                                        AttrDelta.PropertyDelta {
                                            Name = "FontSize"
                                            Value = Some (15.0 :> obj)
                                        };
                                    ]
                                };
                                {
                                    ViewType = typeof<Button>
                                    Attrs = [
                                        AttrDelta.PropertyDelta {
                                            Name = "Background"
                                            Value = Some ((SolidColorBrush.Parse("red").ToImmutable()) :> obj)
                                        };
                                    ]
                                };
                                {
                                    ViewType = typeof<Button>
                                    Attrs = [
                                        AttrDelta.PropertyDelta {
                                            Name = "Background"
                                            Value = Some ((SolidColorBrush.Parse("green").ToImmutable()) :> obj)
                                        };
                                    ]
                                };
                            ]
                        };
                    ]
                }

            let stackpanel = StackPanel()
            // should be updated by patcher
            stackpanel.Children.Add(TextBlock())
            stackpanel.Children.Add(Button())

            // should be replaced by patcher      
            stackpanel.Children.Add(TextBlock())
            
            // should be removed by patcher
            stackpanel.Children.Add(Button())

            Patcher.patch (stackpanel, delta)

            Assert.Equal(3, stackpanel.Children.Count)

            Assert.IsType(typeof<TextBlock>,  stackpanel.Children.[0])
            let textblock = stackpanel.Children.[0] :?> TextBlock
            Assert.Equal("some text", textblock.Text)
            Assert.Equal(15.0, textblock.FontSize)

            Assert.IsType(typeof<Button>,  stackpanel.Children.[1])
            let button = stackpanel.Children.[1] :?> Button
            Assert.Equal(SolidColorBrush.Parse("red").ToImmutable(), button.Background)

            Assert.IsType(typeof<Button>,  stackpanel.Children.[2])
            let button = stackpanel.Children.[2] :?> Button
            Assert.Equal(SolidColorBrush.Parse("green").ToImmutable(), button.Background)
*)