
    struct ApiAddress: Codable, AbstractModelWithId
 {
        var CountryId: String

        var Country: ApiCountry

        var CityId: String

        var City: ApiCity

        var PostalCode: String

        var AddressLine1: String

        var AddressLine2: String

        var GeoLat: Double?

        var GeoLon: Double?

        var Description: String
    }

