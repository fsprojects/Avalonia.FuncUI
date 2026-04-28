namespace Examples.Canvas

module View =
    open Avalonia.Controls
    open Avalonia.Controls.Shapes
    open Avalonia.FuncUI
    open Avalonia.FuncUI.DSL

    // Select Rectangle Demo
    //
    // This demo illustrates basic canvas interactivity with the "Tapped" Pointer event.

    let rectangleSelectView id =
        Component.create(id, fun ctx ->
            let state = ctx.useState None
            Canvas.create [
                Canvas.background "white"
                Canvas.children [
                    Rectangle.create [
                        Canvas.left 60
                        Canvas.top 40
                        Rectangle.width 200
                        Rectangle.height 40
                        Rectangle.fill (if state.Current = Some 0 then "greenyellow" else "blue")
                        Rectangle.onTapped (fun _ -> state.Set (Some 0))
                    ]
                    Rectangle.create [
                        Canvas.left 140
                        Canvas.top 120
                        Rectangle.width 160
                        Rectangle.height 60
                        Rectangle.fill (if state.Current = Some 1 then "greenyellow" else "blue")
                        Rectangle.onTapped (fun _ -> state.Set (Some 1))
                    ]
                    Rectangle.create [
                        Canvas.left 80
                        Canvas.top 240
                        Rectangle.width 100
                        Rectangle.height 120
                        Rectangle.fill (if state.Current = Some 2 then "greenyellow" else "blue")
                        Rectangle.onTapped (fun _ -> state.Set (Some 2))
                    ]
                ]
            ]
        )

    // Drag Rectangle Demo
    //
    // This demo illustrates a less trivial kind of canvas interactivity with more Pointer events.
    // It demonstrates two important constructs:
    //
    // * SubPatchOptions to make sure stale data isn't stuck in event handlers' captured variables.
    // * ctx.control to retrieve the current control for Avalonia's PointerEventArgs

    type RectData = {
        x: float
        y: float
        width: float
        height: float
    }

    type DragData = {
        draggedRectIndex: int
        cornerOffsetX: float
        cornerOffsetY: float
    }

    type DragState =
    | Dragging of DragData
    | Idle

    let rectangleDragView id =
        Component.create(id, fun ctx ->
            let rectListState = ctx.useState [
                { x = 60; y = 40; width = 200; height = 40 };
                { x = 140; y = 120; width = 160; height = 60 };
                { x = 80; y = 240; width = 100; height = 120 }
            ]
            let dragState = ctx.useState Idle

            Canvas.create [
                Canvas.background "white"
                Canvas.children (
                    rectListState.Current
                    |> List.mapi (fun rectIndex rectData ->
                        let rectColor = 
                            match dragState.Current with
                            | Dragging dragData ->
                                if rectIndex = dragData.draggedRectIndex then
                                    "greenyellow"
                                else
                                    "blue"
                            | Idle -> "blue"

                        Rectangle.create [
                            Canvas.left rectData.x
                            Canvas.top rectData.y
                            Rectangle.width rectData.width
                            Rectangle.height rectData.height
                            Rectangle.fill rectColor
                            Rectangle.onPointerPressed (
                                func = (fun args ->
                                    // PointerEventArgs.GetPosition expects a control.
                                    // Notice how we retrieve it with ctx.control!
                                    let pointerPos = args.GetPosition ctx.control
                                    let newDragState = Dragging {
                                        draggedRectIndex = rectIndex;
                                        cornerOffsetX = rectData.x - pointerPos.X;
                                        cornerOffsetY = rectData.y - pointerPos.Y
                                    }
                                    dragState.Set newDragState
                                ),
                                // Since the event handler captures rectData, it needs
                                // to be updated on render to avoid stale data being
                                // stuck in it.
                                //
                                // This line configures the update of the handler
                                // function when rectData changes.
                                subPatchOptions = SubPatchOptions.OnChangeOf rectData
                            )
                            Rectangle.onPointerMoved (fun args ->
                                // PointerEventArgs.GetPosition expects a control.
                                // Notice how we retrieve it with ctx.control!
                                let pointerPos = args.GetPosition ctx.control
                                match dragState.Current with
                                | Dragging dragData ->
                                    let newRectList =
                                        rectListState.Current
                                        |> List.mapi (fun rectIndex rectData ->
                                            if rectIndex = dragData.draggedRectIndex then
                                                let newRectData = {
                                                    x = pointerPos.X + dragData.cornerOffsetX;
                                                    y = pointerPos.Y + dragData.cornerOffsetY;
                                                    width = rectData.width;
                                                    height = rectData.height;
                                                }
                                                newRectData
                                            else
                                                rectData
                                        )
                                    rectListState.Set newRectList
                                | Idle -> ()
                            )
                            Rectangle.onPointerReleased (fun args -> dragState.Set Idle)
                        ]
                    )
                )
            ]
        )

    let mainView() =
        Component(fun ctx ->
            TabControl.create [
                TabItem.borderThickness (left = 0, top = 0, right = 0, bottom = 12)
                TabControl.viewItems [
                    TabItem.create [
                        TabItem.header "Select Rectangle Demo"
                        TabItem.content (rectangleSelectView "rectangleSelect")
                    ]
                    TabItem.create [
                        TabItem.header "Drag Rectangle Demo"
                        TabItem.content (rectangleDragView "rectangleDrag")
                    ]
                ]
            ]
        )
