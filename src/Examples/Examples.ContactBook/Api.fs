[<RequireQualifiedAccess>]
module Examples.ContactBook.Api

    open System.IO
    open System.Net.Http
    open Avalonia.Media.Imaging


    let randomImage (gender: string) =
        async {
            use httpClient = new HttpClient()
            let! bytes =
                $"https://thispersondoesnotexist.com/"
                |> httpClient.GetByteArrayAsync
                |> Async.AwaitTask

            use stream = new MemoryStream(bytes)
            return new Bitmap(stream)
        }
