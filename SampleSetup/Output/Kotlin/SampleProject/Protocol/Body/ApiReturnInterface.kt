package SampleProject.Protocol.Body

interface ApiReturnInterface {
    fun Payload():         AbstractReturn
    fun Meta(): String
    fun Fill(p1: String?, p2: ArrayList<Int>, p3: ArrayList<Double>, p4: Int?): String
}
