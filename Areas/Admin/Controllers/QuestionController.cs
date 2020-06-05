using Microsoft.AspNetCore.Mvc;

namespace ChaVo.Areas.Admin.Controllers
{
    public class QuestionController : AdminHelper
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}