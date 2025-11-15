namespace Avalonia.FuncUI.UnitTests.VirtualDom

open System
open System.Threading
open System.Collections.Generic
open Avalonia
open Avalonia.Controls
open Avalonia.Media
open Avalonia.Styling

module PatcherTests =
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.VirtualDom
    open Avalonia.Headless.XUnit
    open Xunit

    [<AvaloniaFact>]
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

    [<AvaloniaFact>]
    let ``Patch Styles, Classes or Resources`` () =
        let stylesGetter: AvaloniaObject -> obj = (fun c -> (c :?> StyledElement).Styles :> obj)
        let stylesSetter: AvaloniaObject * obj -> unit =
            (fun (c, v) ->
                let se = (c :?> StyledElement)
                let s = v :?> Styles
                se.Styles.Clear()
                se.Styles.AddRange(s))

        let classesGetter: AvaloniaObject -> obj = (fun c -> (c :?> StyledElement).Classes :> obj)
        let classesSetter: AvaloniaObject * obj -> unit = (fun (c, v) -> 
            let element = (c :?> StyledElement)
            element.Classes.Clear()
            element.Classes.AddRange(v :?> Classes))
      
        let resourcesGetter: AvaloniaObject -> obj = (fun c -> (c :?> StyledElement).Resources :> obj)
        let resourcesSetter: AvaloniaObject * obj -> unit = (fun (c, v) -> (c :?> StyledElement).Resources <- v :?> IResourceDictionary)

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
        control.Classes.Add("class")
        control.Resources.Add(KeyValuePair.Create("key" :> obj, "Value" :> obj))

        Patcher.patch (control, delta)

        Assert.Equal(0, control.Styles.Count)
        Assert.Equal(0, control.Classes.Count)
        Assert.Equal(0, control.Resources.Count)

    [<AvaloniaFact>]
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

    [<AvaloniaFact>]
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
        Assert.Equal(SolidColorBrush.Parse("red").ToImmutable() :> IBrush, button.Background)

        Assert.IsType(typeof<Button>,  stackpanel.Children.[2])
        let button = stackpanel.Children.[2] :?> Button
        Assert.Equal(SolidColorBrush.Parse("green").ToImmutable() :> IBrush, button.Background)

    [<AvaloniaFact>]
    let ``Patch Custom Subscription`` () =
        /// Capture list for factory called.
        let factoryCaptures = ResizeArray()
        /// Capture list for token cancellation called.
        let tokenCancelledCaptures = ResizeArray()

        /// Custom subscription binding function for testing common pattern of subscribing to .NET Event in FuncUI.
        let onTextChanging (func, subPatchOptions) =
            let name = "Test_TextChanged"

            /// Custom subscription factory for `TextBox.TextChanging`.
            let factory: AvaloniaObject * ('t * TextChangingEventArgs -> unit) * CancellationToken -> unit when 't :> TextBox =
                fun (control, func, token) ->
                    // When factory is called, capture the subPatchOptions.
                    factoryCaptures.Add subPatchOptions

                    let control = control :?> 't
                    let handler = EventHandler<TextChangingEventArgs>(fun s e -> func(s :?> 't, e))
                    let event = control.TextChanging
                    event.AddHandler(handler)

                    // Register unsubscribe action to token.
                    token.Register(fun _ ->
                        // When token.Cancel is called, capture the subPatchOptions.
                        tokenCancelledCaptures.Add subPatchOptions
                        event.RemoveHandler(handler)
                    ) |> ignore

            AttrBuilder<'t>.CreateSubscription<'t * TextChangingEventArgs>(name, factory, func, subPatchOptions)

        /// Capture list for text changing event.
        let textChangingCaptures = ResizeArray()

        /// testing view function for TextBox.
        /// 
        /// Control is updated by `IAttr<'t> list` in order. This test is to confirm the behavior of 
        /// event subscription around this specification.
        let view text subPatchStr =
            TextBox.create [
                TextBox.text text
                onTextChanging (
                    (fun (tb, e) -> textChangingCaptures.Add $"{subPatchStr}-{tb.Text}"),
                    OnChangeOf subPatchStr
                )
            ]
        
        /// initial view definition. Only set text value.
        let initView = TextBox.create [ TextBox.text "Foo" ]
        /// 1st update view definition. Add event subscription.
        let updatedView = view "Foo" "FirstSubPatch"
        /// 2nd update view definition. Only update text value.
        let updatedView' = view "Hoge" "FirstSubPatch"
        /// 3rd update view definition. Update text value and subscription subPatch.
        let updatedView'' = view "Bar" "SecondSubPatch"
        /// 4th update view definition. Only update text value.
        let updatedView''' = view "Fuga" "SecondSubPatch"

        /// create target control.
        let target =  VirtualDom.create initView :?> TextBox

        // 1st update.
        VirtualDom.update(target, initView, updatedView)
        // Check text value.
        Assert.Equal("Foo", target.Text)
        // Check event subscription.
        Assert.Equal(1, factoryCaptures.Count)
        Assert.Equal(OnChangeOf "FirstSubPatch", factoryCaptures[0])
        // No token cancellation.
        Assert.Empty tokenCancelledCaptures
        // When text has not changed, event is not fired.
        Assert.Empty textChangingCaptures

        // 2nd update.
        VirtualDom.update(target, updatedView, updatedView')
        // Text value is updated.
        Assert.Equal("Hoge", target.Text)
        // Subscription is not updated.
        Assert.Equal(1, factoryCaptures.Count)
        // No token cancellation.
        Assert.Empty tokenCancelledCaptures
        // Check event fired.
        Assert.Equal(1, textChangingCaptures.Count)
        Assert.Equal("FirstSubPatch-Hoge", textChangingCaptures[0])

        // 3rd update.
        VirtualDom.update(target, updatedView', updatedView'')
        // Text value is updated.
        Assert.Equal("Bar", target.Text)
        // Subscription is updated.
        Assert.Equal(2, factoryCaptures.Count)
        Assert.Equal(OnChangeOf "SecondSubPatch", factoryCaptures.[1])
        // Old callback is unsubscribed.
        Assert.Equal(1, tokenCancelledCaptures.Count)
        Assert.Equal(OnChangeOf "FirstSubPatch", tokenCancelledCaptures.[0])
        // Check event fired. Text value update faster than subscription update, so old callback is called.
        Assert.Equal(2, textChangingCaptures.Count)
        Assert.Equal("FirstSubPatch-Bar", textChangingCaptures.[1])

        // 4th update.
        VirtualDom.update(target, updatedView'', updatedView''')
        // Text value is updated.
        Assert.Equal("Fuga", target.Text)
        // Subscription is not updated.
        Assert.Equal(2, factoryCaptures.Count)
        // subscription is not cancelled.
        Assert.Equal(1, tokenCancelledCaptures.Count)
        // Check event fired. New callback is called.
        Assert.Equal(3, textChangingCaptures.Count)
        Assert.Equal("SecondSubPatch-Fuga", textChangingCaptures.[2])
