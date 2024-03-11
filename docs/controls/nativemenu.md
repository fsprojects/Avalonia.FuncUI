# NativeMenu

> You can check the [NativeMenu](https://docs.avaloniaui.net/docs/controls/nativemenu) and [NativeMenu API](http://reference.avaloniaui.net/api/Avalonia.Controls/NativeMenu) Avalonia docs for more information

Native menus were introduced in Avalonia in version 0.9.0, you can check the [announcement](https://avaloniaui.net/blog/#osx-linux-native-menus) to see a brief explanation on how to use them in Avalonia Applications.

Currently for Avalonia.FuncUI there is not a DSL and the NativeMenu control is in a weird spot for Avalonia.FuncUI since this control works directly on the main Application/Window object so it's tough to pull a DSL on top of that. But! thankfully you can just use plain F# for the menu as noted [in this issue](https://github.com/AvaloniaCommunity/Avalonia.FuncUI/issues/113).

### Usage

Inside your `Program.fs` File find the `App` class and be sure to set the name of your Application

```fsharp
type MainWindow() as this = (*... code ... *)

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Load "avares://Avalonia.Themes.Default/DefaultTheme.xaml"
        this.Styles.Load "avares://Avalonia.Themes.Default/Accents/BaseDark.xaml"

        // 🚩name visible in native menu
        this.Name <- "Counter App"

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()
```

then just create a new NativeMenu

```fsharp
type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Counter Example"
        base.Height <- 400.0
        base.Width <- 400.0

        // 🚩create menu and menu items
        let incrementItem = NativeMenuItem "Increment"
        let decrementItem = NativeMenuItem "Decrement"

        let editCounterItem = NativeMenuItem "Edit Counter"
        let editCounterMenu =  NativeMenu()
        editCounterItem.Menu <- editCounterMenu
        editCounterMenu.Add incrementItem
        editCounterMenu.Add decrementItem

        let nativeMenu = NativeMenu()
        nativeMenu.Add editCounterItem

        // 🚩set menu
        NativeMenu.SetMenu(this, nativeMenu)

        Elmish.Program.mkSimple (fun () -> Counter.init) Counter.update Counter.view
        |> Program.withHost this
        |> Program.withConsoleTrace
        |> Program.run
```

and that is enough to show your native menu. If you want to interact with the contents of your menu (the most likely scenario) you will need to add some subscriptions to hook up with your Elmish program

```fsharp
// 🚩hook menu actions in Elmish
let menuSub (_state: Counter.State) =
    let sub (dispatch: Counter.Msg -> unit) =
        incrementItem.Clicked.Add (fun _ -> dispatch Counter.Msg.Increment)
        decrementItem.Clicked.Add (fun _ -> dispatch Counter.Msg.Decrement)
        ()
    Cmd.ofSub sub

Elmish.Program.mkSimple (fun () -> Counter.init) Counter.update Counter.view
|> Program.withHost this
// 🚩 use menu subscription
|> Program.withSubscription menuSub

|> Program.withConsoleTrace
|> Program.run
```
