namespace Avalonia.FuncUI.DSL

open Avalonia

[<AutoOpen>]
module MaskedTextBox =
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.Media.Immutable
    open Avalonia.Media
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<TextBox> list): IView<TextBox> =
        ViewBuilder.Create<TextBox> attrs

    let createFromProvider (provider: System.ComponentModel.MaskedTextProvider) (attrs: IAttr<TextBox> list): IView<TextBox> =
        ViewBuilder.Create<TextBox> attrs
        |> View.withConstructorArgs [|provider|]

    type MaskedTextBox with
      /// <summary>
      /// Sets a value indicating if the masked text box is restricted to accept only ASCII characters.
      /// Default value is false.
      /// </summary>
      static member asciiOnly<'t when 't :> MaskedTextBox>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(MaskedTextBox.AsciiOnlyProperty, value, ValueNone)
      /// <summary>
      /// Sets the culture information associated with the masked text box.
      /// </summary>
      static member culture<'t when 't :> MaskedTextBox>(value: System.Globalization.CultureInfo) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<System.Globalization.CultureInfo>(MaskedTextBox.CultureProperty, value, ValueNone)
      /// <summary>
      /// Sets a value indicating if the prompt character is hidden when the masked text box loses focus.
      /// </summary>
      static member hidePromptOnLeave<'t when 't :> MaskedTextBox>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(MaskedTextBox.HidePromptOnLeaveProperty, value, ValueNone)
      /// <summary>
      /// Sets the mask to apply to the TextBox.
      /// </summary>
      static member mask<'t when 't :> MaskedTextBox>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(MaskedTextBox.MaskProperty, value, ValueNone)
      /// <summary>
      /// Specifies whether the test string required input positions, as specified by the mask, have all been assigned.
      /// </summary>
      static member maskCompleted<'t when 't :> MaskedTextBox>(value: System.Nullable<bool>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<System.Nullable<bool>>(MaskedTextBox.MaskCompletedProperty, value, ValueNone)
      /// <summary>
      /// Specifies whether all inputs (required and optional) have been provided into the mask successfully.
      /// </summary>
      static member maskFull<'t when 't :> MaskedTextBox>(value: System.Nullable<bool>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<System.Nullable<bool>>(MaskedTextBox.MaskFullProperty, value, ValueNone)
      /// <summary>
      /// Sets the character to be displayed in substitute for user input.
      /// </summary>
      static member passwordChar<'t when 't :> MaskedTextBox>(value: char) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<char>(MaskedTextBox.PasswordCharProperty, value, ValueNone)
      /// <summary>
      /// Sets the character used to represent the absence of user input in MaskedTextBox.
      /// </summary>
      static member promptChar<'t when 't :> MaskedTextBox>(value: char) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<char>(MaskedTextBox.PromptCharProperty, value, ValueNone)
      /// <summary>
      /// Sets a value indicating if selected characters should be reset when the prompt character is pressed.
      /// </summary>
      static member resetOnPrompt<'t when 't :> MaskedTextBox>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(MaskedTextBox.ResetOnPromptProperty, value, ValueNone)
      /// <summary>
      /// Sets a value indicating if selected characters should be reset when the space character is pressed.
      /// </summary>
      static member resetOnSpace<'t when 't :> MaskedTextBox>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(MaskedTextBox.ResetOnSpaceProperty, value, ValueNone)
