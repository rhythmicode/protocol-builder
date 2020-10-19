struct ApiBodyWithNested: Codable {
    var Prop1: String = ""
    var Prop2: Int? = nil

    struct Nested_1: Codable {
        var Prop_1_1: String = ""
        var Prop_1_2: Int? = nil

        struct Nested_1_2: Codable {
            var Prop_1_2_1: String = ""
            var Prop_1_2_2: Int? = nil
        }
    }
}
