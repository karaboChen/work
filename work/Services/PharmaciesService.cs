using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using work.DTO;
using work.Repository;

namespace work.Services
{
    public class PharmaciesService
    {
        private readonly OpeningHoursRepository _openingHoursRepository;
        private readonly PharmacyMaskRepository _pharmacyMaskRepository;
        private readonly PurchaseHistoriesRepository _purchaseHistoriesRepository;
        private readonly UserRepository _userRepository;
        private readonly PharmacyRepository _pharmacyRepository;

        public PharmaciesService(
            OpeningHoursRepository openingHoursRepository,
            PharmacyMaskRepository pharmacyMaskRepository,
            PurchaseHistoriesRepository purchaseHistoriesRepository,
            UserRepository userRepository,
            PharmacyRepository pharmacyRepository)
        {
            _openingHoursRepository = openingHoursRepository;
            _pharmacyMaskRepository = pharmacyMaskRepository;
            _purchaseHistoriesRepository = purchaseHistoriesRepository;
            _userRepository = userRepository;
            _pharmacyRepository = pharmacyRepository;
        }


        private Dictionary<int, string> ChangeDay()
        {
            return new Dictionary<int, string>()
            {
                {1,"Mon" },
                {2,"Tue" },
                {3,"Wed" },
                {4,"Thur" },
                {5,"Fri" },
                {6,"Sat" },
                {7,"Sun" },
            };
        }

        public async Task<List<ResGetPharmacies>> GetPharmacies(ReqGetPharmacies req)
        {    
            var ans  = new List<ResGetPharmacies>();

            var data    =  await _openingHoursRepository.GetPharmacies(req);

            #region 格是轉換
            foreach (var item in data)
            {
                ans.Add(new ResGetPharmacies()
                {
                    Name = item.Name,
                    OpeningHours = $"{ChangeDay()[item.DayOfWeek]} {item.OpenTime:hh\\:mm}- {item.CloseTime:hh\\:mm}"
                });
            }
            #endregion
            return ans;
        }


        public async Task<List<ResGetMasks>> GetMasks(ReqGetMasks req)
        {
            var maskList  =   await _pharmacyMaskRepository.GetMasks(req);

            var reqGetMasks  =new List<ResGetMasks>();


            maskList.GroupBy(m => m.PharmacyName).ToList().ForEach(g =>
            {
                reqGetMasks.Add(new ResGetMasks()
                {
                    PharmacyName = g.Key,
                    Masks = g
                    .Select(m => new Mask()
                    {
                        MaskName = m.MaskName +$" ({m.Color}) "+ $" ({m.Quantity} per pack)",
                        Price = m.Price
                    })
                    .OrderByDescending(m => m.Price)   // 按價格排序
                    .ToList()
                });
            });

            return reqGetMasks;
        }


        public async Task<List<ResGetPharmaciesByMaskPrice>> GetPharmaciesByMaskPrice(ReqGetPharmaciesByMaskPrice req)
        {
            var maskList = await _pharmacyMaskRepository.GetPharmaciesByMaskPrice(req);


            var reqGetMasks = new List<ResGetPharmaciesByMaskPrice>();


            maskList.GroupBy(m => m.PharmacyName).ToList().ForEach(g =>
            {
                reqGetMasks.Add(new ResGetPharmaciesByMaskPrice()
                {
                    PharmacyName = g.Key,
                    Masks = g
                    .Select(m => new Mask()
                    {
                        MaskName = m.MaskName + $" ({m.Color}) " + $" ({m.Quantity} per pack)",
                        Price = m.Price
                    })
                    .OrderByDescending(m => m.Price)   // 按價格排序
                    .ToList()
                });
            });
            return reqGetMasks;
        }


        public async Task<List<ResGetTopSpenders>> GetTopSpenders(ReqGetTopSpenders req) 
        {
             var  userList =  await _purchaseHistoriesRepository.GetTopSpenders(req);

            var resGetTopSpenders = userList
                .Select(item => new ResGetTopSpenders
                {
                    Name = item.Name,
                    TotalAmount = item.TotalAmount
                })
                .ToList();

            return resGetTopSpenders;
        }


        public async Task<List<ResGetPurchaseSummary>> GetPurchaseSummary(ReqGetPurchaseSummary req)
        {
             return  await _purchaseHistoriesRepository.GetPurchaseSummary(req);
        }


       public async Task<ApiResult<ResPurchaseMasks>> PurchaseMasks(ReqPurchaseMasks req)
        {    
            //先確定使用者存在 和 餘額
            var (name, money)   =  await _userRepository.CheckUser(req.UserId);
            var totalAmount = req.Masks.Sum(m => m.Price);
            if (string.IsNullOrWhiteSpace(name)) return ApiResult<ResPurchaseMasks>.Fail("使用者不存在");
            if (money is 0  || money  < totalAmount) return ApiResult<ResPurchaseMasks>.Fail("餘額不足，無法扣款");

            //開始交易 先扣 user 款
            decimal newBalance = money - totalAmount;
            int isPay =  await _userRepository.Payment(req.UserId, newBalance);
            if (isPay is  0) return ApiResult<ResPurchaseMasks>.Fail("扣款失敗，請稍後再試");

            //改變 藥局金額  
             await _pharmacyRepository.ChangeAmount(req);

            //新增 交易紀錄
            var purchaseHistoriesList =  await _purchaseHistoriesRepository.AddPurchaseHistory(req);

            //轉成回傳格式
            IEnumerable<string> keys = purchaseHistoriesList.Select(x => x.PurchaseId);
            var purchaseRecord =  await _purchaseHistoriesRepository.PurchaseHistoryList(keys);
            var res = new ResPurchaseMasks()
            {
                User = name,
                UserID = req.UserId,
                Masks = purchaseRecord.Select(x => new PurchaseHistoryResponseDto
                {
                    PurchaseId = x.PurchaseId,
                    PharmacyName = x.PharmacyName,
                    MaskName = x.MaskName + $" ({x.Color}) " + $" ( {x.Quantity} per pack )",
                    TransactionAmount = x.TransactionAmount,
                    TransactionDate = DateTime.Parse(x.TransactionDate).ToString("yyyy-MM-dd HH:mm:ss")
                }).ToList()
            };

            return ApiResult<ResPurchaseMasks>.Ok(res);
        }



       public async Task<ApiResult<List<ResSearchPharmaciesAndMasks>>> SearchPharmaciesAndMasks(ReqSearchPharmaciesAndMasks req)
       {
            var searchList =  await _pharmacyMaskRepository.SearchPharmaciesAndMasks(req);
            if (!searchList.Any()) return ApiResult<List<ResSearchPharmaciesAndMasks>>.Fail("查無符合條件的資料");

            // Group by 藥局
            var  formatList = searchList
                .GroupBy(x => x.Pharmac)
                .Select(pharmacyGroup => new ResSearchPharmaciesAndMasks
                {
                    Name = pharmacyGroup.Key,

                    // 整理營業時間
                    OpeningHours = BuildOpeningHours(pharmacyGroup),

                    // 整理口罩清單
                    Masks = pharmacyGroup
                        .GroupBy(x => new { x.Mask, x.Price })
                        .Select(m => new Mask
                        {
                            MaskName = m.Key.Mask,
                            Price = m.Key.Price
                        })
                        .ToList()
                })
                .ToList();
            return ApiResult<List<ResSearchPharmaciesAndMasks>>.Ok(formatList);
        }


        private string BuildOpeningHours(IEnumerable<SearchPharmaciesAndMasksDto> rows)
        {
            var dayMap = ChangeDay(); // 1~7 → Mon-Sun

            var timeGroups = rows
                .GroupBy(r => new { r.OpenTime, r.CloseTime })
                .Select(g => new
                {
                    g.Key.OpenTime,
                    g.Key.CloseTime,
                    Days = g.Select(x => x.DayOfWeek).Distinct().OrderBy(d => d).ToList() 
                })
                .ToList();

            var parts = new List<string>();

            foreach (var group in timeGroups)
            {
                var daysStr = CompressDays(group.Days);

                // 這裡轉換成星期縮寫
                var dayTokens = daysStr.Split(',');
                var converted = dayTokens.Select(token =>
                {
                    if (token.Contains('-'))
                    {
                        var range = token.Split('-').Select(int.Parse).ToArray();
                        return $"{dayMap[range[0]]}-{dayMap[range[1]]}";
                    }
                    else
                    {
                        return dayMap[int.Parse(token)];
                    }
                });

                var finalDaysStr = string.Join(",", converted);
                parts.Add($"{finalDaysStr} {group.OpenTime.ToString().Substring(0, 5)}-{group.CloseTime.ToString().Substring(0, 5)}");
            }

            return string.Join(" / ", parts);
        }



        private string CompressDays(List<int> days)
        {
            var result = new List<string>();
            int start = days[0], prev = days[0];

            for (int i = 1; i < days.Count; i++)
            {
                if (days[i] == prev + 1)  // 連續
                {
                    prev = days[i];
                }
                else
                {
                    result.Add(start == prev ? $"{start}" : $"{start}-{prev}");
                    start = prev = days[i];
                }
            }
            result.Add(start == prev ? $"{start}" : $"{start}-{prev}");
            return string.Join(",", result);
        }

    }
}
