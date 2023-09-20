
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DecentralizedSystem.Helpers;
using DecentralizedSystem.Models.Globally;
using DecentralizedSystem.Models.Account;
using DecentralizedSystem.Services.UserManagement;
using DecentralizedSystem.Services.Account;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;

namespace DecentralizedSystem.Controllers.UserManagement
{
    [Route("api/v{version:apiVersion}/usermanagement")]
    [ApiVersion("1.0")]
    [ApiController()]
    [Authorize]
    public class UserManagementController : ControllerBase
    {
       private readonly IUserManagementService _userManagementService;


        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        /// <summary>
        /// Thêm user
        /// </summary>
        /// <param name="user">Param truyền vào</param>
        /// <returns></returns>
        [HttpPost]
        [Route("adduser")]
        public async Task<IActionResult> AddUser([FromBody] UserManagementModel user)
        {
            try
            {
                await this._userManagementService.AddAsync(user);
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
