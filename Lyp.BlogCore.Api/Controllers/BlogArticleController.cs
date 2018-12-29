using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Core.Log;
using Lyp.BlogCore.Api.AOP;
using Lyp.BlogCore.Common.Helper;
using Lyp.BlogCore.Common.Redis;
using Lyp.BlogCore.IRepository;
using Lyp.BlogCore.IServices;
using Lyp.BlogCore.Models.Models;
using Lyp.BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;

namespace Lyp.BlogCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[UserAttribute]
    public class BlogArticleController : ControllerBase
    {
        private readonly IBlogArticleService blogArticleService;
        private readonly ICategoryService categoryService;
        private readonly ILoggerHelper loggerHelper;
        private readonly IMapper mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="blogArticle"></param>
        /// <param name="loggerHelper"></param>
        public BlogArticleController(IBlogArticleService blogArticle, ICategoryService categoryService, ILoggerHelper loggerHelper, IMapper mapper)
        {
            this.blogArticleService = blogArticle;
            this.categoryService = categoryService;
            this.loggerHelper = loggerHelper;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetBlogArticle(int page=1,int bcategory=0,bool isAsc=false,string searchString=null,int pageIndex=2)
        {
            try
            {
                if (string.IsNullOrEmpty(searchString))
                {
                    if (RedisCache.Exists("blogList" + page) &&RedisCache.Exists("blogCount"+page))
                    {
                        var blogList = RedisCache.GetStringKey("blogList" + page);
                        return Ok(new
                        {
                            success = true,
                            page = page,
                            pageCount = RedisCache.GetStringKey("blogCount" + page),
                            data = Newtonsoft.Json.JsonConvert.DeserializeObject(blogList)
                        });
                    }
                }
                
                int pageCount = 0;//页数
                int count = 0;
                List<BlogArticle> blogArticleList = new List<BlogArticle>();

                if (bcategory==0)
                {
                    count= (await blogArticleService.Query(s => s.cID > bcategory)).Count;
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        blogArticleList = await blogArticleService.QueryPage(s => s.cID > bcategory && (s.bTitle.Contains(searchString) || s.bContent.Contains(searchString)), x => x.bCreateTime, page, pageIndex, isAsc);
                        count = (await blogArticleService.Query(s => s.cID > bcategory && (s.bTitle.Contains(searchString) || s.bContent.Contains(searchString)))).Count;
                    }
                    else
                    {
                        blogArticleList = await blogArticleService.QueryPage(s => s.cID > bcategory, x => x.bCreateTime, page, pageIndex, isAsc);
                    }
                }
                else
                {
                    count = (await blogArticleService.Query(s => s.cID == bcategory)).Count;
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        blogArticleList = await blogArticleService.QueryPage(s => s.cID == bcategory && (s.bTitle.Contains(searchString) || s.bContent.Contains(searchString)), x => x.bCreateTime, page, pageIndex, isAsc);
                        count = (await blogArticleService.Query(s => s.cID == bcategory && (s.bTitle.Contains(searchString) || s.bContent.Contains(searchString)))).Count;
                    }
                    else
                    {
                        blogArticleList = await blogArticleService.QueryPage(s => s.cID == bcategory, x => x.bCreateTime, page, pageIndex, isAsc);
                    }
                }
                
                pageCount = count % pageIndex != 0 ? count / pageIndex + 1 : count / pageIndex;

                //设置缓存
                
                RedisCache.SetStringKey("blogList" + page, Newtonsoft.Json.JsonConvert.SerializeObject(blogArticleList), TimeSpan.FromMinutes(20));
                RedisCache.SetStringKey("blogCount" + page, pageCount.ToString(), TimeSpan.FromMinutes(10));

                return Ok(new
                {
                    success = true,
                    page = page,
                    pageCount = pageCount,
                    data = blogArticleList
                });
            }
            catch(Exception ex)
            {
                loggerHelper.Error("BlogArticleController.GetBlogArticle", "异常位置：BlogArticleController.GetBlogArticle" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [Route("Rank")]
        [HttpGet]
        public async Task<IActionResult> RankBlogArticle()
        {
            try
            {
                var blogArticleList = await blogArticleService.QueryPage(s => s.cID > 0, x => x.bReadNum, 1, 6, false);

                return Ok(new
                {
                    syccess = true,
                    data = blogArticleList
                });
            }
            catch(Exception ex)
            {
                loggerHelper.Error("BlogArticleController.RankBlogArticle", "异常位置：BlogArticleController.RankBlogArticle" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpGet("{id}", Name = "Get")]
        //[Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetBlogArticle(int id)
        {
            try
            {
                var model = await blogArticleService.Query(s => s.bID == id);
                var category = await categoryService.Query(c => c.cID == model[0].cID);

                var BlogDetail = mapper.Map<List<BlogDetailVM>>(model);
                BlogDetail[0].Category = category[0].cName;

                return Ok(new
                {
                    success = true,
                    data = BlogDetail
                });
            }
            catch (Exception ex)
            {
                loggerHelper.Error("BlogArticleController.GetBlogArticle", "异常位置：BlogArticleController.GetBlogArticle" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlogArticle([FromRoute] int id,[FromBody] BlogArticle blogArticle)
        {
            try
            {
                if (id != blogArticle.bID)
                {
                    return BadRequest();
                }

                string Read = HttpContext.Session.GetString("Read"+id);

                BlogArticle blog = (await blogArticleService.Query(b => b.bID == id))[0];

                if (blogArticle.bReadNum != 0)
                {
                    if (!string.IsNullOrEmpty(Read))
                    {
                        if (Read == IPHelper.GetClientUserIp(HttpContext) + id)
                        {
                            return Ok(new
                            {
                                success = true,
                                code = 0
                            });
                        }
                    }
                    blog.bReadNum += 1;
                    HttpContext.Session.SetString("Read"+id, IPHelper.GetClientUserIp(HttpContext) + id);
                }
                if (blogArticle.bCommentNum != 0)
                {
                    blog.bCommentNum += 1;
                }


                bool flag = await blogArticleService.Update(blog);

                return Ok(new
                {
                    success = flag,
                    code = 1
                });
            }
            catch(Exception ex)
            {
                loggerHelper.Error("BlogArticleController.PutBlogArticle", "异常位置：BlogArticleController.PutBlogArticle" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostBlogArticle([FromBody] BlogArticle blogArticle)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool flag = false;
                string username = HttpContext.Session.GetString("UserName");
                if (!string.IsNullOrEmpty(username))
                {
                    blogArticle.bCreateTime = DateTime.Now;
                    flag = await blogArticleService.Add(blogArticle);
                }
                //清除缓存
                RedisCache.DeleteKeys("blog");

                return Ok(new
                {
                    success = flag
                });
            }
            catch(Exception ex)
            {
                loggerHelper.Error("BlogArticleController.PostBlogArticle", "异常位置：BlogArticleController.PostBlogArticle" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogArticle([FromRoute] int id)
        {
            try
            {
                bool flag = await blogArticleService.DeleteById(id);

                return Ok(new
                {
                    success = flag
                });
            }
            catch(Exception ex)
            {
                loggerHelper.Error("BlogArticleController.DeleteBlogArticle", "异常位置：BlogArticleController.DeleteBlogArticle" + "异常消息：" + ex.Message);
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}