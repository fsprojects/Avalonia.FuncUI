[<RequireQualifiedAccess>]
module Examples.ContactBook.Api

    open System.IO
    open System.Net.Http
    open Avalonia.Media.Imaging

    let private randomImageUri = "https://thispersondoesnotexist.com/image"

    let randomImage =
        async {
            use httpClient = new HttpClient()
            let! bytes =
                randomImageUri
                |> httpClient.GetByteArrayAsync
                |> Async.AwaitTask

            use stream = new MemoryStream(bytes)
            return new Bitmap(stream)
        }
