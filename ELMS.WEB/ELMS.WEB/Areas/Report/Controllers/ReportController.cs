using ELMS.WEB.Areas.Report.Models;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Models.Equipment.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.Report.Controllers
{
    [Authorize]
    [Area("Report")]
    public class ReportController : Controller
    {
        private readonly IEquipmentManager __EquipmentManager;

        public ReportController(IEquipmentManager equipmentManager)
        {
            __EquipmentManager = equipmentManager ?? throw new ArgumentNullException(nameof(equipmentManager));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EquipmentValueReportAsync()
        {
            EquipmentValueReportViewModel _Model = new EquipmentValueReportViewModel();

            EquipmentListResponse _EquipmentList = await __EquipmentManager.GetAsync();
            _Model.ReportItems = _EquipmentList?.Equipments?.Select(x => new EquipmentValueReportItemViewModel { 
                Name = x.Name,
                SerialNumber = x.SerialNumber,
                PurchasePrice = x.PurchasePrice,
                ReplacementPrice = x.PurchasePrice,
                WarrantyExpirationDate = x.WarrantyExpirationDate,
                PurchaseDate = x.PurchaseDate,
                Status = x.Status
            })?.ToList();

            return View(_Model);
        }
    }
}
