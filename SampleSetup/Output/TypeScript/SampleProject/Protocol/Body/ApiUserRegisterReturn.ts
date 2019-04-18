import { AbstractReturn } from './AbstractReturn';
import { ApiTokenPostReturn } from './ApiTokenPostReturn';
export class ApiUserRegisterReturn extends AbstractReturn {
    Token: ApiTokenPostReturn;

    ErrorCodes: Array<string>;
}
