namespace Avalonia.FuncUI

module Core =
    
    module Domain =
       
        type Accessor =
            | Instance of string
            | Avalonia of Avalonia.AvaloniaProperty
        
        type Property =
            {
                accessor: Accessor
                
            }
        
        type Content =
            {
                accessor: Accessor
            }
        
        type Attr =
            | Content
            | Property
            | Event