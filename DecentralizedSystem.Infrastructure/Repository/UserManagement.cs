using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using DecentralizedSystem.Core.Entities;


namespace DecentralizedSystem.Infrastructure.Repository
{
    public interface IUserManagementRepository
    {
        /// <summary>
        /// Tạo mới
        /// </summary>
        /// <param name="entity">data input</param>
        /// <returns></returns>
        Task<int> AddAsync(UserManagements entity);
    }
    public class UserManagementRepository: IUserManagementRepository

    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;

        public UserManagementRepository(IConfiguration configuration, IConnectionContext context )
        {
            this._configuration = configuration;
            this._context = context;
        }
        public async Task<int> AddAsync(UserManagements entity)
        {
            var sql = @"INSERT INTO org_user
                        (user_id, user_name, full_name, email, phone_number, password, role, create_at) 
                        VALUES 
                        (:user_id, :user_name, :full_name, :email, :phone_number, :password, :role, sysdate)";

            var result = await _context.Connection.ExecuteAsync(sql, new
            {
                user_id = Guid.NewGuid().ToString(),
                user_name = entity.UserName,
                full_name = entity.FullName,
                email = entity.Email,
                phone_number = entity.PhoneNumber,
                password = entity.Password,
                role = entity.Role.ToUpper(),
            });
            return result;

        }



    }
}
