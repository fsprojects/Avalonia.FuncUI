namespace Avalonia.FuncUI

open Avalonia
open Avalonia.FuncUI.Types

/// A nonvisual component. Hooks and effects belong to its mounted application host.
type ApplicationComponent(render: IStateContext -> IAttr<Application> list) =
    member internal _.Render context = render context
