using Dapper;
using Dapper.Oracle;
using DecentralizedSystem.Core;
using DecentralizedSystem.Core.Entities;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DecentralizedSystem.Infrastructure.Repository
{
    public interface IMasterPackageRepository
    {
        public Task<MasterPackageResult> ExecuteMasterPackageAsync(MasterPackage entity);
    }

    public class MasterPackageRepository : IMasterPackageRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;

        private readonly string _packageName = "PKG_MIDDLEWARE";

        public MasterPackageRepository(IConfiguration configuration, IConnectionContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<MasterPackageResult> ExecuteMasterPackageAsync(MasterPackage entity)
        {
            var results = new MasterPackageResult();

            var param = new OracleDynamicParameters();
            param.Add(name: "P_TRANSACTION_ID", entity.PTransactionId, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            param.Add(name: "P_FUNCTION_ID", entity.PFunctionId, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            param.Add(name: "P_BRANCH_CODE", entity.PBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            param.Add(name: "P_INPUT_DETAILS", entity.PInputDetails, dbType: OracleMappingType.Clob, direction: ParameterDirection.Input);
            param.Add(name: "P_OUTPUT_DETAILS", dbType: OracleMappingType.Clob, direction: ParameterDirection.Output);
            param.Add(name: "P_ERR_CODE", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output, size: 4000);
            param.Add(name: "P_ERR_DESC", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output, size: 4000);
            using (var queryMultiDatas = await _context.Connection.QueryMultipleAsync($"{_packageName}.PR_MIDDLE", param: param, commandType: CommandType.StoredProcedure))
            {
                results.OutputDetail = param.Get<string>("P_OUTPUT_DETAILS"); ;
                results.ErrCode = param.Get<string>("P_ERR_CODE");
                results.ErrDesc = param.Get<string>("P_ERR_DESC");
            }
            return results;
        }
    }
}
