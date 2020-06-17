using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChaVoV1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChaVo.Areas.Admin.Controllers
{
    public class QuestionController : AdminHelper
    {
        DataContext _context;
        public QuestionController(DataContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> List()
        {
            ViewBag.Categories = _context.Categories.ToList();
            var li = await _context.Questions.OrderBy(p => p.PubDate).ToListAsync();
            return View(li);
        }
        [HttpPost]
        public async Task<IActionResult> List(int id)
        {
            ViewBag.Categories = _context.Categories.ToList();
            var li = id == 0 ? await _context.Questions.OrderBy(p => p.PubDate).ToListAsync() : await _context.Questions.Include(q => q.Category).Where(q => q.Category.Id == id).OrderBy(p => p.PubDate).ToListAsync();
            return View(li);
        }
        public async Task<IActionResult> Add()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Question model, int CategoryId)
        {
            model.PubDate = DateTime.Now.Date;
            var _categoryModel = await _context.Categories.SingleAsync(c => c.Id == CategoryId);
            model.Category = _categoryModel;
            _context.Questions.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _context.Questions.Include(q => q.Category).SingleAsync(q => q.Id == id);
                ViewBag.Categories = await _context.Categories.OrderByDescending(p => p.Id == model.Category.Id).ToListAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("List");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Question model, int CategoryId)
        {
            try
            {
                var _categoryModel = await _context.Categories.SingleAsync(c => c.Id == CategoryId);
                var lastmodel = await _context.Questions.SingleAsync(q => q.Id == model.Id);
                lastmodel.Category = _categoryModel;
                lastmodel.QuestionText = model.QuestionText;
                lastmodel.QuestionTitle = model.QuestionTitle;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
            return RedirectToAction("List");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _context.Questions.SingleAsync(q => q.Id == id);
            var li = await _context.Answers.Include(a => a.Question).Where(a=> a.Question.Id == id).ToListAsync();
            foreach (var x in li)
            {
                var answer = await _context.Answers.FirstAsync(c=>c.Id == x.Id);
                _context.Answers.Remove(answer);
                await _context.SaveChangesAsync();
            }
            _context.Questions.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }
    }
}