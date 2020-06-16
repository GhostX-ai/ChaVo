using System.Linq;
using System.Threading.Tasks;
using ChaVoV1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChaVo.Areas.Admin.Controllers
{
    public class CategoriesController : AdminHelper
    {
        DataContext _context;
        public CategoriesController(DataContext context)
        {   
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            var li = await _context.Categories.ToListAsync();
            return View(li);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _context.Categories.FirstAsync(c=>c.Id == id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category model)
        {
            var lastmodel = await _context.Categories.FirstAsync(c=>c.Id == model.Id);
            lastmodel.CategoryText = model.CategoryText;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _context.Categories.FirstAsync(c=>c.Id == id);
            _context.Categories.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}