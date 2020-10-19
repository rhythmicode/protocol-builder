package SampleProject.Protocol.Context

enum class Languages(val rawValue: String) {
    English("en"),
        
    @Deprecated(message = "Please use Finnish")
    Suomi("fi-old"),
        
    Finnish("fi"),
}
