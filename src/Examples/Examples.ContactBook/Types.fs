namespace Examples.ContactBook

open System
open Avalonia.FuncUI

type Contact =
    { Id: Guid
      FullName: string
      Mail: string
      Phone: string }

    static member Create (fullName, mail, phone) =
        { Contact.Id = Guid.NewGuid ()
          Contact.FullName = fullName
          Contact.Mail = mail
          Contact.Phone = phone }

    static member Init with get () =
        { Contact.Id = Guid.Empty
          Contact.FullName = ""
          Contact.Mail = ""
          Contact.Phone = "" }

    static member Random with get () =
        let faker = Bogus.Faker(locale = "de")

        { Contact.Id = Guid.NewGuid ()
          Contact.FullName = faker.Name.FullName ()
          Contact.Mail = faker.Internet.Email ()
          Contact.Phone = faker.Phone.Locale }


type ContactStore () =
    let value = Value [
         for _ = 0 to 100 do
             Contact.Random
    ]

    member this.Contacts with get () = value

[<RequireQualifiedAccess>]
module ContactStore =
    let shared = ContactStore ()