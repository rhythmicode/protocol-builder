
struct ApiUserRegisterArg: Codable {
    var UserName: String = ""
    var UserNameKind: Int = 0
    var Password: String = ""

    var FirstName: String? = nil

    var LastName: String = ""
    var IsGuest: Bool = false
    var MobileMSISDN: String = ""
}
