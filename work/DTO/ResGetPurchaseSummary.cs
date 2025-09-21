using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace work.DTO
{
    public class ResGetPurchaseSummary
    {    

        [SwaggerParameter(Description = "總金額")]
        [DefaultValue(978.49)]
        public decimal TotalMoney { get; set; }


        [SwaggerParameter(Description = "總數量")]
        [DefaultValue(123)]
        public int TotalPack { get; set; }

    }
}
