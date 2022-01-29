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
        let faker = Bogus.Faker(locale = "de").Person

        { Contact.Id = Guid.NewGuid ()
          Contact.FullName = faker.FullName
          Contact.Mail = faker.Email
          Contact.Phone = faker.Phone }


type ContactStore () =
    let value = State [
         for _ = 0 to 100 do
             Contact.Random
    ]

    member this.Contacts with get () = value

[<RequireQualifiedAccess>]
module ContactStore =
    let shared = ContactStore ()