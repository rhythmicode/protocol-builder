
    struct ApiUserRegisterReturn: Codable, AbstractReturn
 {
        var Token: ApiTokenPostReturn

        var ErrorCodes: [String]
    }

