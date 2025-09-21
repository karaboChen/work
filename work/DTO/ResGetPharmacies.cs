using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace work.DTO
{
    public class ResGetPharmacies
    {
        [SwaggerParameter(Description = "藥局名稱")]
        [DefaultValue("08:00")]
        public string Name { get; set; } = "";

        [SwaggerParameter(Description = "營業時間")]
        [DefaultValue("Mon 08:00- 12:00")]
        public string OpeningHours { get; set; } = "";
    }



}
