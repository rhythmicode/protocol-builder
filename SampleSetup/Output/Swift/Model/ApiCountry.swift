    struct ApiCountry: Codable, AbstractModelWithId
 {
        var TitleShort: String

        var TitleLong: String

        var PhoneCode: String

        var Code: String

        var Cities: [ApiCity] = [ApiCity]()

        var Addresses: [ApiAddress] = [ApiAddress]()

    }

