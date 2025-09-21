using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace work.DTO
{
    public class ResGetMasks
    {
        [SwaggerParameter(Description = "藥局名稱")]
        [DefaultValue("Centrico")]
        public string PharmacyName { get; set; } = string.Empty;   // 藥局名稱

        public List<Mask> Masks { get; set; } = [];                 // 口罩清單
    }

    public class Mask
    {
        [SwaggerParameter(Description = "口罩名稱")]
        [DefaultValue("True Barrier (green) (10 per pack)")]
        public string MaskName { get; set; } = string.Empty;   // 口罩名稱

        [SwaggerParameter(Description = "價格")]
        [DefaultValue("38.33")]
        public decimal Price { get; set; }                 // 價格
    }
}
