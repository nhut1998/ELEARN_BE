using DecentralizedSystem.API.Models.Account;
using DecentralizedSystem.Consts;
using DecentralizedSystem.Helpers;
using DecentralizedSystem.Models.Account.Request;
using DecentralizedSystem.Models.Account;
using DecentralizedSystem.Models.Globally;
using DecentralizedSystem.Services.Account;
using DecentralizedSystem.Services.SOA;
using DecentralizedSystem.Services.UserInfos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DecentralizedSystem.Controllers.Account
{
    /// <summary>
    /// Tài khoản
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IRoleService _roleService;
        private readonly ISOAService _SOAService;
        private readonly IBranchService _branchService;
        private readonly IUserService _userService;
        private readonly IUserInfoService _userInfoService;

        public AccountController(IConfiguration configuration, IRoleService roleService, ISOAService soaService, IBranchService branchService, IUserService userService, IUserInfoService userInfoService)
        {
            _config = configuration;
            _roleService = roleService;
            _SOAService = soaService;
            _branchService = branchService;
            _userService = userService;
            _userInfoService = userInfoService;
        }


       

        [HttpGet]
        [Route("{refreshToken}/refresh")]
        //[Authorize]
        public async Task<IActionResult> Refresh(string refreshToken)
        {
            try
            {
                return Unauthorized();
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
        /// Đăng ký
        /// </summary>
        /// <param name="user">Param truyền vào</param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel  user)
        {
            try
            {
               await _userService.RegisterUserAsync(user);

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



        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginLearn([FromBody] RequestLoginModel user)
        {
            try
           {

                var userName = user.UserName?.Trim().ToString();
                var usersInfo = await this._userService.GetUsersInfoAsync(userName);
                var users = await this._userService.GetByUserNameAsync(userName);
                var rs = users.Select(s => s.UserName).FirstOrDefault();

               

                if (rs != user.UserName)
                {
                    throw new ArgumentException("Tài khoản chưa được đăng ký");
                }

                bool verified = BCrypt.Net.BCrypt.Verify(user.Password, usersInfo.Password);

                if (!verified)
                {
                    throw new ArgumentException("Mật khẩu không đúng");
                }

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Authentication:JwtBearer:SecurityKey"]));
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, usersInfo.UserId),
                    new Claim(ClaimTypes.NameIdentifier, usersInfo.UserName),
                    new Claim(ClaimTypes.Name, usersInfo.FullName ?? ""),
                    new Claim(ClaimTypes.Email, usersInfo.Email)
                };

                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                        issuer: _config["Authentication:JwtBearer:Issuer"],
                        audience: _config["Authentication:JwtBearer:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(500),
                        signingCredentials: signinCredentials
                    );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                return Ok(new ErrorModel
                {
                    Code = "LOGIN_OK",
                    Message = "Successful!",
                    Data = new
                    {
                        token = EncryptionHelper.EncryptJWT(tokenString, _config),
                        user_info = new
                        {
                            id = usersInfo.UserId,
                            username = usersInfo.UserName,
                            fullname = usersInfo.FullName,
                            email = usersInfo.Email,
                            phonenumber = usersInfo.PhoneNumber
                        } 
                        
                       
                    }
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
