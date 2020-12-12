using Microsoft.AspNetCore.Mvc;

namespace ELMS.WEB.Areas.Equipment.Controllers
{
    public class NoteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}