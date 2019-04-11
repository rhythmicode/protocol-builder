import { CurrencyTypes } from './CurrencyTypes';
import { Languages } from './Languages';
export class Constants {
    static DefaultCurrencyEnumId: number = CurrencyTypes.EUR as number;
    static DefaultLanguage: string = Languages.English;
    static HttpHeaderUserLanguage: string = "User-Language";
}
