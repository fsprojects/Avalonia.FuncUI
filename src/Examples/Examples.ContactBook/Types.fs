namespace Examples.ContactBook

open System
open Avalonia.FuncUI
open Bogus.DataSets

type Contact =
    { Id: Guid
      FullName: string
      Mail: string
      Phone: string
      Gender: string }

    static member Random with get () =
        let faker = Bogus.Faker(locale = "de").Person

        { Contact.Id = Guid.NewGuid ()
          Contact.FullName = faker.FullName
          Contact.Gender =
              match faker.Gender with
              | Name.Gender.Male -> "male"
              | Name.Gender.Female -> "female"
              | _ -> "person"
          Contact.Mail = faker.Email
          Contact.Phone = faker.Phone }


type ContactStore () =
    let value = new State<_>([
         for _ = 0 to 100 do
             Contact.Random
    ])

    member this.Contacts with get () = value

[<RequireQualifiedAccess>]
module ContactStore =
    let shared = ContactStore ()