using DecentralizedSystem.Core.Entities;
using DecentralizedSystem.Helpers;
using DecentralizedSystem.Infrastructure.Repository;
using DecentralizedSystem.Models.AccountTranFcc;
using DecentralizedSystem.Models.Fcc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DecentralizedSystem.Services.Account
{
    public interface IAccountTranFccService
    {
        Task<bool> CompletedLoanAsync(string accNo);
        Task<List<AccountDetailCASAModel>> GetAccountDetailsAsync(string cif);
        Task<List<UserListModel>> GetUserListAsync(string branchCode, string status);
    }
    public class AccountTranFccService : IAccountTranFccService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly IAccountTranFccRepository _accounTranRepo;

        public AccountTranFccService(IConfiguration configuration, IUnitOfWorkContext unitOfWork, IAccountTranFccRepository accounTranRepo)
        {
            this._config = configuration;
            this._unitOfWork = unitOfWork;
            this._accounTranRepo = accounTranRepo;
        }



        public async Task<List<AccountDetailCASAModel>> GetAccountDetailsAsync(string cif)
        {
            var rs = await this._accounTranRepo.GetAccountDetailsAsync(cif);
            var ouput = rs.Select(s => s.MapProp<AccountDetailCASA, AccountDetailCASAModel>())
                          //.Where(s => s.trangThai.ToUpper() != "FROZEN")
                          .ToList();
            return ouput;
        }

        public async Task<bool> CompletedLoanAsync(string accNo)
        {
            var rs = await this._accounTranRepo.CompletedLoan(accNo);
            return rs;
        }

        public async Task<List<UserListModel>> GetUserListAsync(string branchCode, string status)
        {
            var result = await _accounTranRepo.GetUserListAsync(branchCode, status);
            return result.Select(s => s.MapProp<UserList, UserListModel>()).ToList();
        }
    }
}
