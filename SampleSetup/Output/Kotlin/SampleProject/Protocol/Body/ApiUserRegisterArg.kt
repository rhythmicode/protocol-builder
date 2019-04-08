
package SampleProject.Protocol.Body

open class ApiUserRegisterArg {
    var UserName: String = ""
    var UserNameKind: Int = 0
    var Password: String = ""

    var FirstName: String? = null

    var LastName: String = ""
    var IsGuest: Boolean = false
    var MobileMSISDN: String = ""
}
