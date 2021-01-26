namespace Avalonia.FuncUI.DSL.Controls

module Helpers =
    [<Struct>]
    type ControllerState =
        | Writing
        | Resetting
        | Listening
        
    module ActionHistory =
        [<Struct>]
        type ActionHistory<'t> =
            { past: 't list
              present: 't
              future: 't list
              limit: int }
            
        let init t limit = { past = []; present = t; future = []; limit = limit }
        
        let setLimit ah l = { ah with limit = l }
        
        let push t ah =
            let past =
                if List.length ah.past = ah.limit
                    then ah.present::(ah.past |> List.take (ah.limit - 1))
                    else ah.present::ah.past
            { ah with present = t; past = past; future = [] }
                
        let undo ah =
            let futureCapped = (List.length ah.future) = ah.limit
            match (ah.past, futureCapped) with
            | ([], _) -> ah
            | (t::ts, false) -> { ah with present = t; past = ts; future = ah.present::ah.future }
            | (t::ts, true) ->
                let future = ah.present::(ah.future |> List.take (ah.limit - 1))
                { ah with present = t; past = ts; future = future }
        
        let redo ah =
            let pastCapped = (List.length ah.past) = ah.limit
            match (ah.future, pastCapped) with
            | ([], _) -> ah
            | (t::ts, false) -> { ah with present = t; past = ah.present::ah.past; future = ts }
            | (t::ts, true) ->
                let past = ah.present::(ah.past |> List.take (ah.limit - 1))
                { ah with present = t; past = past; future = ts }