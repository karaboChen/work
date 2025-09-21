using Dapper;
using Microsoft.Data.SqlClient;
using work.DTO;

namespace work.Repository
{
    public class PharmacyMaskRepository
    {
        private readonly IConfiguration _configuration;
        public PharmacyMaskRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        private string PharmacyMask()
        {
            return @"SELECT p.Name as PharmacyName ,m.Name as MaskName,Price,m.Color,m.Quantity
                           FROM   [Demo].[dbo].[PharmacyMask] pm
                           join   [Demo].[dbo].[Mask] m
                           on     pm.MaskId  = m.MaskId
                           join   [Demo].[dbo].[Pharmacy] p 
                           on     pm.PharmacyId = p.PharmacyId";
        }

        public async Task<List<MasksDto>> GetMasks(ReqGetMasks req)
        {

            using var cn = new SqlConnection(_configuration.GetConnectionString("Db"));
            #region Sql 
            string sql = PharmacyMask() + Environment.NewLine + @" WHERE p.Name LIKE @Name  ORDER BY m.Name DESC";
            #endregion
            var nameParam = req.Name + "%";

            return (await cn.QueryAsync<MasksDto>(sql, new { Name = nameParam })).ToList();
        }

        public async Task<List<MasksDto>> GetPharmaciesByMaskPrice(ReqGetPharmaciesByMaskPrice req)
        {  
            using var cn = new SqlConnection(_configuration.GetConnectionString("Db"));
            #region Sql 
            string sql = req.IsBig switch
            {
                true => PharmacyMask() + Environment.NewLine + "WHERE pm.Price >= @Price ORDER BY pm.Price",
                false => PharmacyMask() + Environment.NewLine + "WHERE pm.Price <= @Price ORDER BY pm.Price"
            };
            #endregion

            return (await cn.QueryAsync<MasksDto>(sql, new { Price = req.Price })).ToList();
        }


        public async Task<List<SearchPharmaciesAndMasksDto>> SearchPharmaciesAndMasks(ReqSearchPharmaciesAndMasks req)
        {
            using var cn = new SqlConnection(_configuration.GetConnectionString("Db"));
            #region Sql 
            string sql = @" select p.Name  as Pharmac, m.Name as Mask,ph.Price ,o.DayOfWeek ,o.OpenTime, o.CloseTime
                             FROM   [Demo].[dbo].[PharmacyMask] ph
                             join   [Demo].[dbo].[OpeningHours] o
                             on      ph.PharmacyId =  o.PharmacyId
                             join   [Demo].[dbo].[Mask] m
                             on     ph.MaskId  = m.MaskId
                             join   [Demo].[dbo].[Pharmacy] p 
                             on     ph.PharmacyId = p.PharmacyId
                             where   p.Name like  @Name  or  m.Name like @Name";
            #endregion
            var  list = (await cn.QueryAsync<SearchPharmaciesAndMasksDto>(sql,new { Name = req.Name +'%' })).ToList();
            return list;
        }


    }
}
