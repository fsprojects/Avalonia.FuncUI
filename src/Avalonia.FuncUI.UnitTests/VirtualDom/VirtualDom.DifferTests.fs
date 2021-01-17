namespace Avalonia.FuncUI.UnitTests.VirtualDom

module DifferTests =
    open Avalonia.FuncUI.VirtualDom
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Types
    open Avalonia.Controls
    open Xunit
    open Avalonia.Layout
    open Avalonia.Media

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
                ViewType = typeof<TextBlock>
                Attrs = [
                    Delta.AttrDelta.Property {
                        Accessor = Accessor.AvaloniaProperty TextBlock.TextProperty
                        Value = Some ("some other text" :> obj)
                        DefaultValueFactory = ValueNone
                    };
                    Delta.AttrDelta.Property {
                        Accessor = Accessor.AvaloniaProperty TextBlock.FontStyleProperty
                        Value = None
                        DefaultValueFactory = ValueNone
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
                ViewType = typeof<TextBlock>
                Attrs = [
                    Delta.AttrDelta.Property {
                        Accessor = Accessor.AvaloniaProperty TextBlock.TextProperty
                        Value = Some ("some other text" :> obj)
                        DefaultValueFactory = ValueNone
                    }
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
                    ViewType = typeof<Button>
                    Attrs = [
                        Delta.AttrDelta.Property {
                            Accessor = Accessor.AvaloniaProperty Button.BackgroundProperty
                            Value = Some ((Avalonia.Media.SolidColorBrush.Parse("red").ToImmutable()) :> obj)
                            DefaultValueFactory = ValueNone
                        };
                        Delta.AttrDelta.Content {
                            Accessor = Accessor.AvaloniaProperty Button.ContentProperty
                            Content = Delta.ViewContentDelta.Single
                                ( Some {
                                    ViewType = typeof<TextBlock>
                                    Attrs = [
                                        Delta.AttrDelta.Property {
                                            Accessor = Accessor.AvaloniaProperty TextBlock.TextProperty
                                            Value = Some ("some other text" :> obj)
                                            DefaultValueFactory = ValueNone
                                        };
                                        Delta.AttrDelta.Property {
                                            Accessor = Accessor.AvaloniaProperty TextBlock.FontSizeProperty
                                            Value = Some (15.0 :> obj)
                                            DefaultValueFactory = ValueNone
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
                    ViewType = typeof<StackPanel>
                    Attrs = [
                        Delta.AttrDelta.Property {
                            Accessor =  Accessor.AvaloniaProperty StackPanel.OrientationProperty
                            Value = Some (Orientation.Vertical :> obj)
                            DefaultValueFactory = ValueNone
                        }
                        Delta.AttrDelta.Content {
                            Accessor = Accessor.InstanceProperty { Name = "Children"; Setter = ValueNone; Getter = ValueNone }
                            Content = Delta.ViewContentDelta.Multiple [
                                {
                                    ViewType = typeof<TextBlock>
                                    Attrs = [
                                        Delta.AttrDelta.Property {
                                            Accessor = Accessor.AvaloniaProperty TextBlock.TextProperty
                                            Value = Some ("some other text" :> obj)
                                            DefaultValueFactory = ValueNone
                                        }
                                        Delta.AttrDelta.Property {
                                            Accessor = Accessor.AvaloniaProperty TextBlock.FontSizeProperty
                                            Value = Some (15.0 :> obj)
                                            DefaultValueFactory = ValueNone
                                        }
                                    ]
                                };
                                {
                                    ViewType = typeof<Button>
                                    Attrs = [
                                        Delta.AttrDelta.Property {
                                            Accessor = Accessor.AvaloniaProperty Button.BackgroundProperty
                                            Value = Some ((Avalonia.Media.SolidColorBrush.Parse("red").ToImmutable()) :> obj)
                                            DefaultValueFactory = ValueNone
                                        }
                                    ]
                                }
                            ]
                        }
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
                    ViewType = typeof<StackPanel>
                    Attrs = [
                        Delta.AttrDelta.Property {
                            Accessor = Accessor.AvaloniaProperty StackPanel.OrientationProperty
                            Value = Some (Orientation.Vertical :> obj)
                            DefaultValueFactory = ValueNone
                        };
                        Delta.AttrDelta.Content {
                            Accessor = Accessor.InstanceProperty { Name = "Children"; Setter = ValueNone; Getter = ValueNone }
                            Content = Delta.ViewContentDelta.Multiple [
                                {
                                    ViewType = typeof<CheckBox>
                                    Attrs = [
                                        Delta.AttrDelta.Property {
                                            Accessor = Accessor.AvaloniaProperty CheckBox.ContentProperty
                                            Value = Some ("some text [new]" :> obj)
                                            DefaultValueFactory = ValueNone
                                        };
                                        Delta.AttrDelta.Property {
                                            Accessor = Accessor.AvaloniaProperty CheckBox.IsCheckedProperty
                                            Value = Some (false :> obj)
                                            DefaultValueFactory = ValueNone
                                        };
                                    ]
                                };
                                {
                                    ViewType = typeof<CheckBox>
                                    Attrs = [
                                        Delta.AttrDelta.Property {
                                            Accessor = Accessor.AvaloniaProperty CheckBox.ContentProperty
                                            Value = Some ("some text 1" :> obj)
                                            DefaultValueFactory = ValueNone
                                        };
                                        Delta.AttrDelta.Property {
                                            Accessor = Accessor.AvaloniaProperty CheckBox.IsCheckedProperty
                                            Value = Some (true :> obj)
                                            DefaultValueFactory = ValueNone
                                        };
                                    ]
                                };
                                {
                                    ViewType = typeof<CheckBox>
                                    Attrs = [
                                        Delta.AttrDelta.Property {
                                            Accessor = Accessor.AvaloniaProperty CheckBox.ContentProperty
                                            Value = Some ("some text 2" :> obj)
                                            DefaultValueFactory = ValueNone
                                        };
                                        Delta.AttrDelta.Property {
                                            Accessor = Accessor.AvaloniaProperty CheckBox.IsCheckedProperty
                                            Value = Some (false :> obj)
                                            DefaultValueFactory = ValueNone
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
                        CheckBox.content "some text 1"
                        CheckBox.isChecked true
                    ]
                    CheckBox.create [
                        CheckBox.content "some text 2"
                        CheckBox.isChecked false
                    ]
                    CheckBox.create [
                        CheckBox.content "some text [new]"
                        CheckBox.isChecked true
                    ]
                ]
            ]

        let delta : Delta.ViewDelta = 
            {
                ViewType = typeof<StackPanel>
                Attrs = [
                    Delta.AttrDelta.Property {
                        Accessor = Accessor.AvaloniaProperty StackPanel.OrientationProperty
                        Value = Some (Orientation.Vertical :> obj)
                        DefaultValueFactory = ValueNone
                    };
                    Delta.AttrDelta.Content {
                        Accessor = Accessor.InstanceProperty { Name = "Children"; Setter = ValueNone; Getter = ValueNone }
                        Content = Delta.ViewContentDelta.Multiple [
                            {
                                ViewType = typeof<CheckBox>
                                Attrs = [  ]
                            };                             
                            {
                                ViewType = typeof<CheckBox>
                                Attrs = [ ]
                            };                         
                            {
                                ViewType = typeof<CheckBox>
                                Attrs = [
                                    Delta.AttrDelta.Property {
                                        Accessor = Accessor.AvaloniaProperty CheckBox.ContentProperty
                                        Value = Some ("some text [new]" :> obj)
                                        DefaultValueFactory = ValueNone
                                    };
                                    Delta.AttrDelta.Property {
                                        Accessor = Accessor.AvaloniaProperty CheckBox.IsCheckedProperty
                                        Value = Some (true :> obj)
                                        DefaultValueFactory = ValueNone
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