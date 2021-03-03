using AutoMapper;
using ELMS.WEB.Areas.Admin.Models.Dashboard;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Managers.Loan.Interface;
using ELMS.WEB.Models.Loan.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NsEquipment = ELMS.WEB.Areas.Equipment;
using NsLoan = ELMS.WEB.Areas.Loan;

namespace ELMS.WEB.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IMapper __Mapper;
        private readonly IEquipmentManager __EquipmentManager;
        private readonly ILoanManager __LoanManager;

        public DashboardController(IMapper mapper, IEquipmentManager equipmentManager, ILoanManager loanManager)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __EquipmentManager = equipmentManager ?? throw new ArgumentNullException(nameof(equipmentManager));
            __LoanManager = loanManager ?? throw new ArgumentNullException(nameof(loanManager));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IndexViewModel _Model = new IndexViewModel
            {
                EquipmentList = __Mapper.Map<IList<NsEquipment.Models.EquipmentViewModel>>((await __EquipmentManager.GetAsync()).Equipments),
                Loans = __Mapper.Map<IList<NsLoan.Models.LoanViewModel>>(await __LoanManager.GetAsync()),
            };

            return View("Index", _Model);
        }
    }
}
