namespace Avalonia.FuncUI

open System
open System.ComponentModel
open System.Reflection
open Avalonia
open Avalonia.Controls
open Avalonia.Data
open Avalonia.FuncUI.Types
open Avalonia.Styling

[<AutoOpen>]
module __Bindable =

    type Control with
        static member binding<'t when 't :> Control>(property: AvaloniaProperty, binding: IBinding) : IAttr<'t> =
            Attr.BindingSetup {
                BindingSetup.Property = property
                BindingSetup.Binding = binding
            }

[<AbstractClass>]
type ViewModelComponentBase () as this =
    inherit Border ()

    let propertyChanged = Event<PropertyChangedEventHandler, PropertyChangedEventArgs>()

    override this.OnInitialized () =
        base.OnInitialized ()

        this.DataContext <- this
        this.Child <-
            ()
            |> this.Build
            |> VirtualDom.VirtualDom.create

        propertyChanged.Publish.Add (fun args -> printfn $"Property Changed '%s{args.PropertyName}'")

    member this.Notify (name: string) =
        propertyChanged.Trigger(this, PropertyChangedEventArgs(name))

    abstract member Build: unit -> IView

    interface INotifyPropertyChanged

        [<CLIEvent>]
        member this.PropertyChanged = propertyChanged.Publish

    interface IStyleable with
        member this.StyleKey = typeof<Border>


open HarmonyLib

type ObserveAttribute () =
    inherit Attribute()

[<AbstractClass; Sealed>]
type ViewModelPatcher () =

    static member MyPrefix (__instance: obj, __originalMethod: MethodBase) =
        printfn $"{__instance}.{__originalMethod.Name}"

    static member MyPostfix (__instance: obj, __originalMethod: MethodBase) =
        printfn $"{__instance}.{__originalMethod.Name}"
        (__instance :?> ViewModelComponentBase).Notify(__originalMethod.Name.Substring("set_".Length))

    static member DoPatching (harmony: Harmony, method: MethodInfo) =
        let a = AccessTools.Method(typeof<ViewModelPatcher>, nameof(ViewModelPatcher.MyPostfix));
        let b = AccessTools.Method(typeof<ViewModelPatcher>, nameof(ViewModelPatcher.MyPostfix));

        let _ = harmony.Patch(method, HarmonyMethod(a), HarmonyMethod(b))
        ()

    static member PatchAll () =
        let harmony = new Harmony("com.example.patch")
        Harmony.DEBUG <- true;

        let setters =
            ()
            |> AccessTools.AllTypes
            |> Seq.filter (fun t -> t.IsSubclassOf(typeof<ViewModelComponentBase>))
            |> Seq.collect (fun t ->
                t.GetProperties()
                |> Seq.filter (fun p -> p.GetCustomAttributes(typeof<ObserveAttribute>, false).Length > 0)
                |> Seq.map (fun p -> p.GetSetMethod())
            )

        setters
        |> Seq.iter (fun m -> ViewModelPatcher.DoPatching(harmony, m))
