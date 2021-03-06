import { AbstractModelWithId } from './AbstractModelWithId';
import { ApiCountry } from './ApiCountry';
import { ApiAddress } from './ApiAddress';
export class ApiCity extends AbstractModelWithId {
    CountryId: string;

    Country: ApiCountry;

    TitleShort: string;

    TitleLong: string;

    Addresses: Array<ApiAddress>;

}
