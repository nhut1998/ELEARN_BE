using DecentralizedSystem.Helpers;
using DecentralizedSystem.Middlewares;
using DecentralizedSystem.Models.Globally;
using DecentralizedSystem.Models.IdmProject;
using DecentralizedSystem.Models.IdmProject.Request;
using DecentralizedSystem.Models.MasterPackage.Request;
using DecentralizedSystem.Services.IdmProjects;
using DecentralizedSystem.Services.MasterPackages;
using DecentralizedSystem.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace DecentralizedSystem.Controllers.IdmProject
{
    /// <summary>
    /// Chương trình
    /// </summary>
    [Route("api/v{version:apiVersion}/idm-project")]
    [ApiVersion("1.0")]
    [ApiController()]
    [Authorize]
    public class IdmProjectController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IIdmProjectService _idmProjectService;
        private readonly IMasterPackageServices _masterPackageServices;

        public IdmProjectController(IConfiguration config, IIdmProjectService idmProjectService, IMasterPackageServices masterPackageServices)
        {
            _config = config;
            _idmProjectService = idmProjectService;
            _masterPackageServices = masterPackageServices;
        }


        /// <summary>
        /// Tra cứu Chương trình
        /// </summary>
        /// <param name="model">Param truyền vào</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetListPagingAsync([FromQuery] GetQueryIdmProjectRequestModel model)
        {
            try
            {
                var masterPackage = new MasterPackageRequestModel()
                {
                    PTransactionId = Guid.NewGuid().ToString("N").ToUpper(),
                    PFunctionId = AppConsts.IdmProjectGetList,
                    PInputDetails = model.SerializeObject()
                };
                var result = await _idmProjectService.GetListPagingAsync(masterPackage);

                return Ok(result);
            }
            catch (CustomException ex)
            {
                return BadRequest(new ErrorModel
                {
                    Code = ex.ErrorCode,
                    Message = $"{ex.Message}"
                });
            }
        }

        /// <summary>
        /// Thêm mới Chương trình
        /// </summary>
        /// <param name="model">Param truyền vào</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddAsync([FromBody] AddIdmProjectRequestModel model)
        {
            try
            {
                var masterPackage = new MasterPackageRequestModel()
                {
                    PTransactionId = Guid.NewGuid().ToString("N").ToUpper(),
                    PFunctionId = AppConsts.IdmProjectCreate,
                    PInputDetails = model.SerializeObject()
                };
                var result = await _masterPackageServices.AddAsync(masterPackage);

                return Ok(result);
            }
            catch (CustomException ex)
            {
                return BadRequest(new ErrorModel
                {
                    Code = "ERROR",
                    Message = $"{ex.Message}"
                });
            }
        }

        /// <summary>
        /// Cập nhật Chương trình
        /// </summary>
        /// <param name="model">Param truyền vào</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute(Name = "id")] string id, [FromBody] UpdateIdmProjectRequestModel model)
        {
            try
            {
                var masterPackage = new MasterPackageRequestModel()
                {
                    PTransactionId = Guid.NewGuid().ToString("N").ToUpper(),
                    PFunctionId = AppConsts.IdmProjectUpdate,
                    PInputDetails = model.SerializeObject()
                };
                var result = await _masterPackageServices.UpdateAsync(masterPackage);

                return Ok(result);
            }
            catch (CustomException ex)
            {
                return BadRequest(new ErrorModel
                {
                    Code = "ERROR",
                    Message = $"{ex.Message}"
                });
            }
        }

        /// <summary>
        /// Xóa Chương trình
        /// </summary>
        /// <param name="model">Param truyền vào</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] string id)
        {
            try
            {
                var masterPackage = new MasterPackageRequestModel()
                {
                    PTransactionId = Guid.NewGuid().ToString("N").ToUpper(),
                    PFunctionId = AppConsts.IdmProjectUpdate,
                    PInputDetails = new { project_id = id }.SerializeObject()
                };
                var result = await _masterPackageServices.DeleteAsync(masterPackage);

                return Ok(result);
            }
            catch (CustomException ex)
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
