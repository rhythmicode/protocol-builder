
require('./CurrencyTypes');
require('./Languages');
<?php class Constants {
    DefaultCurrencyEnumId: Int = Int(CurrencyTypes.EUR)
    DefaultLanguage: String = Languages.English
    HttpHeaderUserLanguage: String = "User-Language"
}
