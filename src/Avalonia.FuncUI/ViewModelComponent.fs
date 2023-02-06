namespace Avalonia.FuncUI

open System
open System.ComponentModel
open System.Reflection
open Avalonia.Controls
open Avalonia.FuncUI.Types
open Avalonia.Styling

[<AbstractClass>]
type ViewModelComponentBase () as this =
    inherit Border ()

    let propertyChanged = Event<PropertyChangedEventArgs>()

    do
        //this.DataContext <- this
        this.Child <-
            ()
            |> this.Build
            |> VirtualDom.VirtualDom.create

    abstract member Build: unit -> IView

    interface INotifyPropertyChanged
        member this.PropertyChanged = propertyChanged.Publish

    interface IStyleable with
        member this.StyleKey = typeof<Border>


open HarmonyLib

type ObserveAttribute () =
    inherit Attribute()

//[<AbstractClass; Sealed>]
//type ViewModelPatcher () =
//
//    static member MyPrefix () =
//        printfn "MyPrefix"
//
//    static member MyPostfix () =
//        printfn "MyPostfix"
//
//    static member DoPatching (harmony: Harmony, method: MethodInfo) =
//
//
//        let mPrefix = SymbolExtensions.GetMethodInfo(fun () -> ViewModelPatcher.MyPrefix());
//        let mPostfix = SymbolExtensions.GetMethodInfo(fun () -> ViewModelPatcher.MyPostfix());
//        // in general, add null checks here (new HarmonyMethod() does it for you too)
//
//        let _ = harmony.Patch(method, new HarmonyMethod(mPrefix), new HarmonyMethod(mPostfix))
//        ()
//
//    static member PatchAll () =
//        let harmony = new Harmony("com.example.patch")
//        Harmony.DEBUG <- true;
//
//        ()
//        |> AccessTools.AllTypes
//        |> Seq.filter (fun t -> t.IsSubclassOf(typeof<ViewModelComponentBase>))
//        |> Seq.collect (fun t ->
//            t.GetProperties()
//            |> Seq.filter (fun p -> p.GetCustomAttributes(typeof<ObserveAttribute>, false).Length > 0)
//            |> Seq.map (fun p -> p.GetSetMethod())
//        )
//        |> Seq.iter (fun m -> ViewModelPatcher.DoPatching(harmony, m))
//
//module ViewModelPatcher =
//    do ViewModelPatcher.PatchAll()