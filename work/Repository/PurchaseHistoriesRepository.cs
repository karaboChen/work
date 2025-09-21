using System.Globalization;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration.UserSecrets;
using work.DTO;

namespace work.Repository
{
    public class PurchaseHistoriesRepository
    {
        private readonly IConfiguration _configuration;
        public PurchaseHistoriesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

         private (DateTime StartDate, DateTime EndDate) GetDateRange(string dateString, int daysBefore)
        {
            string[] formats = { "yyyy-MM-dd", "yyyy/MM/dd" };

            if (!DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
            {
            }

            DateTime startDate = time.AddDays(-daysBefore).Date;
            DateTime endDate = time.Date.AddDays(1).AddTicks(-1); 

            return (startDate, endDate);
        }

        public async Task<List<GetTopSpendersDto>> GetTopSpenders(ReqGetTopSpenders req)
        {
            using var cn = new SqlConnection(_configuration.GetConnectionString("Db"));

            #region Sql 
            string sql = @"WITH UserTotals AS
                            (
                                SELECT 
                                    UserId,
                                    SUM(TransactionAmount) AS TotalAmount
                                FROM [Demo].[dbo].[PurchaseHistories]
                                WHERE TransactionDate BETWEEN @StartDate AND @EndDate
                                GROUP BY UserId
                            )
                            SELECT TOP 10
                                u.[Name] ,
                                ut.TotalAmount
                            FROM UserTotals ut
                            JOIN [Demo].[dbo].[Users] u
                                ON ut.UserId = u.UserId
                            ORDER BY ut.TotalAmount DESC;";
            #endregion

            var (startDate, endDate) = GetDateRange(req.Time, 15);
            return (await cn.QueryAsync<GetTopSpendersDto>(sql, new { StartDate = startDate, EndDate = endDate })).ToList();

        }


        public async Task<List<ResGetPurchaseSummary>> GetPurchaseSummary(ReqGetPurchaseSummary req)
        {
            using var cn = new SqlConnection(_configuration.GetConnectionString("Db"));
            #region sql
            var sql = @" SELECT sum( p.TransactionAmount )  as TotalMoney  , sum (m.Quantity) as TotalPack
                           FROM [Demo].[dbo].[PurchaseHistories] p
						         join [Demo].[dbo].[Mask] m
								 on p.MaskName = m.MaskId
                                WHERE TransactionDate BETWEEN @StartDate AND @EndDate ";
            #endregion

            var (startDate, endDate) = GetDateRange(req.Time, 15);
            return (await cn.QueryAsync<ResGetPurchaseSummary>(sql, new { StartDate = startDate, EndDate = endDate })).ToList();

        }

        public async Task<List<PurchaseHistoryDto>> AddPurchaseHistory(ReqPurchaseMasks req)
        {
            using var cn = new SqlConnection(_configuration.GetConnectionString("Db"));
            await cn.OpenAsync();

            using var transaction = cn.BeginTransaction();
            try
            {
                var insertedRecords = new List<PurchaseHistoryDto>();

                var sql = @"
                INSERT INTO [Demo].[dbo].[PurchaseHistories]
                (UserId, PharmacyName, MaskName, TransactionAmount, TransactionDate)
                OUTPUT inserted.PurchaseId, inserted.UserId, inserted.PharmacyName, inserted.MaskName, inserted.TransactionAmount, inserted.TransactionDate
                VALUES (@UserId, @PharmacyName, @MaskName, @TransactionAmount, GETDATE())";

                // 逐筆新增，取得 OUTPUT
                foreach (var item in req.Masks)
                {
                    var inserted = await cn.QuerySingleAsync<PurchaseHistoryDto>(
                        sql,
                        new
                        {
                            UserId = req.UserId,
                            PharmacyName = item.PharmacyId,
                            MaskName = item.MaskId,
                            TransactionAmount = item.Price
                        },
                        transaction: transaction
                    );

                    insertedRecords.Add(inserted);
                }

                transaction.Commit();
                return insertedRecords;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }


        public async Task<List<PurchaseHistoryResponseDto>> PurchaseHistoryList(IEnumerable<string> req)
        {
            using var cn = new SqlConnection(_configuration.GetConnectionString("Db"));

            string sql = @"
                              SELECT  ph.[PurchaseId]
                                  ,p.Name  as PharmacyName
                                  ,m.Name  as MaskName
                                  ,ph.[TransactionAmount]
                                  ,ph.[TransactionDate]
                                  ,m.Color
	                              ,m.Quantity
                              FROM [Demo].[dbo].[PurchaseHistories] ph
                                  join  [Demo].[dbo].[Pharmacy] p
                                  on     ph.PharmacyName = p.PharmacyId
                                  join  [Demo].[dbo].[Mask] m
                                  on     ph.MaskName = m.MaskId
                                  where  PurchaseId in @PurchaseId";
           return   (await cn.QueryAsync<PurchaseHistoryResponseDto>(sql, new { PurchaseId = req })).ToList();
        }


    }
}
