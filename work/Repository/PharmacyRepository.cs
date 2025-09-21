using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using work.DTO;

namespace work.Repository
{
    public class PharmacyRepository
    {
        private readonly IConfiguration _configuration;
        public PharmacyRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ChangeAmount(ReqPurchaseMasks req)
        {
            using var cn = new SqlConnection(_configuration.GetConnectionString("Db"));
            await cn.OpenAsync();

            using var transaction = cn.BeginTransaction();
            try
            {

                string sql = @"
                                UPDATE [Demo].[dbo].[Pharmacy]
                                SET CashBalance = CashBalance + @CashBalance
                                WHERE PharmacyId = @PharmacyId";
                foreach (var mask in req.Masks)
                {

                    var rows = await cn.ExecuteAsync(sql,
                        new { CashBalance = mask.Price, PharmacyId = mask.PharmacyId },
                        transaction: transaction);

                    if (rows == 0)
                    {
                        throw new Exception($"扣款失敗，藥局ID: {mask.PharmacyId}");
                    }
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

        }


    }
}
