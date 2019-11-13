namespace Avalonia.FuncUI.UnitTests.VirtualDom

module PatcherTests =
    open Avalonia.FuncUI.VirtualDom
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Types
    open Avalonia.Controls
    open Xunit
    open Avalonia.Media
    
    [<Fact>]
    let ``Patch Properties`` () =

        let delta : Delta.ViewDelta = 
            {
                viewType = typeof<TextBlock>
                attrs = [
                    Delta.AttrDelta.Property {
                        accessor = Accessor.AvaloniaProperty TextBlock.TextProperty
                        value = Some ("some text" :> obj)
                    };
                    Delta.AttrDelta.Property {
                        accessor = Accessor.AvaloniaProperty TextBlock.FontSizeProperty
                        value = Some (14.0 :> obj)
                    };
                ]
            }

        let control = TextBlock()

        Patcher.patch (control, delta)

        Assert.Equal("some text", control.Text)
        Assert.Equal(14.0, control.FontSize)

    [<Fact>]
    let ``Patch Content Single`` () =

        let delta : Delta.ViewDelta = 
             {
                 viewType = typeof<Button>
                 attrs = [
                     Delta.AttrDelta.Content {
                         accessor = Accessor.AvaloniaProperty Button.ContentProperty
                         content = Delta.ViewContentDelta.Single
                             ( Some {
                                 viewType = typeof<TextBlock>
                                 attrs = [
                                     Delta.AttrDelta.Property {
                                         accessor = Accessor.AvaloniaProperty TextBlock.TextProperty
                                         value = Some ("some text" :> obj)
                                     };
                                     Delta.AttrDelta.Property {
                                         accessor = Accessor.AvaloniaProperty TextBlock.FontSizeProperty
                                         value = Some (15.0 :> obj)
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
        let delta : Delta.ViewDelta = 
            {
                viewType = typeof<StackPanel>
                attrs = [
                    Delta.AttrDelta.Content {
                        accessor = Accessor.InstanceProperty {
                            name = "Children"
                            getter = ValueSome (fun control -> (control :?> StackPanel).Children :> obj)
                            setter = ValueNone
                        }
                        content = Delta.ViewContentDelta.Multiple [
                            {
                                viewType = typeof<TextBlock>
                                attrs = [
                                    Delta.AttrDelta.Property {
                                        accessor = Accessor.AvaloniaProperty TextBlock.TextProperty
                                        value = Some ("some text" :> obj)
                                    };
                                    Delta.AttrDelta.Property {
                                        accessor = Accessor.AvaloniaProperty TextBlock.FontSizeProperty
                                        value = Some (15.0 :> obj)
                                    };
                                ]
                            };
                            {
                                viewType = typeof<Button>
                                attrs = [
                                    Delta.AttrDelta.Property {
                                        accessor = Accessor.AvaloniaProperty Button.BackgroundProperty
                                        value = Some ((SolidColorBrush.Parse("red").ToImmutable()) :> obj)
                                    };
                                ]
                            };
                            {
                                viewType = typeof<Button>
                                attrs = [
                                    Delta.AttrDelta.Property {
                                        accessor = Accessor.AvaloniaProperty Button.BackgroundProperty
                                        value = Some ((SolidColorBrush.Parse("green").ToImmutable()) :> obj)
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
