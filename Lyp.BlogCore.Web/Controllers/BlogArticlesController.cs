using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lyp.BlogCore.Models.Models;
using Lyp.BlogCore.Repository.MySqlEFCore;

namespace Lyp.BlogCore.Web.Controllers
{
    public class BlogArticlesController : Controller
    {
        private readonly MySqlDbContext _context;

        public BlogArticlesController(MySqlDbContext context)
        {
            _context = context;
        }

        // GET: BlogArticles
        public async Task<IActionResult> Index()
        {
            return View(await _context.blogArticles.ToListAsync());
        }

        // GET: BlogArticles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogArticle = await _context.blogArticles
                .FirstOrDefaultAsync(m => m.bID == id);
            if (blogArticle == null)
            {
                return NotFound();
            }

            return View(blogArticle);
        }

        // GET: BlogArticles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogArticles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("bID,bAuthor,bTitle,bContent,bCreateTime,bReadNum,bCommentNum,cID")] BlogArticle blogArticle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blogArticle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blogArticle);
        }

        // GET: BlogArticles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogArticle = await _context.blogArticles.FindAsync(id);
            if (blogArticle == null)
            {
                return NotFound();
            }
            return View(blogArticle);
        }

        // POST: BlogArticles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("bID,bAuthor,bTitle,bContent,bCreateTime,bReadNum,bCommentNum,cID")] BlogArticle blogArticle)
        {
            if (id != blogArticle.bID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogArticleExists(blogArticle.bID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blogArticle);
        }

        // GET: BlogArticles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogArticle = await _context.blogArticles
                .FirstOrDefaultAsync(m => m.bID == id);
            if (blogArticle == null)
            {
                return NotFound();
            }

            return View(blogArticle);
        }

        // POST: BlogArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogArticle = await _context.blogArticles.FindAsync(id);
            _context.blogArticles.Remove(blogArticle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogArticleExists(int id)
        {
            return _context.blogArticles.Any(e => e.bID == id);
        }
    }
}
