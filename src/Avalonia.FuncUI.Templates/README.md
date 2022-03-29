# Avalonia FuncUI ProjectTemplates

This project contains .NET Project templates for FuncUI, both for Component and Elmish projects.

## Install

```
dotnet new --install JaggerJo.Avalonia.FuncUI.Templates
```

## Create an App

We currently offer templates for both Component projects and Elmish projects. If you are not sure what to choose
you can take a look at the [documentation](../../README.md) or the [examples](../Examples) to help you decide.

### Using components

### Create a new basic App

This template only contains a simple view and no extra resources that serve as a starting point if you are only creating
a basic app.

```
dotnet new funcui.basic -n NewApp
```

### Create a new full App

This template contains a more complex example with a `Style.xaml` file and other extras that you can find on more elaborate
applications.

```
dotnet new funcui.full -n NewApp
```

### Using Elmish

### Create a new basic App

This template only contains a simple view and no extra resources that serve as a starting point if you are only creating
a basic app.

```
dotnet new funcui.basic.mvu -n NewApp
```

### Create a new full App

This template contains a more complex example with a `Style.xaml` file and other extras that you can find on more elaborate
applications.

```
dotnet new funcui.full.mvu -n NewApp
```

## Run the App

All templates in both Components and Elmish flavors should create the same result:

```
dotnet run
```

![](img/NewApp_screenshot.png)
