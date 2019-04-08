
import { AbstractModelWithId } from './AbstractModelWithId';
import { ApiCountry } from './ApiCountry';
import { ApiCity } from './ApiCity';
export class ApiAddress extends AbstractModelWithId {
    CountryId: string;

    Country: ApiCountry;

    CityId: string;

    City: ApiCity;

    PostalCode: string;

    AddressLine1: string;

    AddressLine2: string;

    GeoLat: number | null;

    GeoLon: number | null;

    Description: string;
}
