using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace work.DTO
{
    public class ReqPurchaseMasks
    {

        [SwaggerSchema(Description = "客戶代號")]
        [DefaultValue(1)]
        public required int UserId { get; set; } 
       
        public List<MaskItem> Masks { get; set; } = [];
    }


    public class MaskItem
    {
        [SwaggerSchema(Description = "口罩代號")]
        [DefaultValue(1)]
        public required int MaskId { get; set; }   // 口罩代號


        [SwaggerSchema(Description = "藥局代號")]
        [DefaultValue(1)]
        public required int PharmacyId { get; set; }   // 口罩代號

        [SwaggerSchema(Description = "價格")]
        [DefaultValue(10.00)]
        public required decimal Price { get; set; } // 購買數量
    }




}
