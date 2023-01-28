namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module MaskedTextBox =
    open Avalonia.Controls
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: Attr<MaskedTextBox> list): IView<MaskedTextBox> =
        ViewBuilder.Create<MaskedTextBox> attrs

    let createFromProvider (provider: System.ComponentModel.MaskedTextProvider) (attrs: Attr<MaskedTextBox> list): IView<MaskedTextBox> =
        ViewBuilder.Create<MaskedTextBox> attrs
        |> View.withConstructorArgs [|provider|]

    type MaskedTextBox with
        /// <summary>
        /// Sets a value indicating if the masked text box is restricted to accept only ASCII characters.
        /// Default value is false.
        /// </summary>
        static member asciiOnly<'t when 't :> MaskedTextBox>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(MaskedTextBox.AsciiOnlyProperty, value, ValueNone)
        /// <summary>
        /// Sets the culture information associated with the masked text box.
        /// </summary>
        static member culture<'t when 't :> MaskedTextBox>(value: System.Globalization.CultureInfo) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<System.Globalization.CultureInfo>(MaskedTextBox.CultureProperty, value, ValueNone)
        /// <summary>
        /// Sets a value indicating if the prompt character is hidden when the masked text box loses focus.
        /// </summary>
        static member hidePromptOnLeave<'t when 't :> MaskedTextBox>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(MaskedTextBox.HidePromptOnLeaveProperty, value, ValueNone)
        /// <summary>
        /// Sets the mask to apply to the TextBox.
        /// </summary>
        static member mask<'t when 't :> MaskedTextBox>(value: string) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(MaskedTextBox.MaskProperty, value, ValueNone)
        /// <summary>
        /// Sets the character to be displayed in substitute for user input.
        /// </summary>
        static member passwordChar<'t when 't :> MaskedTextBox>(value: char) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<char>(MaskedTextBox.PasswordCharProperty, value, ValueNone)
        /// <summary>
        /// Sets the character used to represent the absence of user input in MaskedTextBox.
        /// </summary>
        static member promptChar<'t when 't :> MaskedTextBox>(value: char) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<char>(MaskedTextBox.PromptCharProperty, value, ValueNone)
        /// <summary>
        /// Sets a value indicating if selected characters should be reset when the prompt character is pressed.
        /// </summary>
        static member resetOnPrompt<'t when 't :> MaskedTextBox>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(MaskedTextBox.ResetOnPromptProperty, value, ValueNone)
        /// <summary>
        /// Sets a value indicating if selected characters should be reset when the space character is pressed.
        /// </summary>
        static member resetOnSpace<'t when 't :> MaskedTextBox>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(MaskedTextBox.ResetOnSpaceProperty, value, ValueNone)
