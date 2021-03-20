using AutoMapper;
using ELMS.WEB.Areas.Admin.Models.Calendar;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Managers.Loan.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class CalendarController : Controller
    {
        private readonly IMapper __Mapper;
        private readonly ILoanManager __LoanManager;
        private readonly IEquipmentManager __EquipmentManager;

        public CalendarController(IMapper mapper, ILoanManager loanManager, IEquipmentManager equipmentManager)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __LoanManager = loanManager ?? throw new ArgumentNullException(nameof(loanManager));
            __EquipmentManager = equipmentManager ?? throw new ArgumentNullException(nameof(equipmentManager));
        }

        [Authorize(Policy = "ViewCalendarPolicy")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IndexViewModel _Model = new IndexViewModel()
            {
                Loans = __Mapper.Map<IList<CalendarItemLoanViewModel>>(await __LoanManager.GetAsync()),
                Equipment = __Mapper.Map<IList<CalendarItemEquipmentViewModel>>(await __EquipmentManager.GetAsync())
            };

            return View(_Model);
        }
    }
}