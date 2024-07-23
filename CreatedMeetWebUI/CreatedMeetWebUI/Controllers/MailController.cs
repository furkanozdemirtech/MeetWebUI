using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CreatedMeetWebUI.Controllers
{
    [Authorize]
    public class MailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
