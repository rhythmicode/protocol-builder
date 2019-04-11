import { AbstractReturn } from './AbstractReturn';
import { ApiTokenPostReturn } from './ApiTokenPostReturn';
export class ApiUserRegisterReturn {
    Token: ApiTokenPostReturn;

    ErrorCodes: Array<string>;
}
