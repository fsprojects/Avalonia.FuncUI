namespace Avalonia.FuncUI.UnitTests

open System
open Avalonia.Controls
open Avalonia.Headless
open Avalonia.Threading
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Xunit

[<Collection("Application")>]
module ComponentSchedulingTests =
    let private run action =
        use session = HeadlessUnitTestSession.StartNew(typeof<ApplicationTestBuilder>)
        session.Dispatch(Action action, Threading.CancellationToken.None).GetAwaiter().GetResult()

    [<Fact>]
    let ``Pending render requests share one render and see the latest state`` () = run (fun () ->
        let mutable renders = 0
        let mutable effects = 0
        let mutable state: IWritable<int> option = None
        let view = { new ComponentBase() with
            override _.Render (ctx: IComponentContext) : Types.IView =
                renders <- renders + 1
                // Request renders explicitly to test the component's queue directly.
                let count = ctx.useState(0, renderOnChange = false)
                state <- Some count
                ctx.useEffect((fun () -> effects <- effects + 1), [EffectTrigger.AfterRender])
                TextBlock.create [ TextBlock.text (string count.Current) ] }
        let window = Window(Content = view)
        try
            window.Show()
            Dispatcher.UIThread.RunJobs()
            let initialRenders, initialEffects = renders, effects
            state.Value.Set 1
            view.ForceRender()
            state.Value.Set 2
            view.ForceRender()
            view.ForceRender()
            Assert.Equal(initialRenders, renders)
            Dispatcher.UIThread.RunJobs()
            Assert.Equal(initialRenders + 1, renders)
            Assert.Equal(initialEffects + 1, effects)
            Assert.Equal("2", (view.Child :?> TextBlock).Text)
        finally window.Close())

    [<Fact>]
    let ``A request during rendering schedules another pass`` () = run (fun () ->
        let mutable renders = 0
        let mutable requestDuringRender = false
        let view = { new ComponentBase() with
            override _.Render (ctx: IComponentContext) : Types.IView =
                renders <- renders + 1
                if requestDuringRender then
                    requestDuringRender <- false
                    ctx.forceRender()
                TextBlock.create [ TextBlock.text (string renders) ] }
        let window = Window(Content = view)
        try
            window.Show()
            Dispatcher.UIThread.RunJobs()
            let initialRenders = renders
            requestDuringRender <- true
            view.ForceRender()
            Dispatcher.UIThread.RunJobs()
            Assert.Equal(initialRenders + 2, renders)
        finally window.Close())

    [<Fact>]
    let ``Detaching a component cancels its queued render`` () = run (fun () ->
        let mutable renders = 0
        let mutable effects = 0
        let view = { new ComponentBase() with
            override _.Render (ctx: IComponentContext) : Types.IView =
                renders <- renders + 1
                ctx.useEffect((fun () -> effects <- effects + 1), [EffectTrigger.AfterRender])
                TextBlock.create [] }
        let window = Window(Content = view)
        try
            window.Show()
            Dispatcher.UIThread.RunJobs()
            let initialRenders, initialEffects = renders, effects
            view.ForceRender()
            window.Content <- null
            view.ForceRender()
            Dispatcher.UIThread.RunJobs()
            Assert.Equal(initialRenders, renders)
            Assert.Equal(initialEffects, effects)
        finally window.Close())
