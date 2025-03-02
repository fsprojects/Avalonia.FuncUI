namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module HyperlinkButton =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
   
    let create (attrs: IAttr<HyperlinkButton> list): IView<HyperlinkButton> =
        ViewBuilder.Create<HyperlinkButton>(attrs)

    type HyperlinkButton with
    
        /// <summary>
        /// Sets a value indicating whether the <see cref="navigateUri"/> has been visited.
        /// </summary>
        static member isVisited<'t when 't :> HyperlinkButton>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(HyperlinkButton.IsVisitedProperty, value, ValueNone)

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) automatically navigated to when the
        /// <see cref="HyperlinkButton"/> is clicked.
        /// </summary>
        static member navigateUri<'t when 't :> HyperlinkButton>(value: System.Uri) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<System.Uri>(HyperlinkButton.NavigateUriProperty, value, ValueNone)