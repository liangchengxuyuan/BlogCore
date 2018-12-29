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
using Blog.Core.Log;

namespace Lyp.BlogCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService commentService;
        private readonly ILoggerHelper loggerHelper;

        public CommentsController(ICommentService service, ILoggerHelper logger)
        {
            this.commentService = service;
            this.loggerHelper = logger;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<IActionResult> Getcomments()
        {
            try
            {
                var commentList = await commentService.Query();
                return Ok(new
                {
                    success = true,
                    data = commentList
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("CommentsController.Getcomments", "异常位置：CommentsController.Getcomments" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment([FromRoute] int id)
        {
            try
            {
                var comment = await commentService.QueryById(s => s.bID == id);

                if (comment == null)
                {
                    return NotFound();
                }

                return Ok(new
                {
                    success = true,
                    data = comment
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("CommentsController.GetComment", "异常位置：CommentsController.GetComment" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // PUT: api/Comments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment([FromRoute] int id, [FromBody] Comment comment)
        {
            try
            {
                if (id != comment.cmID)
                {
                    return BadRequest();
                }

                bool flag = await commentService.Update(comment);

                return Ok(new
                {
                    success = flag
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("CommentsController.PutComment", "异常位置：CommentsController.PutComment" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // POST: api/Comments
        [HttpPost]
        public async Task<IActionResult> PostComment([FromBody] Comment comment)
        {
            try
            {
                comment.cmCommentator = HttpContext.Session.GetString("UserName");
                comment.cmCreateTime = DateTime.Now;
                bool flag = await commentService.Add(comment);


                return Ok(new
                {
                    success = flag
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("CommentsController.PostComment", "异常位置：CommentsController.PostComment" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            try
            {
                bool flag = await commentService.DeleteById(id);

                return Ok(new
                {
                    success = flag
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("CommentsController.DeleteComment", "异常位置：CommentsController.DeleteComment" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}