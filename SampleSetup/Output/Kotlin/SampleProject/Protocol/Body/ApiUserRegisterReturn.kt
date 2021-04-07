package SampleProject.Protocol.Body

import SampleProject.Protocol.Context.*
open class ApiUserRegisterReturn: AbstractReturn() {
    var UserCurrencyTypeEnumId: Int = (CurrencyTypes.EUR.rawValue).toInt()

    var Token: ApiTokenPostReturn? = null

    var ErrorCodes: ArrayList<String>? = null
}
