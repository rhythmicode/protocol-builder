package SampleProject.Protocol.Body

import SampleProject.Protocol.Context.*
open class ApiUserRegisterReturn: AbstractReturn() {
    var UserCurrencyTypeEnumId: Int = (CurrencyTypes.EUR).toInt()

    var Token: ApiTokenPostReturn

    var ErrorCodes: ArrayList<String>
}
