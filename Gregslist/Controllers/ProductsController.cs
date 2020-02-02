using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gregslist.Data;
using Gregslist.Models;
using Gregslist.Services;
using Gregslist.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Gregslist.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IPostingService _postingService;
        private readonly UserManager<User> _userManager;

        public ProductsController(
            IPostingService postingService,
            UserManager<User> userManager
            )
        {
            _postingService = postingService;
            _userManager = userManager;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _postingService.GetAll());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _postingService.GetById(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                var result = await _postingService.Create(new Product
                {
                    Title = product.Title,
                    Description = product.Description,
                    Price = product.Price,
                    Owner = await _userManager.FindByNameAsync(User.Identity.Name)
                });
                return RedirectToAction("Details", new { id = result.ID });
            }
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var product = await _postingService.GetById(id.Value);
            if (product == null || product.Owner.UserName != User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] Product product)
        {
            if (product.Owner.UserName == User.Identity.Name)
            {
                var result = await _postingService.Edit(product);
                return View("Details", result);
            }
            return RedirectToAction("Index");
        }

        // POST: Products/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id)
        {
            if ((await _postingService.GetById(id)).Owner.UserName == User.Identity.Name)
                await _postingService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
