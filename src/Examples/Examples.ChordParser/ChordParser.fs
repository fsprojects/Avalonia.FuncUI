/// Copied from
/// https://github.com/JordanMarr/FSharp.ChordParser
module ChordParser

module Domain = 
    let rootNotes = ["A"; "A#"; "Bb"; "B"; "C"; "C#"; "Db"; "D"; "D#"; "Eb"; "E"; "F"; "F#"; "Gb"; "G"; "G#"; "Ab"]
    
    type Chord = 
        {
            Root: string
            Tonality: string option
            Extension: string option
            BassNote: string option
        }
    
    /// Transposes the root note of a chord and optional bass note to the given number of semitones, using the preferred accidental (# or b).
    let transpose semitones preferredAccidental chord =
        let sharps = rootNotes |> List.filter (fun n -> not (n.EndsWith "b"))
        let flats = rootNotes |> List.filter (fun n -> not (n.EndsWith "#"))
        let notes  = 
            match preferredAccidental with 
            | "#" -> sharps 
            | "b" -> flats
            | _ -> failwith "Preferred Accidental must be either # or b."
    
        let transposeNote note =
            let sharpIdx = sharps |> List.tryFindIndex(fun n -> n = note)
            let flatIdx = flats |> List.tryFindIndex(fun n -> n = note)
            let idx = sharpIdx |> Option.orElse flatIdx |> Option.defaultValue -1
            if idx = -1 then failwith $"Invalid note: '{note}'"
            let transposedIdx = (idx + semitones) % 12
            let transposedIdx = if transposedIdx < 0 then transposedIdx + 12 else transposedIdx
            notes.[transposedIdx]
    
        { chord with 
            Root = chord.Root |> transposeNote
            BassNote = chord.BassNote |> Option.map transposeNote }
    
    /// Prints chord to string.
    let printChord chord =
        let tonality = chord.Tonality |> Option.defaultValue ""
        let extension = chord.Extension |> Option.defaultValue ""
        let bassNote = chord.BassNote |> Option.map (sprintf " /%s") |> Option.defaultValue ""
        $"({chord.Root}{tonality}{extension}{bassNote})"
    
module Parser = 
    open FParsec
    open Domain
    
    type ChordChart =
        | Lyrics of string
        | Chord of Chord    
    
    let str s = pstring s
    let strCI s = pstringCI s
    let ws = spaces
    
    let createChord (((root, tonality), ext), bassNote) =
        { Root = root
          Tonality = tonality
          Extension = ext
          BassNote = bassNote }
        |> ChordChart.Chord
    
    let chord = 
        let note = 
            rootNotes
            |> List.sortByDescending (fun n -> n.Length) // "C#" must be before "C" to avoid a "partial" consumption of "C" that prevents "C#" from matching.
            |> List.map str
            |> choice
            |>> string
    
        let tonality = 
            [strCI "maj"; str "M"; strCI  "min"; str "m"; str "-"; strCI "dim"; str "o"; strCI "aug"; str "+"; str "+5"] // Order matters to avoid partial consumption bugs
            |> choice 
            |>> string
    
        let tonality = opt (skipChar ' ') >>. tonality
        let extension = many1Chars digit |>> string
        let bassNote = opt (skipChar ' ') >>. skipChar '/' >>. note
        
        skipChar '(' >>. note .>>. opt tonality .>>. opt extension .>>. opt bassNote .>> skipChar ')' |>> createChord
    
    let lyric = many1Chars (noneOf "(") .>> spaces |>> (string >> ChordChart.Lyrics)
    
    let chordChart = many (lyric <|> chord)
    
    /// Parses a chord chart.
    let parse text = 
        match run chordChart text with
        | Success (ast,_,_) -> ast
        | Failure (_,error,_) -> failwith (error.ToString())




open System.Text
open Domain
    
/// Parses and processes the chordchart items.
let processText (semitones: int) (preferredAccidental: string) (ucase: bool) (text: string) = 
    Parser.parse text
    |> List.map (function
        | Parser.Lyrics lyrics -> if ucase then lyrics.ToUpper() else lyrics
        | Parser.Chord chord -> chord |> transpose semitones preferredAccidental |> printChord)
    |> List.fold (fun (sb: StringBuilder) txt -> sb.Append txt) (StringBuilder())
    |> string
    
let tryProcessText (semitones: int) (preferredAccidental: string) (ucase: bool) (text: string) = 
    try 
        let output = processText semitones preferredAccidental ucase text
        Ok output
    with ex ->
        Error ex.Message