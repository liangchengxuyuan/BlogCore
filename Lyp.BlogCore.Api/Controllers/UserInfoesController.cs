using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lyp.BlogCore.Models.Models;
using Lyp.BlogCore.Repository.MySqlEFCore;
using Lyp.BlogCore.IServices;
using Lyp.BlogCore.Common.Helper;
using Blog.Core.Log;

namespace Lyp.BlogCore.Api.Controllers
{
    [Route("api/[controller]")]
    //[Route("api/controller/action")]
    [ApiController]
    public class UserInfoesController : ControllerBase
    {
        private readonly IUserInfoService userInfoServices;
        private readonly ILoggerHelper loggerHelper;
        private readonly string encryptionKey = ConfigHelper.GetSectionValue("encryptionKey");//加密密匙
        private readonly string encryptionIv = ConfigHelper.GetSectionValue("encryptionIv");//加密向量
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="userInfo"></param>
        public UserInfoesController(IUserInfoService userInfo, ILoggerHelper logger)
        {
            this.userInfoServices = userInfo;
            this.loggerHelper = logger;
        }
        //判断是否登录
        [HttpGet]
        public IActionResult IsLoginUserInfoes()
        {
            try
            {
                string UserName = HttpContext.Session.GetString("UserName");

                int codes = 0;
                if (!string.IsNullOrEmpty(UserName))
                {
                    codes = 1;
                }
                return Ok(new
                {
                    success = true,
                    code = codes,
                    username = UserName
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("UserInfoesController.IsLoginUserInfoes", "异常位置：UserInfoesController.IsLoginUserInfoes" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [Route("ReSession")]
        [HttpGet]
        public IActionResult ReSessionUserInfoes()
        {
            try
            {
                HttpContext.Session.Remove("UserName");

                string UserName = HttpContext.Session.GetString("UserName");
                if (!string.IsNullOrEmpty(UserName))
                {
                    return Ok(new
                    {
                        success = false,
                        code = 0
                    });
                }

                return Ok(new
                {
                    success = true,
                    code = 1
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("UserInfoesController.IsLoginUserInfoes", "异常位置：UserInfoesController.IsLoginUserInfoes" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        // GET: api/UserInfoes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserInfo(int id)
        {
            try
            {
                var userInfo = await userInfoServices.Query(u => u.uID == id);

                if (userInfo == null)
                {
                    return NotFound();
                }

                return Ok(new
                {
                    success = true,
                    data = userInfo
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("UserInfoesController.GetUserInfo", "异常位置：UserInfoesController.GetUserInfo" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // PUT: api/UserInfoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserInfo([FromRoute] int id, [FromBody] UserInfo userInfo)
        {
            try
            {
                userInfo.uPassWord = CryptographyHelper.DESEncrypt(userInfo.uPassWord, encryptionKey, encryptionIv);//加密
                if (id != userInfo.uID)
                {
                    return BadRequest();
                }

                bool flag = await userInfoServices.Update(userInfo);

                return Ok(new
                {
                    success = flag
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("UserInfoesController.PutUserInfo", "异常位置：UserInfoesController.PutUserInfo" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // POST: api/UserInfoes注册
        [HttpPost]
        public async Task<IActionResult> PostUserInfo([FromBody] UserInfo userInfo)
        {
            try
            {
                userInfo.uPassWord = CryptographyHelper.DESEncrypt(userInfo.uPassWord, encryptionKey, encryptionIv);//加密
                userInfo.uIsAdministrators = false;
                userInfo.uCreateTime = DateTime.Now;
                bool flag = await userInfoServices.Add(userInfo);

                return Ok(new
                {
                    success = flag
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("UserInfoesController.PostUserInfo", "异常位置：UserInfoesController.PostUserInfo" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        //登录
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]UserInfo userInfo)
        {
            try
            {
                int flag = 0;
                if (!string.IsNullOrEmpty(userInfo.uUserName) || !string.IsNullOrEmpty(userInfo.uPassWord))
                {
                    userInfo.uPassWord = CryptographyHelper.DESEncrypt(userInfo.uPassWord, encryptionKey, encryptionIv);//加密
                    var userinfo = await userInfoServices.Query(u => u.uUserName == userInfo.uUserName && u.uPassWord == userInfo.uPassWord);

                    if (userinfo.Count > 0)
                    {
                        HttpContext.Session.SetString("UserName", userInfo.uUserName);
                        flag = 1;
                    }

                    return Ok(new
                    {
                        success = true,
                        code = flag,
                        data = userinfo
                    });
                }
                return Ok(new
                {
                    success = false,
                    code = flag,
                    data = ""
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("UserInfoesController.Login", "异常位置：UserInfoesController.Login" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // DELETE: api/UserInfoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserInfo([FromRoute] int id)
        {
            try
            {
                bool flag = await userInfoServices.DeleteById(id);

                return Ok(new
                {
                    success = flag
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("UserInfoesController.DeleteUserInfo", "异常位置：UserInfoesController.DeleteUserInfo" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}