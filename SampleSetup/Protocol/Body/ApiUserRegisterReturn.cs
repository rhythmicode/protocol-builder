using System.Collections.Generic;
using SampleProject.Protocol.Context;

namespace SampleProject.Protocol.Body
{
    [UsingRef(nameof(AbstractReturn))]
    [UsingRef(nameof(ApiTokenPostReturn))]
    [UsingRef(nameof(CurrencyTypes), "..", nameof(Context))]
    public class ApiUserRegisterReturn : AbstractReturn
    {
        public int UserCurrencyTypeEnumId { get; set; } = (int)CurrencyTypes.EUR;

        public ApiTokenPostReturn? Token { get; set; } = null;

        public List<string>? ErrorCodes { get; set; } = null;
    }
}
