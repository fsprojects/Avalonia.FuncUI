namespace Avalonia.FuncUI.Experimental

open System
open System.Runtime.CompilerServices
open Avalonia
open Avalonia.Animation
open Avalonia.Animation.Easings
open Avalonia.Styling

[<Extension>]
type AnimationBuilderExtensions () =

    [<Extension>]
    static member WithDuration (animation: Animation, s: float) =
        animation.Duration <- TimeSpan.FromSeconds s
        animation

    [<Extension>]
    static member WithFillMode (animation: Animation, fillMode: FillMode) =
        animation.FillMode <- fillMode
        animation

    [<Extension>]
    static member WithEasing (animation: Animation, easing: Easing) =
        animation.Easing <- easing
        animation

    [<Extension>]
    static member WithKeyFrame (animation: Animation, keyFrame: KeyFrame) =
        animation.Children.Add keyFrame
        animation

    [<Extension>]
    static member WithKeyFrames (animation: Animation, keyFrames: KeyFrame list) =
        animation.Children.AddRange keyFrames
        animation

[<Extension>]
type KeyFrameBuilderExtensions =

    [<Extension>]
    static member WithCue (keyFrame: KeyFrame, cue: float) =
        keyFrame.Cue <- Cue(cue)
        keyFrame

    [<Extension>]
    static member WithKeyTime (keyFrame: KeyFrame, t: float) =
        keyFrame.KeyTime <- TimeSpan.FromSeconds t
        keyFrame

    [<Extension>]
    static member WithSetter (keyFrame: KeyFrame, setter: Setter) =
        keyFrame.Setters.Add setter
        keyFrame

    [<Extension>]
    static member WithSetter (keyFrame: KeyFrame, property: AvaloniaProperty, value: obj) =
        keyFrame.Setters.Add (Setter(property, value))
        keyFrame
