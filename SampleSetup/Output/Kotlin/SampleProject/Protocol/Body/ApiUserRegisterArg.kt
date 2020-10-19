package SampleProject.Protocol.Body

open class ApiUserRegisterArg {
    var UserName: String = ""

    var UserNameKind: Int = 0

    var Password: String = ""

    var FirstName: String? = null

    var LastName: String = ""

    var IsGuest: Boolean = false

    @Deprecated(message = "Please use MobileMSISDN")
    var Mobile: String = ""

    var MobileMSISDN: String = ""
}
