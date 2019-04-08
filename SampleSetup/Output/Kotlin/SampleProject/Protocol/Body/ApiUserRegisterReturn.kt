
package SampleProject.Protocol.Body

import SampleProject.Protocol.Body.*
open class ApiUserRegisterReturn: AbstractReturn() {
    var Token: ApiTokenPostReturn

    var ErrorCodes: ArrayList<String>
}
