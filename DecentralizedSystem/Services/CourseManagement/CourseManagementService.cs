using Dapper;
using DecentralizedSystem.Core.Entities;
using DecentralizedSystem.Helpers;
using DecentralizedSystem.Infrastructure.Repository;
using DecentralizedSystem.Models.CourseManagemet;
using DecentralizedSystem.Models.CourseManagemet.Request;
using DecentralizedSystem.Persistence;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DecentralizedSystem.Models.Globally;

namespace DecentralizedSystem.Services.CourseManagement
{

    public interface ICourseManagementService
    {
        /// <summary>
        /// Tạo mới khóa học
        /// </summary>
        /// <param name="course">data input</param>
        /// <returns></returns>
        Task<string> AddCourseAsync(RequestCourseModel course);

        /// <summary>
        /// Lấy danh sách danh mục khóa học
        /// </summary>
        /// <returns></returns>
        Task<List<CatalogModel>> CatalogQueryAsync();

        /// <summary>
        /// Lấy tất cả  khóa học
        /// </summary>
        ///<returns></returns>
        Task<List<CourseAllListModel>> CourseListAllQueryAsync();
        /// <summary>
        /// Lấy tất cả  khóa học phân trang
        /// </summary>
        ///<returns></returns>
        Task<PagedResultModel<CourseAllListModel>> GetCourseListPagingAsync(string name, string catalogId, string makerId, int? pageSize, int? pageNum);
    }
    public class CourseManagementService:ICourseManagementService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly ICourseManagementRepository _courseRepository;

        public CourseManagementService(IConfiguration configuration, IUnitOfWorkContext unitOfWork, ICourseManagementRepository courseRepository)
        {
            _config = configuration;
            _unitOfWork = unitOfWork;
            _courseRepository = courseRepository;
        }

        public async Task<PagedResultModel<CourseAllListModel>> GetCourseListPagingAsync(
            string name, string catalogId, string makerId, int? pageSize, int? pageNum
            )
        {
            var course = await this._courseRepository.GetCourseListPagingAsync(name, catalogId, makerId, pageSize, pageNum);
            var total = await this._courseRepository.TotalCountCourse(name, catalogId, makerId);
            var result = new PagedResultModel<CourseAllListModel>();
            var list = new List<CourseAllListModel>();
            foreach (var item in course)
            {
                var s = new CourseAllListModel();

                s.CourseId = item.CourseId;
                s.Name = item.Name;
                s.Description = item.Description;
                s.Luotxem = item.Luotxem;
                s.Evaluate = item.Evaluate;
                s.MakerId = item.MakerId;
                s.CatalogId = item.CatalogId;
                s.CreateAt = item.CreateAt;
                s.Aliases = item.Aliases;
                s.Status = item.Status;
                s.Tuition = item.Tuition;
                s.MakerCourse = new MakerModel
                {
                    Account = item.MakerId,
                    FullName = item.FullName,
                    Role = item.Role,
                    RoleName = item.RoleName,
                };
                s.CatalogList = new CatalogListModel
                {
                    CatalogDescription = item.CatalogDescription,
                    CatalogName = item.CatalogName,

                };
                list.Add(s);
            }
            result.Result = list;
            result.TotalCount = total;
            return result;

        }
        public async Task<List<CourseAllListModel>> CourseListAllQueryAsync()
        {
            var result = new List<CourseAllListModel>();
            var course = await this._courseRepository.GetCourseListAsync();

            foreach(var item in course)
            {
                var s = new CourseAllListModel();

                s.CourseId = item.CourseId;
                s.Name = item.Name;
                s.Description = item.Description;
                s.Luotxem = item.Luotxem;
                s.Evaluate = item.Evaluate;
                s.MakerId = item.MakerId;
                s.CatalogId = item.CatalogId;
                s.CreateAt = item.CreateAt;
                s.Aliases = item.Aliases;
                s.Status = item.Status;
                s.Tuition = item.Tuition;
                s.MakerCourse = new MakerModel
                {
                    Account = item.MakerId,
                    FullName = item.FullName,
                    Role = item.Role,
                    RoleName = item.RoleName,
                };
                s.CatalogList = new CatalogListModel
                {
                    CatalogDescription = item.CatalogDescription,
                    CatalogName = item.CatalogName,

                };
                result.Add(s);
                           
            }    
            return result;
            
        }

        public async Task<List<CatalogModel>> CatalogQueryAsync()
        {
            var result = await this._courseRepository.GetAllCatalog();
            var catalog = result.Select(s =>
           {
               var data = s.MapProp<Catalog, CatalogModel>();
               return data;
           });


            return catalog.ToList();
        }
        public async Task<string> AddCourseAsync(RequestCourseModel course)
        {
            using (var uow = _unitOfWork.Create())
            {
                try
                {
                    await this._courseRepository.AddAsync(course.MapProp<RequestCourseModel, Course>());
                    await uow.CommitAsync();
                    return course.CatalogId;
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
