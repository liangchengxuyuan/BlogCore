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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly ILoggerHelper loggerHelper;

        public CategoriesController(ICategoryService category, ILoggerHelper logger)
        {
            this.categoryService = category;
            this.loggerHelper = logger;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<IActionResult> Getcategories()
        {
            try
            {
                var categoryList = await categoryService.Query();
                return Ok(new
                {
                    success = true,
                    data = categoryList
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("CategoriesController.Getcategories", "异常位置：CategoriesController.Getcategories" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory([FromRoute] int id)
        {
            try
            {
                var category = await categoryService.QueryById(s => s.cID == id);

                if (category == null)
                {
                    return NotFound();
                }

                return Ok(new
                {
                    success = true,
                    data = category
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("CategoriesController.GetCategory", "异常位置：CategoriesController.GetCategory" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory([FromRoute] int id, [FromBody] Category category)
        {
            try
            {
                if (id != category.cID)
                {
                    return BadRequest();
                }
                bool flag = await categoryService.Update(category);

                return Ok(new
                {
                    success = flag
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("CategoriesController.PutCategory", "异常位置：CategoriesController.PutCategory" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<IActionResult> PostCategory([FromBody] Category category)
        {
            try
            {
                bool flag = await categoryService.Add(category);

                return Ok(new
                {
                    success = flag
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("CategoriesController.PostCategory", "异常位置：CategoriesController.PostCategory" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                bool falg = await categoryService.DeleteById(id);
                return Ok(new
                {
                    success = falg
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("CategoriesController.DeleteCategory", "异常位置：CategoriesController.DeleteCategory" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}