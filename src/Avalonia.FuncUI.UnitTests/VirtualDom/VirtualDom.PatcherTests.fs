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
                ViewType = typeof<TextBlock>
                KeyDidChange = false
                ConstructorArgs = null
                Attrs = [
                    Delta.AttrDelta.Property {
                        Accessor = Accessor.AvaloniaProperty TextBlock.TextProperty
                        Value = Some ("some text" :> obj)
                        DefaultValueFactory = ValueNone
                    };
                    Delta.AttrDelta.Property {
                        Accessor = Accessor.AvaloniaProperty TextBlock.FontSizeProperty
                        Value = Some (14.0 :> obj)
                        DefaultValueFactory = ValueNone
                    };
                ]
                Outlet = ValueNone
            }

        let control = TextBlock()

        Patcher.patch (control, delta)

        Assert.Equal("some text", control.Text)
        Assert.Equal(14.0, control.FontSize)

    [<Fact>]
    let ``Patch Styles, Classes or Resources`` () =
        let stylesGetter: IAvaloniaObject -> obj = (fun c -> (c :?> StyledElement).Styles :> obj)
        let stylesSetter: IAvaloniaObject * obj -> unit =
            (fun (c, v) ->
                let se = (c :?> StyledElement)
                let s = v :?> Styles
                se.Styles.Clear()
                se.Styles.AddRange(s))

        let classesGetter: IAvaloniaObject -> obj = (fun c -> (c :?> StyledElement).Classes :> obj)
        let classesSetter: IAvaloniaObject * obj -> unit = (fun (c, v) -> (c :?> StyledElement).Classes <- v :?> Classes)

        let resourcesGetter: IAvaloniaObject -> obj = (fun c -> (c :?> StyledElement).Resources :> obj)
        let resourcesSetter: IAvaloniaObject * obj -> unit = (fun (c, v) -> (c :?> StyledElement).Resources <- v :?> IResourceDictionary)

        let delta : Delta.ViewDelta =
            {
                ViewType = typeof<TextBlock>
                KeyDidChange = false
                ConstructorArgs = null
                Attrs = [
                    Delta.AttrDelta.Property {
                        Accessor = Accessor.InstanceProperty {
                            Name = "Styles"
                            Getter = stylesGetter |> ValueSome
                            Setter = stylesSetter |> ValueSome
                        }
                        Value = None
                        DefaultValueFactory = (fun () -> Styles() :> obj) |> ValueSome
                    }
                    Delta.AttrDelta.Property {
                        Accessor = Accessor.InstanceProperty {
                            Name = "Classes"
                            Getter = classesGetter |> ValueSome
                            Setter = classesSetter |> ValueSome
                        }
                        Value = None
                        DefaultValueFactory = (fun () -> Classes() :> obj) |> ValueSome
                    }
                    Delta.AttrDelta.Property {
                        Accessor = Accessor.InstanceProperty {
                            Name = "Resources"
                            Getter = resourcesGetter |> ValueSome
                            Setter = resourcesSetter |> ValueSome
                        }
                        Value = None
                        DefaultValueFactory = (fun () -> ResourceDictionary() :> obj) |> ValueSome
                    }
                ]
                Outlet = ValueNone
            }

        let control = TextBlock()
        control.Styles.Add(Style())
        control.Classes <- Classes([| "class" |])
        control.Resources.Add(KeyValuePair.Create("key" :> obj, "Value" :> obj))

        Patcher.patch (control, delta)

        Assert.Equal(0, control.Styles.Count)
        Assert.Equal(0, control.Classes.Count)
        Assert.Equal(0, control.Resources.Count)

    [<Fact>]
    let ``Patch Content Single`` () =

        let delta : Delta.ViewDelta =
             {
                 ViewType = typeof<Button>
                 KeyDidChange = false
                 ConstructorArgs = null
                 Attrs = [
                     Delta.AttrDelta.Content {
                         Accessor = Accessor.AvaloniaProperty Button.ContentProperty
                         Content = Delta.ViewContentDelta.Single
                             ( Some {
                                 ViewType = typeof<TextBlock>
                                 KeyDidChange = false
                                 ConstructorArgs = null
                                 Attrs = [
                                     Delta.AttrDelta.Property {
                                         Accessor = Accessor.AvaloniaProperty TextBlock.TextProperty
                                         Value = Some ("some text" :> obj)
                                         DefaultValueFactory = ValueNone
                                     };
                                     Delta.AttrDelta.Property {
                                         Accessor = Accessor.AvaloniaProperty TextBlock.FontSizeProperty
                                         Value = Some (15.0 :> obj)
                                         DefaultValueFactory = ValueNone
                                     };
                                 ]
                                 Outlet = ValueNone
                             }
                         )
                     };
                 ]
                 Outlet = ValueNone
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
                ViewType = typeof<StackPanel>
                KeyDidChange = false
                ConstructorArgs = null
                Attrs = [
                    Delta.AttrDelta.Content {
                        Accessor = Accessor.InstanceProperty {
                            Name = "Children"
                            Getter = ValueSome (fun control -> (control :?> StackPanel).Children :> obj)
                            Setter = ValueNone
                        }
                        Content = Delta.ViewContentDelta.Multiple [
                            {
                                ViewType = typeof<TextBlock>
                                KeyDidChange = false
                                ConstructorArgs = null
                                Attrs = [
                                    Delta.AttrDelta.Property {
                                        Accessor = Accessor.AvaloniaProperty TextBlock.TextProperty
                                        Value = Some ("some text" :> obj)
                                        DefaultValueFactory = ValueNone
                                    };
                                    Delta.AttrDelta.Property {
                                        Accessor = Accessor.AvaloniaProperty TextBlock.FontSizeProperty
                                        Value = Some (15.0 :> obj)
                                        DefaultValueFactory = ValueNone
                                    };
                                ]
                                Outlet = ValueNone
                            };
                            {
                                ViewType = typeof<Button>
                                KeyDidChange = false
                                ConstructorArgs = null
                                Attrs = [
                                    Delta.AttrDelta.Property {
                                        Accessor = Accessor.AvaloniaProperty Button.BackgroundProperty
                                        Value = Some ((SolidColorBrush.Parse("red").ToImmutable()) :> obj)
                                        DefaultValueFactory = ValueNone
                                    };
                                ]
                                Outlet = ValueNone
                            };
                            {
                                ViewType = typeof<Button>
                                KeyDidChange = false
                                ConstructorArgs = null
                                Attrs = [
                                    Delta.AttrDelta.Property {
                                        Accessor = Accessor.AvaloniaProperty Button.BackgroundProperty
                                        Value = Some ((SolidColorBrush.Parse("green").ToImmutable()) :> obj)
                                        DefaultValueFactory = ValueNone
                                    };
                                ]
                                Outlet = ValueNone
                            };
                        ]
                    };
                ]
                Outlet = ValueNone
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
