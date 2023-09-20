using Microsoft.Extensions.Configuration;
using Dapper;
using DecentralizedSystem.Infrastructure.Interfaces;
using DecentralizedSystem.Core.Entities;
using Dapper.Oracle;
using System.Data;

namespace DecentralizedSystem.Infrastructure.Repository
{
    public interface IAccountTranFccRepository : IGenericRepository<AccountTranFcc>
    {
        /// <summary>
        /// Lấy thông tin giao dịch bán trên core
        /// </summary>
        /// <param name="cif"></param>
        /// <returns></returns>
        Task<IReadOnlyList<AccountTranFcc>> GetSellAllAsync(string cif, string customFieldData);

        /// <summary>
        /// Lấy thông tin giao dịch mua trên core
        /// </summary>
        /// <param name="cif"></param>
        /// <returns></returns>
        Task<IReadOnlyList<AccountTranFcc>> GetBuyAllAsync(string cif, string customFieldData);
        Task<IReadOnlyList<AccountTranFcc>> CalculatorAsync(string productCode, string customFieldData, string accountNo);
        Task<IReadOnlyList<AccountDetailCASA>> GetAccountDetailsAsync(string cif);
        Task<bool> CompletedLoan(string accNo);
        Task<List<UserList>> GetUserListAsync(string branchCode, string status);
    }
    public class AccountTranFccRepository : IAccountTranFccRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        private readonly IUnitOfWorkContext _unitOfWork;
        public AccountTranFccRepository(IConfiguration configuration, IConnectionContext context, IUnitOfWorkContext unitOfWork)
        {
            this._configuration = configuration;
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public Task<int> AddAsync(AccountTranFcc entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(AccountTranFcc id)
        {
            throw new NotImplementedException();
        }
        public Task<int> UpdateAsync(AccountTranFcc entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> ApproveAsync(AccountTranFcc entity)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResults<AccountTranFcc>> QueryAsync(string userCreate, string status, int pageNum = 0, int pageSize = int.MaxValue)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<AccountTranFcc>> GetSellAllAsync(string cif, string customFieldData)
        {
            var paramQuery = new OracleDynamicParameters();
            paramQuery.Add(name: "V_CIF", cif, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuery.Add(name: "V_CUSTOM_FIELD_DATA", customFieldData, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuery.Add(name: "P_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var queryMultiDatas = await this._context.Connection.QueryMultipleAsync("TKGTCG_PUSH_CORE_PKG.GET_ALL_ACCOUNT_SELL_BY_CIF", param: paramQuery, commandType: CommandType.StoredProcedure);
            return queryMultiDatas.Read<AccountTranFcc>().ToList();
        }

        public async Task<IReadOnlyList<AccountTranFcc>> GetBuyAllAsync(string cif, string customFieldData)
        {
            var paramQuery = new OracleDynamicParameters();
            paramQuery.Add(name: "V_CIF", cif, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuery.Add(name: "V_CUSTOM_FIELD_DATA", customFieldData, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuery.Add(name: "P_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var queryMultiDatas = await this._context.Connection.QueryMultipleAsync("TKGTCG_PUSH_CORE_PKG.GET_ALL_ACCOUNT_BUY_BY_CIF", param: paramQuery, commandType: CommandType.StoredProcedure);
            return queryMultiDatas.Read<AccountTranFcc>().ToList();
        }


        public async Task<IReadOnlyList<AccountTranFcc>> CalculatorAsync(string productCode, string customFieldData, string accountNo)
        {
            var paramQuery = new OracleDynamicParameters();
            paramQuery.Add(name: "V_PRODUCT_CODE", productCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuery.Add(name: "V_CUSTOM_FIELD_DATA", customFieldData, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuery.Add(name: "V_ACCOUNT_NO", accountNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuery.Add(name: "P_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var queryMultiDatas = await this._context.Connection.QueryMultipleAsync("TKGTCG_PUSH_CORE_PKG.CALCULATOR_SELL", param: paramQuery, commandType: CommandType.StoredProcedure);
            return queryMultiDatas.Read<AccountTranFcc>().ToList();
        }

        public async Task<IReadOnlyList<AccountDetailCASA>> GetAccountDetailsAsync(string cif)
        {
            var paramQuery = new OracleDynamicParameters();
            paramQuery.Add(name: "V_CIF", cif, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuery.Add(name: "P_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var queryMultiDatas = await this._context.Connection.QueryMultipleAsync("TKGTCG_PUSH_CORE_PKG.GET_ACCOUNT_DETAIL", param: paramQuery, commandType: CommandType.StoredProcedure);
            return queryMultiDatas.Read<AccountDetailCASA>().ToList();
        }

        public async Task<bool> CompletedLoan(string accNo)
        {
            var paramQuery = new OracleDynamicParameters();
            paramQuery.Add(name: "V_ACC_NO", accNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            await this._context.Connection.ExecuteAsync("TKGTCG_PUSH_CORE_PKG.COMPLETED_LOAN", param: paramQuery, commandType: CommandType.StoredProcedure);
            return true;
        }

        Task<AccountTranFcc> IGenericRepository<AccountTranFcc>.GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<AccountTranFcc>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserList>> GetUserListAsync(string branchCode, string status)
        {
            var results = new List<UserList>();

            var paramQuery = new OracleDynamicParameters();
            paramQuery.Add(name: "v_branch_code", branchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuery.Add(name: "v_status", status, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuery.Add(name: "V_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            using (var uow = _unitOfWork.Create())
            {
                try
                {
                    var queryResult = await _context.Connection.QueryMultipleAsync($"CORE_PKG.GET_LIST_USER_FCC", param: paramQuery, commandType: CommandType.StoredProcedure);
                    results = queryResult.Read<UserList>().ToList();

                }
                catch (Exception ex)
                {
                    await uow.RollBackAsync();
                    throw new ArgumentException(ex.Message);
                }
            }
            return results;
        }
    }
}
