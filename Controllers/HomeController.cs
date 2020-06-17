using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChaVoV1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ChaVoV1.Controllers
{
    public class HomeController : Controller
    {
        DataContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            this._context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public JsonResult Search(string text)
        {
            int id = -1;
            try
            {
                id = int.Parse(text);
            }
            catch (System.Exception)
            {

            }
            text = id == 0 ? null : text;
            // var li = text == null ? _context.Questions.OrderByDescending(p => p.PubDate).ToList() : _context.Questions.Include(q => q.Category).Where(q => q.QuestionText == text || q.QuestionTitle == text || q.Category.CategoryText == text || q.Category.Id == id).ToList();
            List<Question> li = new List<Question>();
            if(text == null)
            {
                li = _context.Questions.OrderByDescending(p=>p.PubDate).ToList();
            }
            else
            {
                var AllQuestions = _context.Questions.OrderByDescending(p=>p.PubDate).ToList();
                foreach (var x in AllQuestions)
                {
                    var splitedText = x.QuestionTitle.Split(' ');
                    for (int i = 0; i < splitedText.Length; i++)
                    {
                        if(splitedText[i] == text)
                        {
                            li.Add(x);
                            break;
                        }
                    }
                }
            }
            List<ShortDataQuestion> QuestionLi = new List<ShortDataQuestion>();
            li.ForEach(q =>
            {
                QuestionLi.Add(new ShortDataQuestion()
                {
                    Id = q.Id,
                    Title = q.QuestionTitle,
                    PubDate = q.PubDate.ToShortDateString(),
                    CountOfAnswers = _context.Answers.Where(a => a.Question == q).Count()
                });
            });
            return new JsonResult(QuestionLi);
        }

        public JsonResult Questions()
        {
            var li = _context.Questions.OrderByDescending(p => p.PubDate).ToList();
            List<ShortDataQuestion> QuestionLi = new List<ShortDataQuestion>();
            li.ForEach(q =>
            {
                QuestionLi.Add(new ShortDataQuestion()
                {
                    Id = q.Id,
                    Title = q.QuestionTitle,
                    PubDate = q.PubDate.ToShortDateString(),
                    CountOfAnswers = _context.Answers.Where(a => a.Question == q).Count()
                });
            });
            return new JsonResult(QuestionLi);
        }

        [HttpGet]
        public IActionResult Question(int id)
        {
            var model = _context.Questions.Single(q => q.Id == id);
            return View(model);
        }

        [HttpGet]
        public JsonResult Answers(int id)
        {
            var model = _context.Answers.Include(a => a.Question).Include(a => a.Writer).Where(a => a.Question.Id == id).OrderByDescending(a => a).ToList();
            List<ShortData> li = new List<ShortData>();
            model.ForEach(a =>
            {
                li.Add(new ShortData()
                {
                    text = a.AnswerText,
                    UserName = a.Writer.FirstName
                });
            });
            return new JsonResult(li);
        }
        [HttpPost]
        public string AddAnswer(ShortData model)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    var QuestionModel = _context.Questions.Single(p => p.Id == model.id);
                    var UserModel = _context.Users.Single(p => p.Login == User.Identity.Name);
                    _context.Answers.Add(new Answer()
                    {
                        AnswerText = model.text,
                        Question = QuestionModel,
                        Writer = UserModel
                    });
                    _context.SaveChanges();
                    return "Ok";
                }
                catch (Exception ex)
                {
                    return "Error";
                }
            }
            return "NotAuthed";
        }

        [HttpPost]
        public int AddCategory(string category)
        {
            _context.Categories.Add(new Category()
            {
                CategoryText = category
            });
            _context.SaveChanges();
            var model = _context.Categories.SingleOrDefault(c => c.CategoryText == category);
            return model.Id;
        }
        public JsonResult Categories()
        {
            var li = _context.Categories.ToList();
            return new JsonResult(li);
        }
    }
    public class ShortData
    {
        public int id { get; set; }
        public string text { get; set; }
        public string UserName { get; set; }
    }
    public class ShortDataQuestion
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PubDate { get; set; }
        public int CountOfAnswers { get; set; }
    }
}
