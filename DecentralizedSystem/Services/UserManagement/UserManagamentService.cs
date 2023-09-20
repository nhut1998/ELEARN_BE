using Dapper;
using DecentralizedSystem.Core.Entities;
using DecentralizedSystem.Helpers;
using DecentralizedSystem.Infrastructure.Repository;
using DecentralizedSystem.Models.Account;
using DecentralizedSystem.Persistence;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DecentralizedSystem.Services.UserManagement
{
    public interface IUserManagementService
    {
        /// <summary>
        /// Tạo mới thông tin người dùng
        /// </summary>
        /// <param name="user">data input</param>
        /// <returns></returns>
        Task<string> AddAsync(UserManagementModel user);
    }
    public class UserManagementService: IUserManagementService
    {
        private readonly IConfiguration _config;
        private readonly IUserManagementRepository _userManagement;
        private readonly IUnitOfWorkContext _unitOfWork;

        public UserManagementService(IConfiguration configuration, IUnitOfWorkContext unitOfWork, IUserManagementRepository userManagementRepository)
        {
            this._config = configuration;
            this._unitOfWork = unitOfWork;
            this._userManagement = userManagementRepository;
        }
        public async Task<string> AddAsync(UserManagementModel user)
        {
            using (var uow = _unitOfWork.Create())
            {
                try
                {
                    await this._userManagement.AddAsync(user.MapProp<UserManagementModel, UserManagements>());
                    await uow.CommitAsync();
                    return user.UserName;

                }
                catch (Exception)
                {
                    await uow.RollBackAsync();
                    throw;
                }


            }    
        }

    }
}
