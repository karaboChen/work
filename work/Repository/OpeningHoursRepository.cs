using Dapper;
using Microsoft.Data.SqlClient;
using work.DTO;

namespace work.Repository
{
    public class OpeningHoursRepository
    {
        private readonly IConfiguration _configuration;
        public OpeningHoursRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<List<PharmacyOpeningDto>> GetPharmacies(ReqGetPharmacies req)
        {

            using var cn = new SqlConnection(_configuration.GetConnectionString("Db"));
            #region Sql 
            string sql = @"SELECT  c.Name,a.DayOfWeek,a.OpenTime, a.CloseTime
                            FROM [Demo].[dbo].[OpeningHours] a
                            JOIN [Demo].[dbo].[Pharmacy] c
                                ON a.PharmacyId = c.PharmacyId
                            WHERE a.DayOfWeek =@CheckDay   AND @CheckTime >= CAST(OpenTime AS TIME)
                              AND @CheckTime <  CAST(CloseTime AS TIME);";
            #endregion

           return  (await cn.QueryAsync<PharmacyOpeningDto>(sql, new { CheckDay = req.DayOfWeek, CheckTime = req.Time })).ToList();

        }

    }
}
