namespace Avalonia.FuncUI.ControlCatalog.Views

open System
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.DSL
open Avalonia.Layout

module DataTemplateDemo =

    type Product =
        {
            Id : Guid
            Name : string
            Price : string
            FavoriteColor : string
        }
        
    module Person =
        let random () =
            let faker = new Bogus.Faker("de")
            {
                Product.Id = Guid.NewGuid();
                Product.Name = faker.Commerce.Product()
                Product.Price = faker.Commerce.Price(0.99m, 1000.0m, 2, "€")
                Product.FavoriteColor = faker.Random.Hexadecimal(6, "#")
            }

    type State = {
        Products : Product list
        Selected : Product option
    }

    let init = {
        Products = [ 0 .. 500 ] |> List.map (fun _ -> Person.random())
        Selected = None
    }

    type Msg =
    | Reset
    | Remove of Guid
    | Select of Product option

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Reset -> init
        | Remove guid ->
            { state with
                Products = state.Products |> List.filter (fun person -> person.Id <> guid)
                Selected = None
            }
        | Select product ->
            { state with Selected = product }
    
    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                // Detail view
                DockPanel.create [
                    DockPanel.children [
                        match state.Selected with
                        | Some product ->
                            yield Border.create [
                                Border.width 16.0
                                Border.height 16.0
                                Border.cornerRadius 8.0
                                Border.margin 5.0
                                Border.background product.FavoriteColor
                            ]
                        | None -> ()
                    ]
                ]
                
                // List of Products
                ListBox.create [
                    ListBox.dock Dock.Left
                    //ListBox.selectedItem (
                    //    match state.Selected with
                    //    | Some s -> (s :> obj)
                    //    | None -> null                                             
                    //)
                    ListBox.onSelectedItemChanged (fun obj ->
                        printfn "selection changed"
                        match obj with
                        | :? Product as p ->
                            p |> Some |> Select |> dispatch
                        | _ ->
                            None |> Select |> dispatch
                    )
                    ListBox.dataItems state.Products
                    ListBox.itemTemplate (
                        DataTemplateView.create (fun (data : Product) ->
                            DockPanel.create [
                                DockPanel.lastChildFill false
                                DockPanel.children [
                                    Border.create [
                                        Border.width 16.0
                                        Border.height 16.0
                                        Border.cornerRadius 8.0
                                        Border.margin 5.0
                                        Border.background data.FavoriteColor
                                    ]
                                    TextBlock.create [
                                        TextBlock.text data.Name
                                        TextBlock.margin 5.0
                                    ]
                                    TextBlock.create [
                                        TextBlock.text data.Price
                                        TextBlock.margin 5.0
                                    ]
                                    Button.create [
                                        Button.dock Dock.Right
                                        Button.content "remove"
                                        Button.onClick (fun args -> data.Id |> Msg.Remove |> dispatch)
                                    ]                                         
                                ]
                            ]                                  
                        )                  
                    )
                ]
            ]
        ]
        

