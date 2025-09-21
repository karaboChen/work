using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace work.DTO
{
    public class ReqGetPharmaciesByMaskPrice
    {
        [SwaggerParameter(Description = "大於(true) 或小於")]
        [DefaultValue(false)]
        public required bool IsBig { get; set; }

        [SwaggerParameter(Description = "價格")]
        [DefaultValue("12")]
        public required int Price { get; set; }
    }
}
