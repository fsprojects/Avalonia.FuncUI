# Stateful application example

Run from the repository root:

```sh
dotnet run --project src/Examples/Examples.Application
```

`App.fs` mounts an `ApplicationComponent` on `HostApplication`. Its attributes patch the application created by `AppBuilder`. The application and views stay in F#, while custom styles live in `AppStyles.axaml`.

The window, application menu and tray menu share a counter through `ctx.usePassed`. The application component also owns a dark-theme setting through `ctx.useState`. Increment from either menu or the window, and watch the menu labels and tray tooltip update. Use the window button to remove and recreate the tray icon.

The native application menu contains a Counter submenu. Each menu description creates its own native objects, since Avalonia menus cannot share a parented instance.

## Custom application styles

`AppStyles.axaml` defines the custom styles in XAML, without an `App.axaml` or code-behind class. Avalonia's default build items include the `.axaml` file automatically. `App.Initialize` loads it through `Avalonia.Markup.Xaml.Styling.StyleInclude` after `FluentTheme`, once for the application's lifetime:

```fsharp
override this.Initialize() =
    this.Styles.Add(FluentTheme())
    this.Styles.Add(
        StyleInclude(
            Uri("avares://Examples.Application/"),
            Source = Uri("avares://Examples.Application/AppStyles.axaml")))
```

The window applies them through `TextBlock.classes [ "counter" ]` and `Button.classes [ "increment" ]`. The counter gets larger, semibold text, and the increment button gets custom padding and font weight. Theme colors remain unchanged, so the styles work with the application's light/dark toggle.

These styles apply to Avalonia controls in the window, not OS-rendered native menus or tray icons. There is no new application-style DSL; one-time style registration stays separate from the stateful application attributes.

## Platform behavior

- macOS displays the application menu in the system menu bar. Windows does not display an application-level native menu.
- Tray availability depends on the OS and desktop environment. On Linux, a status-notifier host or suitable desktop extension may be needed.
- Avalonia does not raise the tray `Clicked` event on macOS. The tray menu's Show window command remains available.
- Closing the main window or choosing Quit exits this example. It deliberately does not stay running as a tray-only app.

## Host lifecycle

Mount with `this.Update(ApplicationComponent(fun ctx -> ...))` in `OnFrameworkInitializationCompleted`. Call the base implementation so the host registers lifetime-exit cleanup. Keep themes and other one-time setup in `Initialize`.

`IStateContext` supplies the same state and effect hooks as visual components, but has no `control` or `attrs` member. Call hooks in a stable order on every render.

State changes rerender the mounted component and patch existing native objects. Calling `Update` with another component starts fresh hooks and disposes the previous context. `Update` also accepts an `IAttr<Application> list` directly for Elmish or manual updates; doing so unmounts any stateful component.

Call `Update` and `Dispose` on the UI thread. The host automatically disposes on exit for controlled application lifetimes. For other lifetimes, call `Dispose` explicitly. Removing native content cancels its FuncUI subscriptions. Avalonia disposes tray icons removed from the attached collection. Icon image objects supplied through properties remain the caller's responsibility.

Native event bindings use `SubPatchOptions.Never` by default, matching control events. A callback created at the same call site retains its original captured values unless you opt into `SubPatchOptions.Always` or `SubPatchOptions.OnChangeOf value`. The example reads `.Current` from stable hook state objects inside callbacks, so it does not need to replace subscriptions on each render.

For a callback capturing a render-time value, specify its dependency explicitly:

```fsharp
NativeMenuItem.onClick (
    (fun _ -> selectItem itemId),
    SubPatchOptions.OnChangeOf itemId
)
```

Use `NativeMenu.onNeedsUpdate` for menu-update notifications, rather than changing menu structure in Opening or Closed callbacks.
