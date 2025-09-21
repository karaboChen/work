using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using work.DTO;
using work.Services;

namespace work.Controllers
{
    [ApiController]
    [Route("pharmacies")]
    public class PharmaciesController : ControllerBase
    {
        private readonly PharmaciesService  _pharmaciesService ;
        public PharmaciesController(PharmaciesService pharmaciesService)
        {
            _pharmaciesService = pharmaciesService;
        }

        [HttpGet("openHour")]
        [SwaggerOperation(Summary = "查詢特定時間、星期幾營業的藥局。", Description = "查詢特定時間、星期幾營業的藥局。")]
        public  async Task<ApiResult<List<ResGetPharmacies>>> GetPharmacies([FromQuery] ReqGetPharmacies req)
        {
            var data = await  _pharmaciesService.GetPharmacies(req);

            return (data.Any())
               ? ApiResult<List<ResGetPharmacies>>.Ok(data)
               : ApiResult<List<ResGetPharmacies>>.Fail("查無符合條件的資料");
        }


        [HttpGet("findMask")]
        [SwaggerOperation(Summary = "特定藥局銷售的所有口罩。", Description = "特定藥局銷售的所有口罩。")]
        public async Task<ApiResult<List<ResGetMasks>>> GetMasks([FromQuery] ReqGetMasks req)
        {
            var data = await _pharmaciesService.GetMasks(req);

            return (data.Any())
               ? ApiResult<List<ResGetMasks>>.Ok(data)
               : ApiResult<List<ResGetMasks>>.Fail("查無符合條件的資料");
        }

        [HttpGet("findPrice")]
        [SwaggerOperation(Summary = "列出特定價格範圍內口罩產品多於或少於的所有藥局。", Description = "列出特定價格範圍內口罩產品多於或少於的所有藥局。")]
        public async Task<ApiResult<List<ResGetPharmaciesByMaskPrice>>> GetPharmaciesByMaskPrice([FromQuery] ReqGetPharmaciesByMaskPrice req)
        {
            var data = await _pharmaciesService.GetPharmaciesByMaskPrice(req);

            return (data.Any())
               ? ApiResult<List<ResGetPharmaciesByMaskPrice>>.Ok(data)
               : ApiResult<List<ResGetPharmaciesByMaskPrice>>.Fail("查無符合條件的資料");
        }

        [HttpGet("getTopSpenders")]
        [SwaggerOperation(Summary = "檢索x某個日期範圍內口罩總交易金額排名靠前的使用者。", Description = "檢索x某個日期範圍內口罩總交易金額排名靠前的使用者。")]
        public async Task<ApiResult<List<ResGetTopSpenders>>> GetTopSpenders([FromQuery] ReqGetTopSpenders req)
        {
            var data = await _pharmaciesService.GetTopSpenders(req);

            return (data.Any())
               ? ApiResult<List<ResGetTopSpenders>>.Ok(data)
               : ApiResult<List<ResGetTopSpenders>>.Fail("查無符合條件的資料");
        }


        [HttpGet("getPurchaseSummary")]
        [SwaggerOperation(Summary = "計算某日期範圍內的口罩總數和交易總金額。", Description = "計算某日期範圍內的口罩總數和交易總金額。")]
        public async Task<ApiResult<List<ResGetPurchaseSummary>>> GetPurchaseSummary([FromQuery] ReqGetPurchaseSummary req)
        {
            var data = await _pharmaciesService.GetPurchaseSummary(req);

            return (data.Any())
               ? ApiResult<List<ResGetPurchaseSummary>>.Ok(data)
               : ApiResult<List<ResGetPurchaseSummary>>.Fail("查無符合條件的資料");
        }

        [HttpGet("searchPharmaciesAndMasks")]
        [SwaggerOperation(Summary = "按名稱搜尋藥局或口罩，並根據與搜尋字詞的相關性對結果進行排名。", Description = "按名稱搜尋藥局或口罩，並根據與搜尋字詞的相關性對結果進行排名。")]
        public async Task<ApiResult<List<ResSearchPharmaciesAndMasks>>> SearchPharmaciesAndMasks([FromQuery] ReqSearchPharmaciesAndMasks req)
        {
            var data = await _pharmaciesService.SearchPharmaciesAndMasks(req);
            return data;
        }

        [HttpPost("purchaseMasks")]
        [SwaggerOperation(Summary = "處理使用者購買口罩的流程，包括可能從不同藥局購買的情況。。", Description = "處理使用者購買口罩的流程，包括可能從不同藥局購買的情況。。")]
        public async Task<ApiResult<ResPurchaseMasks>> PurchaseMasks([FromBody] ReqPurchaseMasks req)
        {
            var data = await _pharmaciesService.PurchaseMasks(req);

            return data;
        }
    }
}
