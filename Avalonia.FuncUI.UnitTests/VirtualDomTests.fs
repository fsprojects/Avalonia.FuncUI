namespace Avalonia.FuncUI.UnitTests

open Xunit
open System


module VirtualDomTests =

    module DifferTests = 
        open Avalonia.FuncUI.VirtualDom
        open Avalonia.FuncUI.VirtualDom.Delta
        open Avalonia.FuncUI
        open Avalonia.Controls

        module AttrDifferTests = 

            [<Fact>]
            let ``Diff`` () =
                let last =
                    Views.stackPanel [
                        Attrs.orientation Orientation.Horizontal
                        Attrs.children [
                            Views.textblock [
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
                            Views.textblock [
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