protocol ApiReturnInterface {
    func Payload() ->         AbstractReturn
    func Meta() -> String
    func Fill(_ p1: String?, _ p2: [Int], _ p3: [Double], _ p4: Int?) -> String
}
