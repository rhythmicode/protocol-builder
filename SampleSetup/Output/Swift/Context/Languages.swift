enum Languages: String, Codable {
    case English = "en"

    @available(*, deprecated, message: "Please use Finnish")
    case Suomi = "fi-old"

    case Finnish = "fi"
}
