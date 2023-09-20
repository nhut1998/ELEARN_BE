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
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using DecentralizedSystem.Models.CourseManagemet;



namespace DecentralizedSystem.Services.Account
{
    public interface IUserService
    {
        /// <summary>
        /// Tạo mới thông tin nhân sự
        /// </summary>
        /// <param name="user">data input</param>
        /// <returns></returns>
        Task<string> RegisterUserAsync(RegisterModel user);

        /// <summary>
        /// Lấy thông tin chi tiết nhân sự theo id
        /// </summary>
        /// <param name="userName">id của nhân sự</param>
        /// <returns></returns>
        Task<IReadOnlyList<UsersModel>> GetByUserNameAsync(string userName);


        Task<UsersModel> GetUsersInfoAsync(string userName);

        /// <summary>
        /// Lấy danh sách thông tin nhân sự
        /// </summary>
        /// <returns></returns>
        Task<List<UserModel>> QueryAsync();

        /// <summary>
        /// Lấy thông tin chi tiết nhân sự theo id
        /// </summary>
        /// <param name="id">id của nhân sự</param>
        /// <returns></returns>
        Task<UserModel> QueryDetailAsync(string id);

        /// <summary>
        /// Tạo mới thông tin nhân sự
        /// </summary>
        /// <param name="user">data input</param>
        /// <returns></returns>
        Task<string> CreateAsync(UserModel user);

        /// <summary>
        /// Cập nhật thông tin nhân sự
        /// </summary>
        /// <param name="user">data input</param>
        /// <returns></returns>
        Task UpdateAsync(UserModel user);

        /// <summary>
        /// Cập nhật Role cho nhân sự
        /// </summary>
        /// <param name="userId">id của nhân sự</param>
        /// <param name="roles">data input</param>
        /// <returns></returns>
        Task UpdateUserHasRolesAsync(string userId, List<RoleModel> roles);

        /// <summary>
        /// Lấy danh sách Role của nhân sự
        /// </summary>
        /// <param name="userId">id của nhân sự</param>
        /// <returns></returns>
        Task<List<RoleModel>> GetUserHasRolesAsync(string userId);

        /// <summary>
        /// Tạo thông tin role và user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task CreateRoleForUser(string userId, string roleId);

        /// <summary>
        /// Xóa thông tin role và user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task DeleteRoleForUser(string userId, string roleId);
    }

    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IUserRoleRepository _userRoleRepo;
        private readonly ICourseManagementRepository _courseRepository;

        public UserService(IConfiguration configuration, IUnitOfWorkContext unitOfWork, IUserRepository userRepository, IRoleRepository roleRepo, IUserRoleRepository userRoleRepo)
        {
            this._config = configuration;
            this._unitOfWork = unitOfWork;
            this._userRepo = userRepository;
            this._roleRepo = roleRepo;
            this._userRoleRepo = userRoleRepo;
        }
        public async Task<List<UserModel>> QueryAsync()
        {
            var users = await this._userRepo.GetAllAsync();

            var result = users.Select(async s =>
            {
                var data = s.MapProp<User, UserModel>();
                var roles = await this._roleRepo.GetRoleByUserAsync(s.Id);
                data.Roles = roles.Select(sp => sp.MapProp<Role, RoleModel>()).ToList();
                return data;
            });

            return result.Select(s => s.Result).ToList();
        }
        public async Task<UserModel> QueryDetailAsync(string id)
        {
            var user = await this._userRepo.GetByIdAsync(id);
            var result = user.MapProp<User, UserModel>();
            result.Roles = await GetUserHasRolesAsync(result.Id);

            return result;
        }
        public async Task<string> CreateAsync(UserModel user)
        {
            using (var uow = _unitOfWork.Create())
            {
                try
                {
                    await this._userRepo.AddAsync(user.MapProp<UserModel, User>());
                    await uow.CommitAsync();
                    return user.Id;
                }
                catch (Exception)
                {
                    await uow.RollBackAsync();
                    throw;
                }
            }
        }
        public async Task UpdateAsync(UserModel user)
        {
            await this._userRepo.UpdateAsync(user.MapProp<UserModel, User>());
        }
        public async Task UpdateUserHasRolesAsync(string userId, List<RoleModel> roles)
        {

            using (var uow = _unitOfWork.Create())
            {
                try
                {

                    await this._userRoleRepo.DeleteByUserAsync(userId);
                    foreach (var item in roles)
                    {
                        await this._userRoleRepo.AddAsync(new UserRole
                        {
                            RoleId = item.Id,
                            UserId = userId,
                        });
                    }
                    await uow.CommitAsync();
                }
                catch (Exception)
                {
                    await uow.RollBackAsync();
                    throw;
                }
            }
        }

        public async Task<List<RoleModel>> GetUserHasRolesAsync(string userId)
        {
            var roles = await this._roleRepo.GetRoleByUserAsync(userId);
            var result = roles.Select(s => s.MapProp<Role, RoleModel>());
            return result.ToList();
        }

        public async Task CreateRoleForUser(string userId, string roleId)
        {
            await this._userRoleRepo.AddAsync(new UserRole
            {
                RoleId = roleId,
                UserId = userId,
            });
        }

        public async Task DeleteRoleForUser(string userId, string roleId)
        {
            await this._userRoleRepo.DeleteAsync(userId, roleId);
        }


        //--------------------------------------------------------------------------------------------//

        public async Task<UsersModel> GetUsersInfoAsync(string userName)
        {
            var result = await this._userRoleRepo.GetByUserNamePassAsync(userName);
            var data = result.MapProp<Users, UsersModel>();
            return data;
        }
       public async Task<IReadOnlyList<UsersModel>> GetByUserNameAsync(string userName)
        {
            var rs = (await this._userRoleRepo.GetByUserNameAsync(userName));
            var result = rs.Select(s => s.MapProp<Users, UsersModel>());
            return result.ToList();
        }


        public async Task<string> RegisterUserAsync(RegisterModel user)
        {



            //const int keySize = 36;
            //const int iterations = 350000;
            //HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            //string HashPasword(string password, out byte[] salt)
            //{
            //   salt = RandomNumberGenerator.GetBytes(keySize);
            //    var hash = Rfc2898DeriveBytes.Pbkdf2(
            //        Encoding.UTF8.GetBytes(password),
            //        salt,
            //        iterations,
            //        hashAlgorithm,
            //        keySize);
            //    return Convert.ToHexString(hash);
            //}

           // var hashPass = HashPasword(user.Password, out var salt);


            //bool VerifyPassword(string password, string hash, byte[] salt)
            //{
            //    var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
            //    return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
            //}

            //var isVerify = VerifyPassword("nhut1234", hashPass, salt);

            using (var uow = _unitOfWork.Create())
            {
                try
                {
                    //var usersInfo = await GetUsersInfoAsync(user.UserName);
                    //var id = usersInfo.UserId;
               
                    var users = await GetByUserNameAsync(user.UserName);
                    var rs = users.Select(s => s.UserName).FirstOrDefault();
                    if (rs == user.UserName)
                    {
                        throw new ArgumentException("User đã tồn tại");
                    }

                     
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    await this._userRoleRepo.RegisterAsync(user.MapProp<RegisterModel, Register>());
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
