using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecentralizedSystem.Core.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;


namespace DecentralizedSystem.Infrastructure.Repository
{

    public interface ILikeRepository
    {
        public Task<int> LikeAsync(string CourseId);

        public Task<int> AddLikeAsync(Like entity);

        public Task<int> DisLikeAsync(string CourseId);

        public Task<int> DeleteLikeAsync(string CourseId, string userId);

        public Task<Like> GetLikeAsync(string CourseId, string userId);

    }
    public class CourseLikeRepository: ILikeRepository
    {
        private readonly  IConnectionContext _context;
        private readonly IConfiguration _configuration;

        public CourseLikeRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }

        public async Task<int> DisLikeAsync(string CourseId)
        {
            var sql = @"UPDATE elearn_course SET luotxem = luotxem - 1 WHERE course_id = :course_id";

            var result = await this._context.Connection.ExecuteAsync(sql, new
            {
                course_id = CourseId,
            });

            return result;

        }
        public async Task<int> DeleteLikeAsync(string CourseId, string userId)
        {
            var sql = @"DELETE FROM ELEARN_LIKE WHERE course_id = :course_id and user_id = :user_id";
            var result = await this._context.Connection.ExecuteAsync(sql, new
            {
                course_id = CourseId,
                user_id = userId,
            });
            return result;

        }

        public async Task<Like> GetLikeAsync(string CourseId, string userId)
        {
            var sql = @"SELECT * FROM ELEARN_LIKE WHERE course_id = :course_id and user_id = :user_id";
            var result = await this._context.Connection.QueryAsync<Like>(sql, new
            {
                course_id = CourseId,
                user_id = userId,
            });
            return result.FirstOrDefault();
        }
        public async Task<int> LikeAsync(string CourseId)
    
        {
            var sql = @"UPDATE elearn_course SET luotxem = luotxem + 1 WHERE course_id = :course_id";

            var result = await this._context.Connection.ExecuteAsync(sql, new
            {
                course_id = CourseId,
            });

            return result;
        }

        public async Task<int> AddLikeAsync(Like entity)
        {
            var sql = @"INSERT INTO ELEARN_LIKE (course_id, user_id, like_at) VALUES (:course_id, :user_id, sysdate)";

            var result = await this._context.Connection.ExecuteAsync(sql, new
            {
                course_id = entity.CourseID,
                user_id = entity.UserID,
            });

            return result;
        }

    }
}
