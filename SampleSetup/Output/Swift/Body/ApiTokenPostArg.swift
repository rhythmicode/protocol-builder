struct ApiTokenPostArg: Codable {
    @available(*, deprecated, message: "Use the other one")
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
