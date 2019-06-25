namespace Avalonia.FuncUI.UnitTests

open Xunit
open System



module VirtualDomTests =

    module DifferTests = 
        open Avalonia.FuncUI.VirtualDom
        open Avalonia.FuncUI.VirtualDom.Delta
        open Avalonia.FuncUI
        open Avalonia.Controls
        open Avalonia.Media

        [<Fact>]
        let ``Diff Properties`` () =
            let last =
                Views.textBlock [
                    Attrs.background "green"
                    Attrs.text "some text"
                    Attrs.fontStyle FontStyle.Italic
                ]

            let next =
                Views.textBlock [
                    Attrs.background "green" // keep the same
                    Attrs.text "some other text" // change
                    // don't include last attr
                ]

            let delta = 
                {
                    ViewType = typeof<TextBlock>
                    Attrs = [
                        AttrDelta.PropertyDelta {
                            Name = "Text"
                            Value = Some ("some other text" :> obj)
                        };
                        AttrDelta.PropertyDelta {
                            Name = "FontStyle"
                            Value = None
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
                Views.stackPanel [
                    Attrs.orientation Orientation.Horizontal
                ]

            let next =
                Views.textBlock [
                    Attrs.text "some other text"
                ]

            let delta = 
                {
                    ViewType = typeof<TextBlock>
                    Attrs = [
                        AttrDelta.PropertyDelta {
                            Name = "Text"
                            Value = Some ("some other text" :> obj)
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
                    Views.button [
                        Attrs.background "green"
                        Attrs.content (
                            Views.textBlock [
                                Attrs.text "some text"
                                Attrs.fontSize 14.0
                            ]
                        )
                    ]

                let next =
                    Views.button [
                        Attrs.background "red"
                        Attrs.content (
                            Views.textBlock [
                                Attrs.text "some other text"
                                Attrs.fontSize 15.0
                            ]
                        )
                    ]

                let delta = 
                    {
                        ViewType = typeof<Button>
                        Attrs = [
                            AttrDelta.PropertyDelta {
                                Name = "Background"
                                Value = Some ((Avalonia.Media.SolidColorBrush.Parse("red").ToImmutable()) :> obj)
                            };
                            AttrDelta.ContentDelta {
                                Name = "Content"
                                Content = ViewContentDelta.Single
                                    ( Some {
                                        ViewType = typeof<TextBlock>
                                        Attrs = [
                                            AttrDelta.PropertyDelta {
                                                Name = "Text"
                                                Value = Some ("some other text" :> obj)
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

                let result = Differ.diff(last, next)
            
                Assert.Equal(delta, result)

                // just to make sure the types are actually comparable
                Assert.True(not (delta <> result))

        [<Fact>]
        let ``Diff Content Multiple`` () =
                let last =
                    Views.stackPanel [
                        Attrs.orientation Orientation.Horizontal
                        Attrs.children [
                            Views.textBlock [
                                Attrs.text "some text"
                                Attrs.fontSize 14.0
                            ]
                            Views.button [
                                Attrs.background "green"
                            ]
                        ]
                    ]

                let next =
                    Views.stackPanel [
                        Attrs.orientation Orientation.Vertical
                        Attrs.children [
                            Views.textBlock [
                                Attrs.text "some other text"
                                Attrs.fontSize 15.0
                            ]
                            Views.button [
                                Attrs.background "red"
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
                                        ViewType = typeof<TextBlock>
                                        Attrs = [
                                            AttrDelta.PropertyDelta {
                                                Name = "Text"
                                                Value = Some ("some other text" :> obj)
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
                                                Value = Some ((Avalonia.Media.SolidColorBrush.Parse("red").ToImmutable()) :> obj)
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
        let ``Diff Content Multiple (insert iten into homogenous list - tail)`` () =
                let last =
                    Views.stackPanel [
                        Attrs.orientation Orientation.Horizontal
                        Attrs.children [
                            Views.checkBox [
                                Attrs.content "some text 1"
                                Attrs.isChecked true
                            ]
                            Views.checkBox [
                                Attrs.content "some text 2"
                                Attrs.isChecked false
                            ]
                        ]
                    ]

                let next =
                    Views.stackPanel [
                        Attrs.orientation Orientation.Vertical
                        Attrs.children [
                            Views.checkBox [
                                Attrs.content "some text [new]"
                                Attrs.isChecked false
                            ]
                            Views.checkBox [
                                Attrs.content "some text 1"
                                Attrs.isChecked true
                            ]
                            Views.checkBox [
                                Attrs.content "some text 2"
                                Attrs.isChecked false
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
                                        Attrs = [
                                            AttrDelta.PropertyDelta {
                                                Name = "Content"
                                                Value = Some ("some text [new]" :> obj)
                                            };
                                            AttrDelta.PropertyDelta {
                                                Name = "IsChecked"
                                                Value = Some (false :> obj)
                                            };
                                        ]
                                    };
                                    {
                                        ViewType = typeof<CheckBox>
                                        Attrs = [
                                            AttrDelta.PropertyDelta {
                                                Name = "Content"
                                                Value = Some ("some text 1" :> obj)
                                            };
                                            AttrDelta.PropertyDelta {
                                                Name = "IsChecked"
                                                Value = Some (true :> obj)
                                            };
                                        ]
                                    };
                                    {
                                        ViewType = typeof<CheckBox>
                                        Attrs = [
                                            AttrDelta.PropertyDelta {
                                                Name = "Content"
                                                Value = Some ("some text 2" :> obj)
                                            };
                                            AttrDelta.PropertyDelta {
                                                Name = "IsChecked"
                                                Value = Some (false :> obj)
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
        let ``Diff Content Multiple (insert iten into homogenous list - head)`` () =
                let last =
                    Views.stackPanel [
                        Attrs.orientation Orientation.Horizontal
                        Attrs.children [
                            Views.checkBox [
                                Attrs.content "some text 1"
                                Attrs.isChecked true
                            ]
                            Views.checkBox [
                                Attrs.content "some text 2"
                                Attrs.isChecked false
                            ]
                        ]
                    ]

                let next =
                    Views.stackPanel [
                        Attrs.orientation Orientation.Vertical
                        Attrs.children [
                            Views.checkBox [
                                Attrs.content "some text 1"
                                Attrs.isChecked true
                            ]
                            Views.checkBox [
                                Attrs.content "some text 2"
                                Attrs.isChecked false
                            ]
                            Views.checkBox [
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
        open Avalonia.FuncUI.VirtualDom
        open Avalonia.FuncUI.VirtualDom.Delta
        open Avalonia.FuncUI
        open Avalonia.Controls
        open Avalonia.Media

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