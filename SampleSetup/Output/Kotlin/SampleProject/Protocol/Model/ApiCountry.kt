package SampleProject.Protocol.Model

import SampleProject.Protocol.Model.*
open class ApiCountry: AbstractModelWithId() {
    var TitleShort: String

    var TitleLong: String

    var PhoneCode: String

    var Code: String

    var Cities: ArrayList<ApiCity> = ArrayList<ApiCity>()

    var Addresses: ArrayList<ApiAddress> = ArrayList<ApiAddress>()

}
