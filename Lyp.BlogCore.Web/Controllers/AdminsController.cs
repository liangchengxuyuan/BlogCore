using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lyp.BlogCore.Models.Models;
using Lyp.BlogCore.Repository.MySqlEFCore;
using Lyp.BlogCore.IServices;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Lyp.BlogCore.Models.ViewModels;
using Lyp.BlogCore.Common;
using Blog.Core.Log;

namespace Lyp.BlogCore.Web.Controllers
{
    public class AdminsController : Controller
    {
        //private readonly MySqlDbContext _context;
        private readonly IAdminService adminService;
        private readonly IMapper IMapper;
        private readonly ILoggerHelper loggerHelper;

        public AdminsController(IAdminService admin, IMapper IMapper, ILoggerHelper loggerHelper)
        {
            //_context = context;
            this.adminService=admin;
            this.IMapper = IMapper;
            this.loggerHelper = loggerHelper;
        }

        // GET: Admins
        public async Task<IActionResult> Index()
        {
            var admin = await adminService.Query();

            List<AdminViewModel> list = IMapper.Map<List<AdminViewModel>>(admin);
            return View(admin);
        }

        // GET: Admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await adminService.Query(s=>s.ID==id);
            if (admin.FirstOrDefault() == null)
            {
                return NotFound();
            }

            return View(admin.FirstOrDefault());
        }

        // GET: Admins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserName,PassWord")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                await adminService.Add(admin);
               
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }

        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await adminService.Query(s => s.ID == id);
            if (admin.FirstOrDefault() == null)
            {
                return NotFound();
            }
            return View(admin.FirstOrDefault());
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserName,PassWord")] Admin admin)
        {
            if (id != admin.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await adminService.Update(admin);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.ID))
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
            return View(admin);
        }

        // GET: Admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await adminService.Query(s => s.ID == id);

            if (admin.FirstOrDefault() == null)
            {
                return NotFound();
            }

            return View(admin.FirstOrDefault());
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await adminService.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(int id)
        {
            return true;
        }
    }
}
