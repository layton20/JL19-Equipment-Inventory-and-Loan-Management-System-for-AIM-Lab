using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
