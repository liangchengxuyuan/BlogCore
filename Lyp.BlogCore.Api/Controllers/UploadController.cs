using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Log;
using Lyp.BlogCore.Api.AOP;
using Lyp.BlogCore.Common.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lyp.BlogCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ILoggerHelper loggerHelper;

        public UploadController(ILoggerHelper logger)
        {
            this.loggerHelper = logger;
        }
        #region 文件上传  可以带参数
        //[HttpPost]
        //public IActionResult Upload(IFormFile file)
        //{
        //    var a = "sas";

        //    if (file != null)
        //    {
        //        var fileDir = "/Home/lyp/Images";
        //        if (!Directory.Exists(fileDir))
        //        {
        //            Directory.CreateDirectory(fileDir);
        //        }
        //        //文件名称
        //        string projectFileName = file.FileName;

        //        //上传的文件的路径
        //        string filePath = fileDir + $@"\{projectFileName}";
        //        using (FileStream fs = System.IO.File.Create(filePath))
        //        {
        //            file.CopyTo(fs);
        //            fs.Flush();
        //        }
        //        return Ok("ok");


        //    }
        //    else
        //    {
        //        return Ok("no");
        //    }

        //}
        #endregion

        //[HttpPost]
        //public async Task<IActionResult> Post([FromFile]FileHelper file)
        //{
        //    if (file == null || !file.IsValid)
        //        return Ok(new { code = 2, message = "不允许上传的文件类型" });

        //    string newFile = string.Empty;
        //    if (file != null)
        //        newFile = await file.SaveAs("/var/www/VueApp/static/img");

        //    return Ok(new
        //    {
        //        success = true,
        //        code = 1,
        //        data = newFile
        //    });
        //}

        [HttpPost]
        //[FromServices]IHostingEnvironment environment
        public async Task<IActionResult> OnPostUpload(IFormFileCollection files)
        {
            try
            {
                string username = HttpContext.Session.GetString("UserName");

                if (string.IsNullOrEmpty(username))
                {
                    return Ok(new
                    {
                        success = true,
                        code = 0
                    });
                }
                files = Request.Form.Files;
                List<TmpUrl> list = new List<TmpUrl>();
                string destinationDir = "/var/www/VueApp/static/img";//  /var/www/VueApp/static/img

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(destinationDir))
                            Directory.CreateDirectory(destinationDir);

                        var fileName = Guid.NewGuid().ToString() + ".jpg";
                        var path = Path.Combine(destinationDir, fileName);
                        using (var stream = new FileStream(path, FileMode.CreateNew))
                        {
                            await formFile.CopyToAsync(stream);
                            TmpUrl tu = new TmpUrl();
                            tu.Url = @"/static/img/" + fileName; //  /static/img/
                            list.Add(tu);
                        }
                    }
                }

                return Ok(new
                {
                    success = true,
                    code = 1,
                    data = list
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("UploadController.OnPostUpload", "异常位置：UploadController.OnPostUpload" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    code = 0,
                    message = ex.Message
                });
            }

        }

        public class TmpUrl
        {
            public string Url { get; set; }
        }
    }
}