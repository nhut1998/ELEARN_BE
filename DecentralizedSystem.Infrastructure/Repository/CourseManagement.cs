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
    public interface ICourseManagementRepository
    {
        /// <summary>
        /// Tạo mới
        /// </summary>
        /// <param name="entity">data input</param>
        /// <returns></returns>
        Task<int> AddAsync(Course entity);

        Task<IReadOnlyList<Catalog>> GetAllCatalog();

        Task<IReadOnlyList<Course>> GetAllCourseAsync();

        /// <summary>
        /// lấy danh sách khóa học
        /// </summary>
      
        /// <returns></returns>
        
        Task<IReadOnlyList<CourseList>> GetCourseListAsync();

        Task<int> TotalCountCourse(string? name, string? catalogId, string? makerId);

        public Task<IReadOnlyList<CourseList>> GetCourseListPagingAsync(string? name, string? catalogId, string? makerId, int? pageSize, int? pageNum);
    }

    public class CourseManagementRepository : ICourseManagementRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;

        public CourseManagementRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }

        public async Task<int> TotalCountCourse(string? name, string? catalogId, string? makerId)
        {
            var sql = @"SELECT  count(t.course_id) from ELEARN_COURSE t JOIN ORG_USER u ON t.maker_id = u.user_name 
                        JOIN AUTH_ROLE a ON u.role = a.role_name JOIN ELEARN_CATALOG c ON t.catalog_id = c.id
                         where (:name is null or t.name = :name) and (:catalog_id is null or t.catalog_id = :catalog_id) and 
                        (:maker_id is null or t.maker_id = :maker_id) 
                        ";

            var result = await _context.Connection.ExecuteScalarAsync<int>(sql, new
            {
                name = name,
                catalog_id = catalogId,
                maker_id = makerId,
            });
            return result;
        }
        public async Task<IReadOnlyList<CourseList>> GetCourseListPagingAsync(string? name, string? catalogId, string? makerId, int? pageSize, int? pageNum)
        {
            var sql = @"SELECT t.*, u.full_name,a.role_name as role , a.description as role_name, c.catalog_id as catalog_name,
                        c.catalog_name as catalog_description from ELEARN_COURSE t JOIN ORG_USER u ON t.maker_id = u.user_name JOIN AUTH_ROLE a
                        ON u.role = a.role_name JOIN ELEARN_CATALOG c ON t.catalog_id = c.id
                         where (:name is null or t.name = :name) and (:catalog_id is null or t.catalog_id = :catalog_id) and (:maker_id is null or t.maker_id = :maker_id) order by t.create_at desc  offset :pagesize*(:pagenum)  rows fetch next  :pagesize rows only 
                        ";

            var result = await _context.Connection.QueryAsync<CourseList>(sql, new
            {
                name = name,
                catalog_id = catalogId,
                maker_id = makerId,
                pagesize = pageSize,
                pagenum = pageNum
            });
            return result.ToList();
        }

        public async Task<IReadOnlyList<CourseList>> GetCourseListAsync()
        {
            var sql = @"SELECT t.*, u.full_name,a.role_name as role , a.description as role_name, c.catalog_id as catalog_name, c.catalog_name as catalog_description from ELEARN_COURSE t JOIN ORG_USER u ON t.maker_id = u.user_name JOIN AUTH_ROLE a ON u.role = a.role_name JOIN ELEARN_CATALOG c ON t.catalog_id = c.id";

            var result = await _context.Connection.QueryAsync<CourseList>(sql);
            return result.ToList();

        }
        public async Task<IReadOnlyList<Course>> GetAllCourseAsync()
        {
            var sql = @"SELECT * FROM elearn_course";
            var result = await _context.Connection.QueryAsync<Course>(sql);
            return result.ToList();

        }

         public async  Task<IReadOnlyList<Catalog>> GetAllCatalog()
        {
            var sql = @"SELECT * FROM elearn_catalog";
            var result = await _context.Connection.QueryAsync<Catalog>(sql);
            return result.ToList();
        }
        public async Task<int> AddAsync(Course entity)
        {
            var sql = @"INSERT INTO elearn_course 
                        (course_id, name, description, luotxem, evaluate, catalog_id, maker_id, aliases,tuition, status, luotxem, create_at) 
                        VALUES
                         (:course_id, :name, :description, :luotxem, :evaluate, :catalog_id, :maker_id, :aliases,:tuition, :status, :luotxem, sysdate)";



            var result = await this._context.Connection.ExecuteAsync(sql, new
            {
                course_id = Guid.NewGuid().ToString(),
                name = entity.Name,
                description = entity.Description,
                luotxem = 0,
                evaluate = entity.Evaluate,
                catalog_id = entity.CatalogId,
                maker_id = entity.MakerId,
                aliases = entity.Aliases,
                tuition = entity.Tuition,
               // img = entity.Img,
                status = entity.Status,
            });
            return result;

        }
    }
}