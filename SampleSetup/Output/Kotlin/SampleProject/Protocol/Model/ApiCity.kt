
package SampleProject.Protocol.Model

import SampleProject.Protocol.Model.*
open class ApiCity: AbstractModelWithId() {
    var CountryId: String

    var Country: ApiCountry

    var TitleShort: String

    var TitleLong: String

    var Addresses: ArrayList<ApiAddress>

}
