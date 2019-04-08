
    struct ApiCity: Codable, AbstractModelWithId
 {
        var CountryId: String

        var Country: ApiCountry

        var TitleShort: String

        var TitleLong: String

        var Addresses: [ApiAddress]

    }

