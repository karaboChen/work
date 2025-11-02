using System.Collections.Generic;
using DemoApi.Dtos;
using DemoApi.Tools;

namespace DemoApi.Services
{
    public class WeatherService
    {

        public async Task<ApiResult<List<Items>>> CreateItem(Items name)
        {
            await Task.Delay(500); // 模擬非同步操作
            // 🔸 商業邏輯驗證
            if (name.Name is "123") return ApiResult<List<Items>>.Fail("資料已存在");


            // ✅ 成功建立
            var item = new Items
            {
                Id = new Random().Next(1, 1000),
                Name = name.Name
            };
           var aa  = new List<Items>();
            aa.Add(item);
            return ApiResult<List<Items>>.Created(aa, "建立成功");
        }

    }
}
