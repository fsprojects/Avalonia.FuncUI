[fantomas]: https://github.com/fsprojects/fantomas
[f# formatting]: https://marketplace.visualstudio.com/items?itemName=asti.fantomas-vs

# AvaApp

This is an Avalonia.FuncUI Starter template, this template shows you in a brief way how you can create components and functions to render your UI.

To run this application just type

```
dotnet run
```

You should briefly see your application window showing on your desktop.

### Next Steps

If you're using VSCode we recommend you to install [Fantomas]

```
dotnet new tool-manifest
dotnet tool install fantomas
```

and add the following `.editorconfig`

```editorconfig
root = true

[*]
indent_style=space
indent_size=4
charset=utf-8
trim_trailing_whitespace=true
insert_final_newline=false

[*.fs]
fsharp_single_argument_web_mode=true
```

This should will allow Fantomas to format your code on save.

Other editors like Rider and Visual Studio [F# Formatting] can also pick up these settings even if you don't install fantomas.

### Feeling a little lost?

Check out the documentation:
https://avaloniacommunity.github.io/Avalonia.FuncUI.Docs/

The documentation is still work in progress and will change over the next few months, but feel free to contribute and ask questions if needed.
