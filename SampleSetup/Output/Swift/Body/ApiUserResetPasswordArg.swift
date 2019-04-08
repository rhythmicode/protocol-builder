
struct ApiUserResetPasswordArg: Codable {
    var UserId: String

    var Token: String

    var NewPassword: String
}
