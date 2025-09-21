using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace work.DTO
{
    public class ReqSearchPharmaciesAndMasks
    {
        [SwaggerParameter(Description = "搜尋藥局或口罩")]
        [DefaultValue("DFW Wellness")]
        public required string Name { get; set; }
    }
}
