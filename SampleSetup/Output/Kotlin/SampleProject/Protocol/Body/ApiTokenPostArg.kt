package SampleProject.Protocol.Body

open class ApiTokenPostArg {
    @Deprecated(message = "Use the other one")
    var UserName: String = ""

    /**
     * A sample summary.
     */
    var UserNameKind: Int = 0

    /**
     * A multiline summary.
     * test explanations :)
     */
    var Password: String = ""
}
