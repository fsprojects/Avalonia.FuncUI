namespace Avalonia.FuncUI.UnitTests.VirtualDom

open System.Collections.Generic
open Avalonia
open Avalonia.Styling

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
                        defaultValueFactory = ValueNone
                    };
                    Delta.AttrDelta.Property {
                        accessor = Accessor.AvaloniaProperty TextBlock.FontSizeProperty
                        value = Some (14.0 :> obj)
                        defaultValueFactory = ValueNone
                    };
                ]
            }

        let control = TextBlock()

        Patcher.patch (control, delta)

        Assert.Equal("some text", control.Text)
        Assert.Equal(14.0, control.FontSize)
        
    [<Fact>]
    let ``Patch Styles, Classes or Resources`` () =
        let stylesGetter: IControl -> obj = (fun c -> (c :?> StyledElement).Styles :> obj)
        let stylesSetter: IControl * obj -> unit = (fun (c, v) -> (c :?> StyledElement).Styles <- v :?> Styles)

        let classesGetter: IControl -> obj = (fun c -> (c :?> StyledElement).Classes :> obj)
        let classesSetter: IControl * obj -> unit = (fun (c, v) -> (c :?> StyledElement).Classes <- v :?> Classes)

        let resourcesGetter: IControl -> obj = (fun c -> (c :?> StyledElement).Resources :> obj)
        let resourcesSetter: IControl * obj -> unit = (fun (c, v) -> (c :?> StyledElement).Resources <- v :?> IResourceDictionary)

        let delta : Delta.ViewDelta = 
            {
                viewType = typeof<TextBlock>
                attrs = [
                    Delta.AttrDelta.Property {
                        accessor = Accessor.InstanceProperty {
                            name = "Styles"
                            getter = stylesGetter |> ValueSome
                            setter = stylesSetter |> ValueSome
                        }
                        value = None
                        defaultValueFactory = (fun () -> Styles() :> obj) |> ValueSome
                    }
                    Delta.AttrDelta.Property {
                        accessor = Accessor.InstanceProperty {
                            name = "Classes"
                            getter = classesGetter |> ValueSome
                            setter = classesSetter |> ValueSome
                        }
                        value = None
                        defaultValueFactory = (fun () -> Classes() :> obj) |> ValueSome
                    }
                    Delta.AttrDelta.Property {
                        accessor = Accessor.InstanceProperty {
                            name = "Resources"
                            getter = resourcesGetter |> ValueSome
                            setter = resourcesSetter |> ValueSome
                        }
                        value = None
                        defaultValueFactory = (fun () -> ResourceDictionary() :> obj) |> ValueSome
                    }
                ]
            }

        let control = TextBlock()
        control.Styles.Add(Style())
        control.Classes <- Classes([| "class" |])
        control.Resources.Add(KeyValuePair.Create("key" :> obj, "value" :> obj))

        Patcher.patch (control, delta)

        Assert.Equal(0, control.Styles.Count)
        Assert.Equal(0, control.Classes.Count)
        Assert.Equal(0, control.Resources.Count)

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
                                        defaultValueFactory = ValueNone
                                    };
                                    Delta.AttrDelta.Property {
                                        accessor = Accessor.AvaloniaProperty TextBlock.FontSizeProperty
                                        value = Some (15.0 :> obj)
                                        defaultValueFactory = ValueNone
                                    };
                                ]
                            };
                            {
                                viewType = typeof<Button>
                                attrs = [
                                    Delta.AttrDelta.Property {
                                        accessor = Accessor.AvaloniaProperty Button.BackgroundProperty
                                        value = Some ((SolidColorBrush.Parse("red").ToImmutable()) :> obj)
                                        defaultValueFactory = ValueNone
                                    };
                                ]
                            };
                            {
                                viewType = typeof<Button>
                                attrs = [
                                    Delta.AttrDelta.Property {
                                        accessor = Accessor.AvaloniaProperty Button.BackgroundProperty
                                        value = Some ((SolidColorBrush.Parse("green").ToImmutable()) :> obj)
                                        defaultValueFactory = ValueNone
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
