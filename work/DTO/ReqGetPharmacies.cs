using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace work.DTO
{
    public class ReqGetPharmacies
    {

        [SwaggerParameter(Description = "特定時間")]
        [DefaultValue("08:00")]
        [RegularExpression(@"^(?:[01]\d|2[0-3]):[0-5]\d$", ErrorMessage = "時間格式必須為 HH:mm，例如 08:00")]
        public required string Time { get; set; } 

        [SwaggerParameter(Description = "1=星期一 ~ 7=星期日")]
        [DefaultValue(1)]
        [Range(1,7)]
        public required int DayOfWeek { get; set; }

    }
}
