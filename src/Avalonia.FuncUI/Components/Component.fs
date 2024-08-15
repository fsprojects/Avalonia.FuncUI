namespace Avalonia.FuncUI

open System.Diagnostics.CodeAnalysis
open Avalonia.FuncUI
open Avalonia.FuncUI.Types

[<AllowNullLiteral>]
[<DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)>]
type Component (render: IComponentContext -> IView) as this =
    inherit ComponentBase ()

    override this.Render ctx =
        render ctx