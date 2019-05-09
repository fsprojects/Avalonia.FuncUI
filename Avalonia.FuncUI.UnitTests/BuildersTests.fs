namespace Avalonia.FuncUI.UnitTests

module BuildersTests =
    open Avalonia.FuncUI.Core
    open Avalonia.FuncUI.Builders
    open Xunit
    open Avalonia.Controls

    [<Fact>]
    let ``Avalonia Property is set`` () =
        let _value = "some text"
        let _width = 100.0
        let _tag = System.Guid.NewGuid() :> obj

        let textView = textblock {
            text _value
            width _width
            tag _tag
        }

        let textControl = (textView :> IViewElement).Create() :?> TextBlock

        Assert.Equal(_value, textControl.Text)
        Assert.Equal(_width, textControl.Width)
        Assert.Equal(_tag, textControl.Tag)