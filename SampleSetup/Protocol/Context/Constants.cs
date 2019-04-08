using CurrencyTypes = SampleProject.Protocol.Context.CurrencyTypes;
using Languages = SampleProject.Protocol.Context.Languages;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Protocol.Context
{
    public static class Constants
    {
        public const int DefaultCurrencyEnumId = (int)CurrencyTypes.EUR;
        public const string DefaultLanguage = Languages.English;
        public const string HttpHeaderUserLanguage = "User-Language";
    }
}
