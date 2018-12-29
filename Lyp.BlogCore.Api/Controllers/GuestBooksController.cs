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
    public class GuestBooksController : ControllerBase
    {
        private readonly IGuestBookService guestBookService;
        private readonly ILoggerHelper loggerHelper;

        public GuestBooksController(IGuestBookService guestBook, ILoggerHelper logger)
        {
            this.guestBookService = guestBook;
            this.loggerHelper = logger;
        }

        // GET: api/GuestBooks
        [HttpGet]
        public async Task<IActionResult> GetguestBooks(int pageIndex = 1, int pageSize = 7, bool isAsc = false)
        {
            try
            {
                int pageCount = 0;
                var guestCount = await guestBookService.Query();
                List<GuestBook> guestbookList = new List<GuestBook>();
                guestbookList = await guestBookService.QueryPage(s => s.gID > 0, x => x.gCreateTime, pageIndex, pageSize, isAsc);
                pageCount = guestCount.Count() % pageSize != 0 ? guestCount.Count() / pageSize + 1 : guestCount.Count() / pageSize;

                return Ok(new
                {
                    success = true,
                    page = pageIndex,
                    pageCount = pageCount,
                    data = guestbookList
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("GuestBooksController.GetguestBooks", "异常位置：GuestBooksController.GetguestBooks" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // GET: api/GuestBooks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGuestBook([FromRoute] int id)
        {
            try
            {
                var guestBook = await guestBookService.Query(s => s.gID == id);

                if (guestBook == null)
                {
                    return NotFound();
                }

                return Ok(new
                {
                    success = true,
                    data = guestBook
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("GuestBooksController.GetGuestBook", "异常位置：GuestBooksController.GetGuestBook" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // PUT: api/GuestBooks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGuestBook([FromRoute] int id, [FromBody] GuestBook guestBook)
        {
            try
            {
                if (id != guestBook.gID)
                {
                    return BadRequest();
                }
                bool flag = await guestBookService.Update(guestBook);

                return Ok(new
                {
                    success = flag
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("GuestBooksController.PutGuestBook", "异常位置：GuestBooksController.PutGuestBook" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // POST: api/GuestBooks
        [HttpPost]
        public async Task<IActionResult> PostGuestBook([FromBody] GuestBook guestBook)
        {
            try
            {
                string username = HttpContext.Session.GetString("UserName");

                //未登录
                if (string.IsNullOrEmpty(username))
                {
                    return Ok(new
                    {
                        success = false,
                        code = 0
                    });
                }

                guestBook.gUserName = username;
                guestBook.gCreateTime = DateTime.Now;
                bool flag = await guestBookService.Add(guestBook);

                return Ok(new
                {
                    success = flag,
                    code = 1
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("GuestBooksController.PostGuestBook", "异常位置：GuestBooksController.PostGuestBook" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // DELETE: api/GuestBooks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuestBook([FromRoute] int id)
        {
            try
            {
                bool flag = await guestBookService.DeleteById(id);

                return Ok(new
                {
                    success = flag
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("GuestBooksController.DeleteGuestBook", "异常位置：GuestBooksController.DeleteGuestBook" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}