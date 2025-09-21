using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace work.DTO
{
    public class ResGetTopSpenders 
    {
        [SwaggerParameter(Description = "客戶名稱")]
        [DefaultValue("Yvonne Guerrero")]
        public string Name { get; set; } = "";

        [SwaggerParameter(Description = "總金額")]
        [DefaultValue(978.49)]
        public decimal TotalAmount { get; set; }
    }
}
