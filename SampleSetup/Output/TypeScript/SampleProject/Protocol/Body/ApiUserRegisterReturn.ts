import { AbstractReturn } from './AbstractReturn';
import { ApiTokenPostReturn } from './ApiTokenPostReturn';
import { CurrencyTypes } from '../Context/CurrencyTypes';
export class ApiUserRegisterReturn extends AbstractReturn {
    UserCurrencyTypeEnumId: number = CurrencyTypes.EUR as number;

    Token: ApiTokenPostReturn | null = null;

    ErrorCodes: Array<string> | null = null;
}
