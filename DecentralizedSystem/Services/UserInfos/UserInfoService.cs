using DecentralizedSystem.Core.Entities;
using DecentralizedSystem.Helpers;
using DecentralizedSystem.Infrastructure.Repository;
using DecentralizedSystem.Models.UserInfo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DecentralizedSystem.Services.UserInfos
{
    public interface IUserInfoService
    {
        Task<IReadOnlyList<UserInfoModel>> GetListAsync(List<string> rolesFcc, List<string> rolesId, string branchId, int? activeFlag);

        Task<UserInfoModel> GetUserInfoByName(string userName);
    }
    public class UserInfoService : IUserInfoService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly IUserInfoRepository _userInfoRepository;

        public UserInfoService(IConfiguration config, IUnitOfWorkContext unitOfWork, IUserInfoRepository userInfoRepository)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _userInfoRepository = userInfoRepository;
        }

        public async Task<IReadOnlyList<UserInfoModel>> GetListAsync(List<string> rolesFcc, List<string> rolesId, string branchId, int? activeFlag)
        {
            var data = new List<UserInfoModel>();

            foreach (var roleFcc in rolesId)
            {
                var query = await _userInfoRepository.GetListAsync(roleFcc, branchId, activeFlag);
                data.AddRange(query.Select(s => s.MapProp<UserInfo, UserInfoModel>()).ToList());
            }
            foreach (var roleFcc in rolesFcc)
            {
                var query = await _userInfoRepository.GetListByFCCAsync(roleFcc, branchId, activeFlag);
                data.AddRange(query.Select(s => s.MapProp<UserInfo, UserInfoModel>()).ToList());
            }
            return data;
        }

        public async Task<UserInfoModel> GetUserInfoByName(string userName)
        {
            var query = await _userInfoRepository.GetUserInfoByName(userName);

            var data = query.MapProp<UserInfo, UserInfoModel>();

            return data; 
        }
    }
}
