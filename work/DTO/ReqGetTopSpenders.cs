using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Globalization;

namespace work.DTO
{
    public class ReqGetTopSpenders :IValidatableObject
    {
        [SwaggerParameter(Description = "日期 時間格式必須為 yyyy/MM/dd 或 時間格式必須為 yyyy-MM-dd")]
        [DefaultValue("2021-12-23")]
        [RegularExpression(@"^\d{4}[-/]\d{2}[-/]\d{2}$", ErrorMessage = "時間格式必須為 yyyy/MM/dd，例如 2021-12-23")]
        public required string Time { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var formats = new[] { "yyyy/MM/dd", "yyyy-MM-dd" };
            if (!DateTime.TryParseExact(Time, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                yield return new ValidationResult("時間格式錯誤，必須為 yyyy/MM/dd 或 yyyy-MM-dd", new[] { nameof(Time) });
                yield break;
            }

            var minDate = new DateTime(2001, 1, 1);
            var today = DateTime.Today;

            if (parsedDate < minDate || parsedDate > today)
            {
                yield return new ValidationResult($"日期必須介於 {minDate:yyyy-MM-dd} 與 {today:yyyy-MM-dd} 之間", new[] { nameof(Time) });
            }
        }
    }
}
