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
                viewType = typeof<TextBlock>
                attrs = [
                    Delta.AttrDelta.Property {
                        accessor = Accessor.AvaloniaProperty TextBlock.TextProperty
                        value = Some ("some other text" :> obj)
                        defaultValueFactory = ValueNone
                    };
                    Delta.AttrDelta.Property {
                        accessor = Accessor.AvaloniaProperty TextBlock.FontStyleProperty
                        value = None
                        defaultValueFactory = ValueNone
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
                        accessor = Accessor.AvaloniaProperty TextBlock.TextProperty
                        value = Some ("some other text" :> obj)
                        defaultValueFactory = ValueNone
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
                    viewType = typeof<Button>
                    attrs = [
                        Delta.AttrDelta.Property {
                            accessor = Accessor.AvaloniaProperty Button.BackgroundProperty
                            value = Some ((Avalonia.Media.SolidColorBrush.Parse("red").ToImmutable()) :> obj)
                            defaultValueFactory = ValueNone
                        };
                        Delta.AttrDelta.Content {
                            accessor = Accessor.AvaloniaProperty Button.ContentProperty
                            content = Delta.ViewContentDelta.Single
                                ( Some {
                                    viewType = typeof<TextBlock>
                                    attrs = [
                                        Delta.AttrDelta.Property {
                                            accessor = Accessor.AvaloniaProperty TextBlock.TextProperty
                                            value = Some ("some other text" :> obj)
                                            defaultValueFactory = ValueNone
                                        };
                                        Delta.AttrDelta.Property {
                                            accessor = Accessor.AvaloniaProperty TextBlock.FontSizeProperty
                                            value = Some (15.0 :> obj)
                                            defaultValueFactory = ValueNone
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
                            accessor =  Accessor.AvaloniaProperty StackPanel.OrientationProperty
                            value = Some (Orientation.Vertical :> obj)
                            defaultValueFactory = ValueNone
                        }
                        Delta.AttrDelta.Content {
                            accessor = Accessor.InstanceProperty { name = "Children"; setter = ValueNone; getter = ValueNone }
                            content = Delta.ViewContentDelta.Multiple [
                                {
                                    viewType = typeof<TextBlock>
                                    attrs = [
                                        Delta.AttrDelta.Property {
                                            accessor = Accessor.AvaloniaProperty TextBlock.TextProperty
                                            value = Some ("some other text" :> obj)
                                            defaultValueFactory = ValueNone
                                        }
                                        Delta.AttrDelta.Property {
                                            accessor = Accessor.AvaloniaProperty TextBlock.FontSizeProperty
                                            value = Some (15.0 :> obj)
                                            defaultValueFactory = ValueNone
                                        }
                                    ]
                                };
                                {
                                    viewType = typeof<Button>
                                    attrs = [
                                        Delta.AttrDelta.Property {
                                            accessor = Accessor.AvaloniaProperty Button.BackgroundProperty
                                            value = Some ((Avalonia.Media.SolidColorBrush.Parse("red").ToImmutable()) :> obj)
                                            defaultValueFactory = ValueNone
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
                    viewType = typeof<StackPanel>
                    attrs = [
                        Delta.AttrDelta.Property {
                            accessor = Accessor.AvaloniaProperty StackPanel.OrientationProperty
                            value = Some (Orientation.Vertical :> obj)
                            defaultValueFactory = ValueNone
                        };
                        Delta.AttrDelta.Content {
                            accessor = Accessor.InstanceProperty { name = "Children"; setter = ValueNone; getter = ValueNone }
                            content = Delta.ViewContentDelta.Multiple [
                                {
                                    viewType = typeof<CheckBox>
                                    attrs = [
                                        Delta.AttrDelta.Property {
                                            accessor = Accessor.AvaloniaProperty CheckBox.ContentProperty
                                            value = Some ("some text [new]" :> obj)
                                            defaultValueFactory = ValueNone
                                        };
                                        Delta.AttrDelta.Property {
                                            accessor = Accessor.AvaloniaProperty CheckBox.IsCheckedProperty
                                            value = Some (false :> obj)
                                            defaultValueFactory = ValueNone
                                        };
                                    ]
                                };
                                {
                                    viewType = typeof<CheckBox>
                                    attrs = [
                                        Delta.AttrDelta.Property {
                                            accessor = Accessor.AvaloniaProperty CheckBox.ContentProperty
                                            value = Some ("some text 1" :> obj)
                                            defaultValueFactory = ValueNone
                                        };
                                        Delta.AttrDelta.Property {
                                            accessor = Accessor.AvaloniaProperty CheckBox.IsCheckedProperty
                                            value = Some (true :> obj)
                                            defaultValueFactory = ValueNone
                                        };
                                    ]
                                };
                                {
                                    viewType = typeof<CheckBox>
                                    attrs = [
                                        Delta.AttrDelta.Property {
                                            accessor = Accessor.AvaloniaProperty CheckBox.ContentProperty
                                            value = Some ("some text 2" :> obj)
                                            defaultValueFactory = ValueNone
                                        };
                                        Delta.AttrDelta.Property {
                                            accessor = Accessor.AvaloniaProperty CheckBox.IsCheckedProperty
                                            value = Some (false :> obj)
                                            defaultValueFactory = ValueNone
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
                viewType = typeof<StackPanel>
                attrs = [
                    Delta.AttrDelta.Property {
                        accessor = Accessor.AvaloniaProperty StackPanel.OrientationProperty
                        value = Some (Orientation.Vertical :> obj)
                        defaultValueFactory = ValueNone
                    };
                    Delta.AttrDelta.Content {
                        accessor = Accessor.InstanceProperty { name = "Children"; setter = ValueNone; getter = ValueNone }
                        content = Delta.ViewContentDelta.Multiple [
                            {
                                viewType = typeof<CheckBox>
                                attrs = [  ]
                            };                             
                            {
                                viewType = typeof<CheckBox>
                                attrs = [ ]
                            };                         
                            {
                                viewType = typeof<CheckBox>
                                attrs = [
                                    Delta.AttrDelta.Property {
                                        accessor = Accessor.AvaloniaProperty CheckBox.ContentProperty
                                        value = Some ("some text [new]" :> obj)
                                        defaultValueFactory = ValueNone
                                    };
                                    Delta.AttrDelta.Property {
                                        accessor = Accessor.AvaloniaProperty CheckBox.IsCheckedProperty
                                        value = Some (true :> obj)
                                        defaultValueFactory = ValueNone
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