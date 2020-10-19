struct ApiUserRegisterArg: Codable {
    var UserName: String = ""

    var UserNameKind: Int = 0

    var Password: String = ""

    var FirstName: String? = nil

    var LastName: String = ""

    var IsGuest: Bool = false

    @available(*, deprecated, message: "Please use MobileMSISDN")
    var Mobile: String = ""

    var MobileMSISDN: String = ""
}
