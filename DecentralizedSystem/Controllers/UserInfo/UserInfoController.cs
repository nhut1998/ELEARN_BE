using DecentralizedSystem.Helpers;
using DecentralizedSystem.Models.Globally;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using DecentralizedSystem.Services.UserInfos;
using System.Collections.Generic;

namespace DecentralizedSystem.Controllers.UserInfo
{
    /// <summary>
    /// Thông tin User
    /// </summary>
    [Route("api/v{version:apiVersion}/user")]
    [ApiVersion("1.0")]
    [ApiController()]
    [Authorize]

    public class UserInfoController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserInfoService _userInfoService;

        public UserInfoController(IConfiguration config, IUserInfoService userInfoService)
        {
            _config = config;
            _userInfoService = userInfoService;
        }

        /// <summary>
        /// Lấy thông tin User
        /// </summary>
        /// <param name="rolesFcc">Roles</param>
        /// <param name="branchId">Mã đơn vị</param>
        /// <param name="activeFlag"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetListAsync([FromQuery(Name = "roles_fcc")] List<string> rolesFcc,
                                                      [FromQuery(Name = "roles_id")] List<string> rolesId,
                                                      [FromQuery(Name = "branch_id")] string branchId,
                                                      [FromQuery(Name = "active_flag")] int? activeFlag)
        {
            try
            {
                var session = User.GetSession();
                var result = await _userInfoService.GetListAsync(rolesFcc, rolesId, branchId, activeFlag);

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
    }
}
