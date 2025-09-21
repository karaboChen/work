using Dapper;
using Microsoft.Data.SqlClient;

namespace work.Repository
{
    public class UserRepository
    {
        private readonly IConfiguration _configuration;
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(string,decimal)> CheckUser(int userId)
        {
            using var cn = new SqlConnection(_configuration.GetConnectionString("Db"));
            #region Sql 
            string sql = @"SELECT Name, CashBalance
                           FROM [Demo].[dbo].[Users]
                           WHERE UserId = @UserId";
            #endregion
            var userInfo = await cn.QueryFirstOrDefaultAsync<(string, decimal)>(sql, new { UserId = userId });
            return userInfo;
        }
        public async Task<int>  Payment(int userId ,decimal money)
        {
            using var cn = new SqlConnection(_configuration.GetConnectionString("Db"));

            #region Sql 
            string sql = @"   update [Demo].[dbo].[Users] 
                              set  CashBalance =@CashBalance
                              where  UserId  =@UserId";
            #endregion
            return  await cn.ExecuteAsync(sql, new { CashBalance = money, UserId = userId });

        }



    }
}
