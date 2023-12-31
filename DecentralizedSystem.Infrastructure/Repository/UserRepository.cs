﻿using Microsoft.Extensions.Configuration;
using Dapper;
using DecentralizedSystem.Infrastructure.Interfaces;
using DecentralizedSystem.Core.Entities;

namespace DecentralizedSystem.Infrastructure.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        /// <summary>
        /// Lấy danh sách user theo phân trang
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        Task<PagedResults<User>> QueryAsync(int pageNum = 0, int pageSize = int.MaxValue, int isActive = 1);
    }
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        public UserRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }
        public async Task<int> AddAsync(User entity)
        {
            //var sql = @"INSERT INTO ORG_USER
            //              (id,
            //               user_name,
            //               fullname,
            //               email,
            //               department_id,
            //               branch_id,
            //               active_flag,
            //               created_at)
            //            VALUES
            //              (:v_id,
            //               :v_user_name,
            //               :v_fullname,
            //               :v_email,
            //               :v_department_id,
            //               :v_branch_id,
            //               1,
            //               sysdate)";
            //var result = await this._context.Connection.ExecuteAsync(sql, new
            //{
            //    v_id = entity.Id,
            //    v_user_name = entity.Name.ToLower(),
            //    v_fullname = entity.FullName,
            //    v_email = entity.Email,
            //    v_department_id = "0001",
            //    v_branch_id = entity.BranchID,
            //});
            //return result;
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(string id)
        {
            var sql = @"DELETE FROM auth_users
                        WHERE 
	                        id = @id";

            var result = await this._context.Connection.ExecuteAsync(sql, new { Id = id });
            return result;
        }

        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            var sql = @"SELECT u.* FROM auth_users u";
            var result = await this._context.Connection.QueryAsync<User>(sql);
            return result.ToList();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var sql = @"SELECT u.* FROM auth_users u where id = @user_id or name = @user_id";
            var result = await this._context.Connection.QuerySingleOrDefaultAsync<User>(sql, new { user_id = id });
            return result;
        }

        public async Task<PagedResults<User>> QueryAsync(int pageNum = 0, int pageSize = int.MaxValue, int isActive = 1)
        {
            var results = new PagedResults<User>();

            var sql = @"SELECT p.*
                        FROM auth_users p
                        WHERE p.is_active = @is_active
                        LIMIT @page_size OFFSET @page_num; 
                        
                        SELECT 
                        COUNT(*)
                        FROM auth_users p
                        WHERE p.is_active = @is_active";

            var queryMultiDatas = await this._context.Connection.QueryMultipleAsync(sql,
                new
                {
                    is_active = isActive,
                    page_size = pageSize,
                    page_num = pageNum * pageSize
                });
            results.Result = queryMultiDatas.Read<User>().ToList();
            results.TotalCount = queryMultiDatas.ReadFirst<int>();
            return results;
        }

        public async Task<int> UpdateAsync(User entity)
        {
            var sql = @"UPDATE auth_users
                        SET 
                           name = @name,
                           full_name = @full_name,
                           email = @email,
                           is_active = @is_active,
                           trial_ends_at = @trial_ends_at,
                           password = @password,
                           url_avatar = @url_avatar,
                           updated_at = SYSDATE()
                        WHERE 
                           id = @id";
            var result = await this._context.Connection.ExecuteAsync(sql, entity);
            return result;
        }
    }

    
}
