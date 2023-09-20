using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DecentralizedSystem.Helpers;
using DecentralizedSystem.Models.Globally;
using DecentralizedSystem.Models.CourseManagemet;
using DecentralizedSystem.Models.CourseManagemet.Request;
using DecentralizedSystem.Services.CourseManagement;
using DecentralizedSystem.Services.Account;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;


namespace DecentralizedSystem.Controllers.CourseManagement
{
    [Route("api/v{version:apiVersion}/usermanagement")]
    [ApiVersion("1.0")]
    [ApiController()]
   // [Authorize] 
    public class CourseManagementController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICourseManagementService _courseService;
        private readonly ICourseLikeService _courseLikeService;

        public CourseManagementController(ICourseManagementService courseService, ICourseLikeService courseLikeService)
        {
            _courseService = courseService;
            _courseLikeService = courseLikeService;
        }

        /// <summary>
        /// Lấy danh sách khóa học
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("courseall")]

        public async Task<IActionResult> GetListAllAsync()
        {
            try
            {
                var result = await this._courseService.CourseListAllQueryAsync();
                return Ok(new ErrorModel
                {
                    Code = "OK",
                    Message = "Successful!",
                    Data = result
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel
                {
                    Code = "ERROR",
                    Message = $"{ex.Message}"
                });

            }
        }

        /// <summary>
        /// Lấy danh sách khóa học phân trang
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("coursePaging")]

        public async Task<IActionResult> GetListAllPagingAsync([FromQuery] GetQueryCourseModel data)
        {
            try
            {
                var result = await this._courseService.GetCourseListPagingAsync(data.Name, data.CatalogId, data.MakerId, data.PageNum, data.PageSize);
                return Ok(new ErrorModel
                {
                    Code = "OK",
                    Message = "Successful!",
                    Data = result
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel
                {
                    Code = "ERROR",
                    Message = $"{ex.Message}"
                });

            }
        }

        /// <summary>
        /// Lấy thông tin danh mục khóa học
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("catalog")]
        public async Task<IActionResult> GetCatalog()
        {
            try
            {
               var result = await this._courseService.CatalogQueryAsync();
                return Ok(new ErrorModel
                {
                    Code = "OK",
                    Message = "Successful!",
                    Data = result
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel
                {
                    Code = "ERROR",
                    Message = $"{ex.Message}"
                });

            }
        }
        /// <summary>
        /// Like khóa học
        /// </summary>
        /// <param name="like">Param truyền vào</param>
        /// <returns></returns>
        [HttpPost]
        [Route("like")]
        public async Task<IActionResult> LikeCourse([FromBody] LikeModel like)
        {
            try
            {
                await this._courseLikeService.CourseLike(like);
                return Ok(new ErrorModel
                {
                    Code = "OK",
                    Message = "Successful!",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel
                {
                    Code = "ERROR",
                    Message = $"{ex.Message}"
                });

            }
        }

        /// <summary>
        /// Hủy Like khóa học
        /// </summary>
        /// <param name="like">Param truyền vào</param>
        /// <returns></returns>
        [HttpPost]
        [Route("dislike")]
        public async Task<IActionResult> DisLikeCourse([FromBody] LikeModel like)
        {
            try
            {
                await this._courseLikeService.DisCourseLike(like);
                return Ok(new ErrorModel
                {
                    Code = "OK",
                    Message = "Successful!",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel
                {
                    Code = "ERROR",
                    Message = $"{ex.Message}"
                });

            }
        }

        /// <summary>
        /// Thêm khóa học
        /// </summary>
        /// <param name="course">Param truyền vào</param>
        /// <returns></returns>
        [HttpPost]
        [Route("course")]

        public async Task<IActionResult> AddCourse([FromBody] RequestCourseModel course)
        {
            try
            {
                await this._courseService.AddCourseAsync(course);

                return Ok(new ErrorModel
                {
                    Code = "OK",
                    Message = "Successful!",
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel
                {
                    Code = "ERROR",
                    Message = $"{ex.Message}"
                });

            }
        }
    }
   
  
}
