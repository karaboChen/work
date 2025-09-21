using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace work.DTO
{
    public class ResPurchaseMasks
    {

        [SwaggerSchema(Description = "客戶名稱")]
        [DefaultValue("Geneva Floyd")]
        public required string User { get; set; } = "";

        [SwaggerSchema(Description = "客戶代號")]
        [DefaultValue(1)]
        public required int UserID { get; set; }

        public List<PurchaseHistoryResponseDto> Masks { get; set; } = [];
    }
}
