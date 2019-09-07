namespace Avalonia.FuncUI.Library

module internal Hashing =
    open System
    open MBrace.FsPickler
    
    let private binarySerializer = FsPickler.CreateBinarySerializer()
    
    let hash (value: 'value) : string =
        binarySerializer.ComputeHash(value).Id