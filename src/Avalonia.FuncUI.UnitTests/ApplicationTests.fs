namespace Avalonia.FuncUI.UnitTests

open System
open Avalonia
open Avalonia.Controls
open Avalonia.Headless
open Avalonia.Threading
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.VirtualDom
open Xunit

// These tests change Avalonia's application services and dispatcher.
[<CollectionDefinition("Application", DisableParallelization = true)>]
type ApplicationCollection() = class end

type ApplicationTestBuilder =
    static member BuildAvaloniaApp() =
        AppBuilder.Configure<HostApplication>().UseHeadless(AvaloniaHeadlessPlatformOptions())

type CountingTrayProxy() =
    inherit Reflection.DispatchProxy()
    let mutable disposals = 0
    member _.Disposals = disposals
    override _.Invoke(methodInfo, _) =
        if methodInfo.Name = "Dispose" then disposals <- disposals + 1
        null

[<Collection("Application")>]
module ApplicationTests =
    let private run action =
        use session = HeadlessUnitTestSession.StartNew(typeof<ApplicationTestBuilder>)
        session.Dispatch(Action action, Threading.CancellationToken.None).GetAwaiter().GetResult()

    let private click (item: NativeMenuItem) =
        (item :> INativeMenuItemExporterEventsImplBridge).RaiseClicked()

    [<Fact>]
    let ``Native event handlers update and unsubscribe without a Control`` () = run (fun () ->
        let mutable observed = 0
        let view value = NativeMenuItem.create [
            NativeMenuItem.header "Increment"
            NativeMenuItem.onClick ((fun _ -> observed <- value), SubPatchOptions.Always)
        ]
        let first = view 1
        let item = VirtualDom.createObject first :?> NativeMenuItem
        click item
        Assert.Equal(1, observed)
        let next = view 2
        VirtualDom.update(item, first, next)
        click item
        Assert.Equal(2, observed)
        let empty = NativeMenuItem.create []
        VirtualDom.update(item, next, empty)
        observed <- 0
        click item
        Assert.Equal(0, observed))

    [<Fact>]
    let ``Application attributes patch native objects in place and clean up removal`` () = run (fun () ->
        use app = Application.Current :?> HostApplication
        let attrs text = [
            Application.nativeMenu (NativeMenu.create [
                NativeMenu.items [
                    NativeMenuItem.create [
                        NativeMenuItem.header text
                        NativeMenuItem.onClick ignore
                        NativeMenuItem.menu (NativeMenu.create [ NativeMenu.items [ NativeMenuItemSeparator.create [] ] ])
                    ]
                ]
            ])
            Application.trayIcons [ TrayIcon.create [ TrayIcon.toolTipText text; TrayIcon.onClicked ignore ] ]
        ]
        app.Update(attrs "First")
        let menu = NativeMenu.GetMenu app
        let item = menu.Items.[0] :?> NativeMenuItem
        let icon = (TrayIcon.GetIcons app).[0]
        app.Update(attrs "Second")
        Assert.Same(menu, NativeMenu.GetMenu app)
        Assert.Same(item, menu.Items.[0])
        Assert.Same(icon, (TrayIcon.GetIcons app).[0])
        Assert.Equal("Second", item.Header)
        Assert.Equal("Second", icon.ToolTipText)
        app.Update([]: IAttr<Application> list)
        Assert.Null(NativeMenu.GetMenu app)
        Assert.Empty(TrayIcon.GetIcons app)
        Assert.Empty(ViewMetaData.GetViewSubscriptions item)
        Assert.Empty(ViewMetaData.GetViewSubscriptions icon))

    [<Fact>]
    let ``Application component keeps state and cancels queued work on disposal`` () = run (fun () ->
        use app = Application.Current :?> HostApplication
        let mutable state: IWritable<int> option = None
        let mutable renders = 0
        let mutable effects = 0
        let mutable cleanups = 0
        app.Update(ApplicationComponent(fun ctx ->
            let count = ctx.useState 0
            state <- Some count
            renders <- renders + 1
            ctx.useEffect((fun () ->
                effects <- effects + 1
                { new IDisposable with member _.Dispose() = cleanups <- cleanups + 1 }), [EffectTrigger.AfterInit])
            [ Application.name $"Count {count.Current}" ]))
        Dispatcher.UIThread.RunJobs()
        let count = state.Value
        Assert.Equal(1, effects)
        count.Set 5
        Dispatcher.UIThread.RunJobs()
        Assert.Same(count, state.Value)
        Assert.Equal("Count 5", app.Name)
        Assert.Equal(2, renders)
        Assert.Equal(1, effects)
        count.Set 6
        app.Dispose()
        Dispatcher.UIThread.RunJobs()
        Assert.Equal(2, renders)
        Assert.Equal(1, cleanups))

    [<Fact>]
    let ``Replacing a component cancels its pending effects`` () = run (fun () ->
        use app = Application.Current :?> HostApplication
        let mutable effects = 0
        app.Update(ApplicationComponent(fun ctx ->
            ctx.useEffect((fun () -> effects <- effects + 1), [EffectTrigger.AfterInit])
            [ Application.name "Old" ]))
        app.Update([ Application.name "New" ])
        Dispatcher.UIThread.RunJobs()
        Assert.Equal(0, effects)
        Assert.Equal("New", app.Name))

    [<Fact>]
    let ``Tray removal disposes the platform handle exactly once`` () = run (fun () ->
        use app = Application.Current :?> HostApplication
        // Platform implementation members are hidden in Avalonia's reference assembly.
        let impl = Reflection.DispatchProxy.Create<Avalonia.Platform.ITrayIconImpl, CountingTrayProxy>()
        let counter = box impl :?> CountingTrayProxy
        // Inject a counting platform implementation through Avalonia's private constructor.
        let ctor = typeof<TrayIcon>.GetConstructor(
            Reflection.BindingFlags.Instance ||| Reflection.BindingFlags.NonPublic,
            null, [| typeof<Avalonia.Platform.ITrayIconImpl> |], null)
        let icon = ctor.Invoke([| impl |]) :?> TrayIcon
        let icons = TrayIcons()
        TrayIcon.SetIcons(app, icons)
        icons.Add icon
        app.Update([ Application.trayIcons [ TrayIcon.create [ TrayIcon.onClicked ignore ] ] ])
        app.Update([ Application.trayIcons [] ])
        Assert.Equal(1, counter.Disposals)
        Assert.Empty(ViewMetaData.GetViewSubscriptions icon)
        app.Dispose()
        Assert.Equal(1, counter.Disposals))

    [<Fact>]
    let ``Key replacement and shutdown release nested native subscriptions`` () = run (fun () ->
        use app = Application.Current :?> HostApplication
        let attrs key = [
            Application.trayIcons [
                TrayIcon.create [
                    TrayIcon.onClicked ignore
                    TrayIcon.menu (NativeMenu.create [
                        NativeMenu.onOpening ignore
                        NativeMenu.items [ NativeMenuItem.create [ NativeMenuItem.onClick ignore ] ]
                    ])
                ] |> View.withKey key
            ]
        ]
        app.Update(attrs "first")
        let first = (TrayIcon.GetIcons app).[0]
        let child = first.Menu.Items.[0] :?> NativeMenuItem
        app.Update(attrs "second")
        let second = (TrayIcon.GetIcons app).[0]
        Assert.NotSame(first, second)
        Assert.Empty(ViewMetaData.GetViewSubscriptions first)
        Assert.Empty(ViewMetaData.GetViewSubscriptions first.Menu)
        Assert.Empty(ViewMetaData.GetViewSubscriptions child)
        app.Dispose()
        Assert.Empty(TrayIcon.GetIcons app)
        Assert.Empty(ViewMetaData.GetViewSubscriptions second)
        Assert.Empty(ViewMetaData.GetViewSubscriptions second.Menu)
        Assert.Throws<ObjectDisposedException>(fun () -> app.Update([]: IAttr<Application> list)) |> ignore)
