namespace Avalonia.FuncUI.Diagnostics

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.DSL
open Avalonia.Media

[<AbstractClass; Sealed>]
type IconView =

    static member create (drawing: Drawing, size: int) : IView =
        Component.create ($"icon_{drawing.GetHashCode()}", fun ctx ->
            let vectorImage = DrawingImage(drawing)

            Image.create [
                Image.source (vectorImage :> IImage)
                Image.width (float size)
                Image.height (float size)
            ]
            :> IView
        ) :> IView

type Icons () =

    static member openInNewWindow: Drawing =
        let geo =
            StreamGeometry.Parse
                """
                M 40.960938 4.9804688 A 2.0002 2.0002 0 0 0 40.740234 5 L 28 5 A 2.0002 2.0002 0 1 0 28 9 L 36.171875 9 L 22.585938 22.585938 A 2.0002 2.0002 0 1 0 25.414062 25.414062 L 39 11.828125 L 39 20 A 2.0002 2.0002 0 1 0 43 20 L 43 7.2460938 A 2.0002 2.0002 0 0 0 40.960938 4.9804688 z M 12.5 8 C 8.3826878 8 5 11.382688 5 15.5 L 5 35.5 C 5 39.617312 8.3826878 43 12.5 43 L 32.5 43 C 36.617312 43 40 39.617312 40 35.5 L 40 26 A 2.0002 2.0002 0 1 0 36 26 L 36 35.5 C 36 37.446688 34.446688 39 32.5 39 L 12.5 39 C 10.553312 39 9 37.446688 9 35.5 L 9 15.5 C 9 13.553312 10.553312 12 12.5 12 L 22 12 A 2.0002 2.0002 0 1 0 22 8 L 12.5 8 z
                """

        GeometryDrawing(Geometry = geo, Pen = Pen(Brushes.White, 0.5), Brush = Brushes.White) :> _