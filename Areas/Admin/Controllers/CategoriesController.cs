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
            var QuestionLi = await _context.Questions.Include(q=>q.Category).Where(q=> q.Category.Id == model.Id).ToListAsync();
            foreach (var x in QuestionLi)
            {
                var QuestionModel = await _context.Questions.FirstAsync(q=>q.Id == x.Id);
                var AnswerLi = await _context.Answers.Include(a=>a.Question).Where(a=>a.Question.Id == x.Id).ToListAsync();
                foreach (var answer in AnswerLi)
                {
                    var AnswerModel = await _context.Answers.FirstAsync(a=>a.Id == answer.Id);
                    _context.Answers.Remove(AnswerModel);
                    await _context.SaveChangesAsync();
                }
                _context.Questions.Remove(QuestionModel);
                await _context.SaveChangesAsync();
            }
            _context.Categories.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}