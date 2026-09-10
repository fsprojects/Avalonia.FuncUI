namespace Avalonia.FuncUI.DSL

open Avalonia
open Avalonia.Controls
open Avalonia.Styling
open Avalonia.FuncUI.Builder
open Avalonia.FuncUI.Types

[<AutoOpen>]
module Application =
    type Application with
        static member name(value: string) : IAttr<Application> =
            AttrBuilder<Application>.CreateProperty(Application.NameProperty, value, ValueNone)

        static member requestedThemeVariant(value: ThemeVariant) : IAttr<Application> =
            AttrBuilder<Application>.CreateProperty(Application.RequestedThemeVariantProperty, value, ValueNone)

        static member nativeMenu(value: IView<NativeMenu> option) : IAttr<Application> =
            AttrBuilder<Application>.CreateContentSingle(NativeMenu.MenuProperty, value |> Option.map (fun menu -> menu :> IView))

        static member nativeMenu(value: IView<NativeMenu>) : IAttr<Application> =
            Application.nativeMenu(Some value)

        /// The host owns the created icons and disposes them on removal or shutdown.
        static member trayIcons(value: IView<TrayIcon> list) : IAttr<Application> =
            let getter (app: Application) : obj =
                match TrayIcon.GetIcons app with
                | null ->
                    let icons = TrayIcons()
                    TrayIcon.SetIcons(app, icons)
                    icons
                | icons -> icons
            AttrBuilder<Application>.CreateContentMultiple(
                "TrayIcons", ValueSome getter, ValueNone,
                value |> List.map (fun icon -> icon :> IView))
