using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChaVo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminHelper : Controller
    {

    }
}