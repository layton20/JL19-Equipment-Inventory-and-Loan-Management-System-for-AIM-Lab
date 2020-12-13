using Microsoft.AspNetCore.Mvc;

namespace ELMS.WEB.Areas.Equipment.Controllers
{
    public class EquipmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}