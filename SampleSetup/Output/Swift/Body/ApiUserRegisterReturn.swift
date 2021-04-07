    struct ApiUserRegisterReturn: Codable, AbstractReturn
 {
        var UserCurrencyTypeEnumId: Int = Int(CurrencyTypes.EUR)

        var Token: ApiTokenPostReturn? = nil

        var ErrorCodes: [String]? = nil
    }

