---
layout: guide
title: Quickstart Template
author: Avalonia Community
list-order: 3
guide-category: beginner
---
[Basic Template]: guides/Basic-Template.html
[Full Template]: guides/Full-Template.html
[Shell.fs]: guides/Full-Template.html#shellfs

The **Quickstart Template** is intended to be used as a starting point for more *serious* projects, which means it includes more files that give you a lead on how you can do more complex things. It also is not intended to block you or dictate how you **MUST** do Avalonia.FuncUI applications. You can add/remove files/solutions as you want or need. It should not impede you, as we explained in the [Basic Template] all you need is a couple of files and `.net core` installed to get up and running.

### File Structure
This is the File Structure for the Quickstart Template
```
│   ProjectName.sln
│
├───ProjectName
│       About.fs
│       Program.fs
│       ProjectName.fsproj
│       Shell.fs
│       Styles.xaml
│       TreeViewPage.fs
│       UserProfiles.fs
│
├───ProjectName.Core
│       Library.fs
│       ProjectName.Core.fsproj
│       Users.fs
│
└───ProjectName.Core.Tests
        Main.fs
        Sample.fs
        ProjectName.Core.Tests.fsproj
```
We offer three main projects
- ProjectName
- ProjectName.Core
- ProjectName.Core.Tests

`ProjectName` is where your Avalonia.FuncUI code resides, it is your application. 

`ProjectName.Core` is where you may have Shared logic that can be reused between different kinds of solutions like `Web` or `Console` in case you decide to add these solutions for your project (having a Shared/Core is not required).

`ProjectName.Core.Tests` is a Test Project based on [Expecto](https://github.com/haf/expecto) an F# Testing library. It is not required to use Expecto, you can replace it with any testing library you see fit for your needs.

# ProjectName
The ProjectName directory contains seven files, some of those have been previously discussed in the [Basic Template] and [Full Template] doc pages. There are a couple of differences on the `Shell.fs` file which will be covered in short. There are also two new pages

- TreeViewPage.fs
- UserProfiles.fs

## TreeViewPage.fs
TreeViewPage is a simple Elmish module that shows you how to use an Avalonia Tree Control
```fsharp
type Taxonomy =
  { Name: string
    Children: Taxonomy seq }
```
That is the main Tree structure that is going to be represented inside this Elmish module

```fsharp
/// ... omitted code
TreeView.create
    [ TreeView.dock Dock.Left
      /// dataItems refers to the source of your control's data
      /// these are going to be iterated to fill your template's contents
      TreeView.dataItems [ food ]
      TreeView.itemTemplate
          /// You can pass the type of your data collection
          /// to have a safe type reference in the create function
          (DataTemplateView<Taxonomy>
              .create
                  ((fun data -> data.Children),
                    (fun data ->
                        TextBlock.create
                            [ TextBlock.onTapped (fun _ -> dispatch (ShowDetail data))
                              TextBlock.text data.Name ]))) ]
/// ... more omitted code
```
Just as you would do in a XAML based approach, you can use DataTemplates in Avalonia.FuncUI.
`.dataItems` represents the data you want to use with this control it can be any kind of data, the item template uses `DataTemplateView<'t>` with a couple of functions to declare how the control should look like
as always you can use any control you deem necessary in your data template.

From there on our Elmish module is almost equal as any other module we've seen so far. There's a key difference though and that is that this module exposes a `Host` Type

```fsharp
type Host() as this =
    inherit Hosts.HostControl()
    do
        /// You can use `.mkProgram` to pass Commands around
        /// if you decide to use it, you have to also return a Command in the initFn
        /// (init, Cmd.none)
        /// you can learn more at https://elmish.github.io/elmish/basics.html
        let startFn () =
            init
        Elmish.Program.mkSimple startFn update view
        |> Program.withHost this
        |> Program.run
```
[HostControls](https://github.com/AvaloniaCommunity/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI/Components/Hosts.fs#L24) are standalone controls, this is a different approach from what we saw at [Shell.fs] when including children structures, in that module we used the functions exposed by the *About.fs* and *Counter.fs* and integrated them with `Shell` our workflow, so we could intercept messages and act accordingly on the `Shell` module (for a better example on that check the [Music Player's Update function](https://github.com/AvaloniaCommunity/Avalonia.FuncUI/blob/master/src/Examples/Examples.MusicPlayer/Shell.fs#L109)). The Host control can act as an individual Control itself and handle its workflow without having to expose its functions to the `Shell` module. If you know your control doesn't need any external input and doesn't have any output that might affect other Controls in your application (the about page could be an example of that as well) perhaps you may want to use the *HostControl* approach.

## UserProfiles.fs
The `UserProfiles` page exposes a `HostControl` similarly to the `TreeViewPage`. It also shows you some ways on how to work with internet resources like a Restful API

What is different from other modules? 
- The usage of `Cmd.ofAsync`

our init function is defined as 
```fsharp
/// sample function to load the initial data
let loadInit() =
    /// this comes from our `ProjectName.Core` solution
    Users.getUsers None None

type State =
    { users: (Users.UserEndpoint.Result * Bitmap) array }

type Msg =
    | SetUsers of (Users.UserEndpoint.Result * Bitmap) array
    | LoadImages of Users.UserEndpoint.Result array

let init = { users = Array.empty }, Cmd.OfAsync.perform loadInit () LoadImages
```
Elmish provides a module `OfAsync` that has great functions to allows us to have a seamless workflow with async interactions as well as a module `OfTask` in case we need to deal with `System.Threading.Tasks.Task`.

When the init function is called by `Elmish.Program.mkProgram...` we schedule an async command that upon successful execution it will invoke the `LoadImages of Users.UserEndpointResult array` message, that way we're loading the initial payload for this Control fetching the resources over the network.

The `update` function also shows a little bit of how neat is to work with F#'s Async

```fsharp
let update (msg: Msg) (state: State): State * Cmd<_> =
    match msg with
    | LoadImages users ->
        let loadingImgs() =
            async {
                let! requests = users
                                |> Array.map (fun user -> Users.getImageFromUrl user user.Picture.Large)
                                |> Async.Parallel
                return requests |> Array.Parallel.map (fun (user, src) -> user, new Bitmap(src))
            }
        state, Cmd.OfAsync.perform loadingImgs () SetUsers
    | SetUsers users ->
        { state with users = users }, Cmd.none
```
`loadingImgs()` is an async function that does a bit of `Parallel` processing, if you come from javascript'land this is a version of `Promise.all(promiseArray)`. Once we're done with our async request we simply return what's expected from the update function, `The State` and the `Command` in this case an async command and the unmodified state, to update the state upon completion we'll use `SetUsers of (Users.UserEndpoint.Result * Bitmap) array`. The rest should be familiar and consistent with other the other templates we've discussed before

# ProjectName.Core
The core solution is a simple `.netstandard2.0` F# library. If you take a look at the `Library.fs` file you'll see the default values from `dotnet new classlib -lang F#`
```fsharp
module Say =
    let hello name = sprintf "Hello, %s" name
```
## Users.fs
the `Users.fs` file includes an example of fetching resources over the network using [F#'s Type Providers](https://fsharp.github.io/FSharp.Data/) which is a `Type Safe` way to interact with a Public Restful API. Here we used the [Random User API](https://randomuser.me/)


# ProjectName.Core.Tests
Lastly, the Tests solution includes a sample suite of tests including a `Hello World` test
```fsharp
test "Hello, World!" {
    /// Testing our Core Library
    let actual = hello "World!"
    Expect.equal actual "Hello, World!" "hello Should say Hello, World!"
}
```
If you wonder how to test your application using Expecto you can find that [here](guides/Unit-Testing-Avalonia-FuncUI-Apps.html)


