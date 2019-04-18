import { AbstractModelWithId } from './AbstractModelWithId';
import { ApiCity } from './ApiCity';
import { ApiAddress } from './ApiAddress';
export class ApiCountry extends AbstractModelWithId {
    TitleShort: string;

    TitleLong: string;

    PhoneCode: string;

    Code: string;

    Cities: Array<ApiCity>;

    Addresses: Array<ApiAddress>;

}
