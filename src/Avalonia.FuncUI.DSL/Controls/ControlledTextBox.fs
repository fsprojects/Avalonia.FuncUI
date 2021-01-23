namespace Avalonia.FuncUI.Controls
open System
open System.Linq
open System.Collections.Generic
open Avalonia
open Avalonia.Controls
open Avalonia.Input
open Avalonia.Input.Platform
open Avalonia.Interactivity
open Avalonia.Styling

[<Struct>]
type State = { Text: string; CaretIdx: int }

type ControlledTextBox() =
    inherit TextBox()

    let sort a b =
        (min a b, max a b)
    
    let withoutSelection selectionStart selectionEnd str =
        if selectionStart = selectionEnd
        then { Text = str; CaretIdx = selectionStart }
        else
        let (min, max) = sort selectionStart selectionEnd
        { Text = str.Substring(0, min) + str.Substring(max); CaretIdx = min }
        
    let injectAtIdx idx str input =
        let len = String.length str        
        let txt = str.[0..idx - 1] + input + str.[idx..len]
        { Text = txt; CaretIdx = idx + String.length input }
        
    let replaceSelection selectionStart selectionEnd str input =
        let less = withoutSelection selectionStart selectionEnd str
        injectAtIdx (less.CaretIdx) (less.Text) input
                
    let synthesizeTextInputEvent (e: RoutedEventArgs) txt =
        let syntheticEvent = TextInputEventArgs()
        syntheticEvent.Text <- txt
        syntheticEvent.Route <- e.Route
        syntheticEvent.Source <- e.Source
        syntheticEvent.RoutedEvent <- e.RoutedEvent
        syntheticEvent
        
    let nullStrToEmpty str =
        match str with null -> "" | s -> s
        
    interface IStyleable with
        member this.StyleKey = typeof<TextBox>
        
    member val NextCaretIdx = 0 with get, set
    member val OnChangeCallback : TextInputEventArgs -> unit = ignore with get, set
    
    member private this.IsPasswordBox () =
        this.PasswordChar <> Unchecked.defaultof<char>
        
    member private this.InvokeChange (e, text, nextCaretIdx) =
        let syntheticEvent = synthesizeTextInputEvent e text
        
        this.ClearSelection()
        this.NextCaretIdx <- nextCaretIdx
        syntheticEvent |> this.OnChangeCallback
        syntheticEvent.Handled <- true
        
    member private this.HandleTextInput (e: TextInputEventArgs) =
        let input = e.Text |> this.RemoveInvalidCharacters
        let thisText = this.Text |> nullStrToEmpty
        let selectionLength = Math.Abs(this.SelectionStart - this.SelectionEnd)
        let nextLength = thisText.Length + input.Length - selectionLength
        let invalid =
            String.IsNullOrEmpty(input) || (this.MaxLength <> 0 && nextLength <= this.MaxLength)
        if not invalid then do
            let someSelection = this.SelectionStart <> this.SelectionEnd
            let next = if someSelection
                        then replaceSelection this.SelectionStart this.SelectionEnd thisText input
                        else injectAtIdx this.CaretIndex thisText input
            this.ClearSelection()
            this.NextCaretIdx <- next.CaretIdx
            let e' = synthesizeTextInputEvent e next.Text
            this.OnChangeCallback e'
            e'.Handled <- true
    
    override this.OnTextInput (e: TextInputEventArgs) =
        if not e.Handled then do
            this.HandleTextInput e
            e.Handled <- true
            
    member private this.HandleDeleteSelection(e: KeyEventArgs) =
        let next = withoutSelection this.SelectionStart this.SelectionEnd this.Text
        let syntheticEvent = synthesizeTextInputEvent e next.Text
        
        this.ClearSelection()
        this.NextCaretIdx <- this.SelectionStart
        syntheticEvent |> this.OnChangeCallback
        syntheticEvent.Handled <- true
            
    member private this.HandleCut(e: KeyEventArgs) =
        let someSelection = this.SelectionStart <> this.SelectionEnd
        if someSelection then do
            this.Copy()
            this.HandleDeleteSelection(e)       
            
    member private this.HandlePaste(e: KeyEventArgs) =
        let service = AvaloniaLocator.Current.GetService<IClipboard>()
        let text = service.GetTextAsync() |> Async.AwaitTask |> Async.RunSynchronously
        let syntheticEvent = synthesizeTextInputEvent e text
        do this.HandleTextInput syntheticEvent
        
    member private this.HandleCtrlBackspace(e: KeyEventArgs) =
        let text = this.Text.Substring(this.CaretIndex)
        do this.InvokeChange (e, text, 0)
                
    member private this.HandleBackspace(e: KeyEventArgs) =
        let shouldCleanupDanglingReturn =
            this.CaretIndex > 1 &&
            this.Text.[this.CaretIndex - 1] = '\n' &&
            this.Text.[this.CaretIndex - 2] = '\r'
        let removeCt = if shouldCleanupDanglingReturn then 2 else 1
        let text = this.Text.Substring(0, this.CaretIndex - removeCt) +
                   this.Text.Substring(this.CaretIndex)        
        do this.InvokeChange (e, text, (this.CaretIndex - removeCt))
    
    member private this.HandleCtrlDelete(e: KeyEventArgs) =
        let text = this.Text.Substring(0, this.CaretIndex)
        do this.InvokeChange (e, text, text.Length)
        
    member private this.HandleDelete(e: KeyEventArgs) =
        let shouldCleanupDanglingReturn =
            this.CaretIndex < this.Text.Length - 1 &&
            this.Text.[this.CaretIndex + 1] = '\n' &&
            this.Text.[this.CaretIndex] = '\r'
        let removeCt = if shouldCleanupDanglingReturn then 2 else 1
        let text = this.Text.Substring(0, this.CaretIndex) +
                   this.Text.Substring(this.CaretIndex + removeCt)        
        do this.InvokeChange (e, text, this.CaretIndex)
        
    member private this.HandleTab(e: KeyEventArgs) =
        let syntheticEvent = synthesizeTextInputEvent e "\t"
        do this.HandleTextInput syntheticEvent
        
    member private this.HandleReturn(e: KeyEventArgs) =
        let syntheticEvent = synthesizeTextInputEvent e this.NewLine
        do this.HandleTextInput syntheticEvent
        
    member this.HandleUndo(e: KeyEventArgs) =
        
        ()
        
    // This OnKeyDown override intercepts cases which would result in this.Text being mutated.
    // In these cases, non-destructive handlers pass the event to this.OnChangeCallback.
    // Outside of these cases, this override forwards the event to the base TextBox implementation. 
    override this.OnKeyDown (e: KeyEventArgs) =
        let keymap = AvaloniaLocator.Current.GetService<PlatformHotkeyConfiguration>()
        let matches (gestures: List<KeyGesture>) =
            gestures.Any(fun g -> g.Matches e)
        let isPw = this.IsPasswordBox()
        let someSelection = this.SelectionStart <> this.SelectionEnd
        let modifiers = e.KeyModifiers
        let hasWholeWordModifiers = modifiers.HasFlag(keymap.WholeWordTextActionModifiers);
        
        // TODO: Undo redo state
        if (matches keymap.Cut && not isPw) then
            this.HandleCut(e)
            e.Handled <- true
        elif (matches keymap.Paste) then
            this.HandlePaste(e)
            e.Handled <- true
        elif (e.Key = Key.Back) then
            if someSelection then
                this.HandleDeleteSelection(e)
            elif this.CaretIndex > 0 then
                if hasWholeWordModifiers
                then do this.HandleCtrlBackspace(e)
                else do this.HandleBackspace(e)
            e.Handled <- true
        elif (e.Key = Key.Delete) then            
            if someSelection then
                this.HandleDeleteSelection(e)
            elif this.CaretIndex < this.Text.Length - 1 then
                if hasWholeWordModifiers
                then do this.HandleCtrlDelete(e)
                else do this.HandleDelete(e)
            e.Handled <- true
        elif (e.Key = Key.Enter && this.AcceptsReturn) then
            this.HandleReturn(e)
            e.Handled <- true
        elif (e.Key = Key.Tab && this.AcceptsTab) then
            this.HandleTab(e)
            e.Handled <- true
        else do
            base.OnKeyDown(e)   
        ()                
        
    member this.MutateControlledValue str =
        this.Text <- str
        this.CaretIndex <- this.NextCaretIdx
                
