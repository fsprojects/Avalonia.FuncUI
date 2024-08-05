namespace Avalonia.FuncUI.ControlCatalog.Views

open System
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI
open Avalonia.Layout
open Avalonia.Media

module DataTemplateDemo =

    type Product =
        { Id : Guid
          Name : string
          Price : string
          FavoriteColor : string
          Description : string }
        with
            static member Random () : Product =
                let faker = Bogus.Faker(locale = "de")
                { Id = Guid.NewGuid();
                  Name = faker.Commerce.Product()
                  Price = faker.Commerce.Price(0.99m, 1000.0m, 2)
                  FavoriteColor = faker.Random.Hexadecimal(6, "#")
                  Description = faker.Lorem.Sentences(Nullable(3)) }

    type State = {
        Products : Product list
        Selected : Product option
    }

    let init() = {
        Products = [ 0 .. 500 ] |> List.map (fun _ -> Product.Random())
        Selected = None
    }

    type Msg =
    | Reset
    | Remove of Guid
    | Select of Product option

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Reset -> init()
        | Remove guid ->
            { state with
                Products = state.Products |> List.filter (fun person -> person.Id <> guid)
                Selected = None
            }
        | Select product ->
            { state with Selected = product }
    
    let productDetailsView (state: Product option) (_dispatch) =
        DockPanel.create [
            DockPanel.dock Dock.Right
            DockPanel.isVisible state.IsSome  
            DockPanel.width 250.0
            DockPanel.children [
                match state with
                | Some product ->
                    // Product Color
                    yield Border.create [
                        Border.dock Dock.Top
                        Border.horizontalAlignment HorizontalAlignment.Center
                        Border.width 64.0
                        Border.height 64.0
                        Border.cornerRadius 34.0
                        Border.margin 5.0
                        Border.background product.FavoriteColor
                    ]
                    
                    // Product Name and Price
                    yield DockPanel.create [
                        DockPanel.dock Dock.Top
                        DockPanel.margin 10.0
                        DockPanel.children [
                            TextBlock.create [
                                Border.dock Dock.Left
                                TextBlock.text product.Name
                                TextBlock.fontSize 24.0
                                TextBlock.margin 5.0
                            ]
                            TextBlock.create [
                                Border.dock Dock.Right
                                TextBlock.text product.Price
                                TextBlock.foreground "#3498db"
                                TextBlock.fontSize 24.0
                                TextBlock.margin 5.0
                            ]                                    
                        ]
                    ]
                    
                    // Product Details
                    yield StackPanel.create [
                        StackPanel.dock Dock.Top
                        StackPanel.orientation Orientation.Vertical
                        StackPanel.margin 10.0
                        StackPanel.children [
                            TextBlock.create [
                                Border.dock Dock.Top
                                TextBlock.text "Description"
                                TextBlock.fontWeight FontWeight.Bold
                                TextBlock.margin 5.0
                            ]                               
                            TextBlock.create [
                                Border.dock Dock.Top
                                TextBlock.text product.Description
                                TextBlock.margin 5.0
                            ]
                        ]
                    ]
                | None -> ()
            ]
        ]    
    
    let productListView (state: State) (dispatch) =
        ListBox.create [
            ListBox.dock Dock.Left
            ListBox.onSelectedItemChanged (fun obj ->
                match obj with
                | :? Product as p -> p |> Some |> Select |> dispatch
                | _ -> None |> Select |> dispatch
            )
            ListBox.dataItems state.Products
            ListBox.itemTemplate (
                DataTemplateView<Product>.create (fun data ->
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
                                TextBlock.width 100.0
                                TextBlock.text data.Name
                                TextBlock.margin 5.0
                            ]
                            TextBlock.create [
                                TextBlock.width 100.0
                                TextBlock.text data.Price
                                TextBlock.margin 5.0
                            ]
                            Button.create [
                                Button.dock Dock.Right
                                Button.content "remove"
                                Button.onClick ((fun _ -> data.Id |> Msg.Remove |> dispatch), SubPatchOptions.OnChangeOf data.Id)
                            ]                                         
                        ]
                    ]                                  
                )                  
            )
        ]
        
    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.margin 5.0
                    TextBlock.text ($"Total Products: %i{state.Products.Length}")
                ]    
                
                productDetailsView state.Selected dispatch
                productListView state dispatch
            ]
        ]
        

