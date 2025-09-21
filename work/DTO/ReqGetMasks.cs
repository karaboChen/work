using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace work.DTO
{
    public class ReqGetMasks
    {

        [SwaggerParameter(Description = "藥局名稱")]
        [DefaultValue("DFW Wellness")]
        public required string Name { get; set; }
    }
}
