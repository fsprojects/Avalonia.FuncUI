namespace Avalonia.FuncUI.ControlCatalog.Views

open System
open Avalonia.Controls
open Elmish
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.Elmish

module DataGridDemo =
    // [<CLIMutable>] required for editability
    [<CLIMutable>] 
    type Product =
        { Name : string
          Price : string
          FavoriteColor : string
          Description : string }
        with
            static member Random () : Product =
                let faker = Bogus.Faker(locale = "de")
                { Name = faker.Commerce.Product()
                  Price = faker.Commerce.Price(0.99m, 1000.0m, 2)
                  FavoriteColor = faker.Random.Hexadecimal(6, "#")
                  Description = faker.Lorem.Sentences(Nullable(3)) }

    type State = 
      { products: Product list }

    let init () = 
      { products = List.map (fun _ -> Product.Random()) [0 .. 50] }

    type Msg =
    | AddEmptyRow

    let update (msg: Msg) (state: State) : State =
        match msg with
        | AddEmptyRow -> { state with products = { Name = ""; Price = ""; FavoriteColor = ""; Description = "" } :: state.products }
           
    let view (state: State) dispatch =
        StackPanel.create[
            StackPanel.children[
                Button.create[
                    Button.onClick(fun _ -> AddEmptyRow |> dispatch)
                    Button.content "Add Row"
                ]
                DataGrid.create[
                    DataGrid.items state.products
                    DataGrid.autoGenerateColumns true
                ]
            ]
        ]
        
    type Host() as this =
        inherit Hosts.HostControl()
        do
            Program.mkSimple init update view
            |> Program.withHost this
            |> Program.withConsoleTrace
            |> Program.run