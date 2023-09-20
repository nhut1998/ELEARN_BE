using Microsoft.Extensions.Configuration;
using Dapper;
using DecentralizedSystem.Infrastructure.Interfaces;
using DecentralizedSystem.Core.Entities;

namespace DecentralizedSystem.Infrastructure.Repository
{
    public interface IUserRoleRepository
    {
        /// <summary>
        /// Đăng ký
        /// </summary>
        /// <param name="user">data input</param>
        /// <returns></returns>
        Task<int> RegisterAsync(Register user);


        /// <summary>
        /// Lấy thông tin user theo user_name
        /// </summary>
        /// <param name="userName">id data info</param>
        /// <returns></returns>
        Task<IReadOnlyList<Users>> GetByUserNameAsync(string userName);


        /// <summary>
        /// Lấy thông tin user theo user_name
        /// </summary>
        /// <param name="userName">id data info</param>
        /// <returns></returns>
        Task<Users> GetByUserNamePassAsync(string userName);


        /// <summary>
        /// Tạo mới
        /// </summary>
        /// <param name="entity">data input</param>
        /// <returns></returns>
        Task<int> AddAsync(UserRole entity);

        /// <summary>
        /// Xóa thông tin theo role id
        /// </summary>
        /// <param name="roleId">id data info</param>
        /// <returns></returns>
        Task<int> DeleteByRoleAsync(string roleId);

        /// <summary>
        /// Xóa thông tin theo user id
        /// </summary>
        /// <param name="roleId">id data info</param>
        /// <returns></returns>
        Task<int> DeleteByUserAsync(string userId);

        /// <summary>
        /// Xóa thông tin theo user id và role id
        /// </summary>
        /// <param name="userId">id data info</param>
        /// <param name="roleId">id data info</param>
        /// <returns></returns>
        Task<int> DeleteAsync(string userId, string roleId);
    }
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        public UserRoleRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }
        public async Task<int> AddAsync(UserRole entity)
        {
            var sql = @"INSERT INTO auth_user_has_roles 
                         (user_id, role_id)
                        VALUES 
                         (@user_id, @role_id)";
            var result = await this._context.Connection.ExecuteAsync(sql, entity);
            return result;
        }

        public async Task<int> DeleteAsync(string userId, string roleId)
        {
            var sql = @"DELETE FROM auth_user_has_roles
                        WHERE 
	                        role_id = @role_id and user_id = @user_id";

            var result = await this._context.Connection.ExecuteAsync(sql, new { role_id = roleId, user_id = userId });
            return result;
        }

        public async Task<int> DeleteByRoleAsync(string roleId)
        {
            var sql = @"DELETE FROM auth_user_has_roles
                        WHERE 
	                        role_id = @role_id";

            var result = await this._context.Connection.ExecuteAsync(sql, new { role_id = roleId });
            return result;
        }

        public async Task<int> DeleteByUserAsync(string userId)
        {
            var sql = @"DELETE FROM auth_user_has_roles
                        WHERE 
	                        user_id = @user_id";

            var result = await this._context.Connection.ExecuteAsync(sql, new { user_id = userId });
            return result;
        }

        //----------------------------------------------------------------//



        public async Task<IReadOnlyList<Users>> GetByUserNameAsync(string userName)
        {
            var sql = @"SELECT * FROM org_user  where user_name = :user_name";
            var result = (await _context.Connection.QueryAsync<Users>(sql, new { user_name = userName })).ToList();
            return result;
        }

        public async Task<Users> GetByUserNamePassAsync(string userName)
        {
            var sql = @"SELECT * FROM org_user  where user_name = :user_name";
            var result = await this._context.Connection.QueryAsync<Users>(sql, new { user_name = userName });
            return result.FirstOrDefault();
        }



        public async Task<int> RegisterAsync(Register user)
        {
            var sql = @"INSERT INTO  org_user
                         (user_id, user_name, full_name, email, phone_number, password, create_at)
                        VALUES 
                         (:user_id, :user_name, :full_name, :email, :phone_number, :password,sysdate)";
            var result = await _context.Connection.ExecuteAsync(sql,  new
            {
                user_id = Guid.NewGuid().ToString(),
                user_name = user.UserName,
                full_name = user.FullName,
                email = user.Email,
                phone_number = user.PhoneNumber,
                password = user.Password,
            });
            return result;
        }

    }
}
