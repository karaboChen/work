using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace DemoApi.Dtos
{
    public class Items
    {
        public int Id { get; set; }

        [SwaggerSchema(Description = "客戶名稱")]
        [DefaultValue("123")]
        public string Name { get; set; } = string.Empty;
    }
}
