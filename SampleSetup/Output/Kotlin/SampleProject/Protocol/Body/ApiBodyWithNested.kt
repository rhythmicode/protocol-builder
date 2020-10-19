package SampleProject.Protocol.Body

open class ApiBodyWithNested {
    var Prop1: String = ""
    var Prop2: Int? = null

    open class Nested_1 {
        var Prop_1_1: String = ""
        var Prop_1_2: Int? = null

        open class Nested_1_2 {
            var Prop_1_2_1: String = ""
            var Prop_1_2_2: Int? = null
        }
    }
}
