# Expander

> _Note_: You can check the Avalonia docs for the [Expander](https://docs.avaloniaui.net/docs/controls/expander) and [Expander API](http://reference.avaloniaui.net/api/Avalonia.Controls/Expander/) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [Expander.fs](https://github.com/fsprojects/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI/DSL/Expander.fs)

The Expander (known as accordion as well) is a control that allows a user to show or hide content to make more room for relevant information or to show detailed information about the current view

### Usage

**Set Label**

```fsharp
Expander.create [
    Expander.header "Check Logs"
    // the Logs property may be an extremely long text that's why we use an expander
    // hide the logs unless you want to see them
    Expander.content (TextBlock.create [ TextBlock.text state.Logs ])
]
```

**Change Expand Direction** you can set in which direction the content should flow

```fsharp
Expander.create [
    Expander.groupName "newsletter"
    Expander.content expanderContent
    // ExpandDirection.Up
    // ExpandDirection.Down
    // ExpandDirection.Left
    // ExpandDirection.Right
    Expander.expandDirection ExpandDirection.Up
]
```

**Use Different Transitions**

```fsharp
Expander.create [
    Expander.groupName "newsletter"
    Expander.content expanderContent
    // supply an IPageTransition
    // Expander.contentTransition (PageSlide(TimeSpan.FromSeconds(3.5), PageSlide.SlideAxis.Horizontal) :> IPageTransition)
    Expander.contentTransition (CrossFade(TimeSpan.FromSeconds(2.5)) :> IPageTransition)
]
```

**Use Multiple Expanders** you can use multiple expanders and open them programatically via their `isExpanded` property

```fsharp
Expander.create [
    Expander.header "Profile"
    Expander.isExpanded (state.CurrentSection = Sections.Profile)
    Expander.content (TextBlock.create [ TextBlock.text state.Logs ])
    Expander.onIsExpandedChanged(
        (fun isExpanded -> 
            if isExpanded then
            dispatch (SetCurrentSection (Sections.Profile))
        ), OnChangeOf(state.CurrentSection))
]
Expander.create [
    Expander.header "Preferences"
    Expander.isExpanded (state.CurrentSection = Sections.Preferences)
    Expander.content (TextBlock.create [ TextBlock.text state.Logs ])
    Expander.onIsExpandedChanged(
        (fun isExpanded -> 
            if isExpanded then
            dispatch (SetCurrentSection (Sections.Preferences))
        ), OnChangeOf(state.CurrentSection))
]
Expander.create [
    Expander.header "Misc. Information"
    Expander.isExpanded (state.CurrentSection = Sections.Preferences)
    Expander.content (TextBlock.create [ TextBlock.text state.Logs ])
    Expander.onIsExpandedChanged(
        (fun isExpanded -> 
            if isExpanded then
            dispatch (SetCurrentSection (Sections.Preferences))
        ), OnChangeOf(state.CurrentSection))
]
```
