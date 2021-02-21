using AutoMapper;
using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.Areas.Report.Models;
using ELMS.WEB.Enums.General;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Managers.Loan.Interface;
using ELMS.WEB.Models.Equipment.Response;
using ELMS.WEB.Models.Loan.Response;
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
        private readonly IMapper __Mapper;
        private readonly IEquipmentManager __EquipmentManager;
        private readonly ILoanManager __LoanManager;

        public ReportController(IMapper mapper, IEquipmentManager equipmentManager, ILoanManager loanManager)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __EquipmentManager = equipmentManager ?? throw new ArgumentNullException(nameof(equipmentManager));
            __LoanManager = loanManager ?? throw new ArgumentNullException(nameof(loanManager));
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
            _Model.ReportItems = _EquipmentList?.Equipments?.Select(x => new EquipmentValueReportItemViewModel
            {
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

        [HttpGet]
        public async Task<IActionResult> LoanHistoryReportAsync()
        {
            LoanHistoryViewModel _Model = new LoanHistoryViewModel();

            IList<LoanResponse> _Response = await __LoanManager.GetAsync();


            _Model.Loans = _Response?.Select(x => new LoanHistoryItemViewModel
            {
                UID = x.UID,
                CreatedTimestamp = x.CreatedTimestamp,
                AmendedTimestamp = x.AmendedTimestamp,
                EquipmentList = __Mapper.Map<IList<EquipmentViewModel>>(x.EquipmentList),
                ExpiryTimestamp = x.ExpiryTimestamp,
                LoaneeEmail = x.LoaneeEmail,
                LoanerEmail = x.LoanerEmail,
                AcceptedTermsAndConditions = x.AcceptedTermsAndConditions,
                FromTimestamp = x.FromTimestamp,
                Name = x.Name,
                Status = x.Status
            }).ToList();

            return View(_Model);
        }

        [HttpPost]
        public async Task<IActionResult> LoanHistoryReportFilterAsync(LoanHistoryReportFilterViewModel filter)
        {
            IList<LoanResponse> _Response = await __LoanManager.GetAsync();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                _Response = _Response.Where(x => x.Name.ToUpper().Contains(filter.Name.ToUpper())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(filter.LoaneeEmail))
            {
                _Response = _Response.Where(x => x.LoaneeEmail.ToUpper().Contains(filter.LoaneeEmail.ToUpper())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(filter.LoanerEmail))
            {
                _Response = _Response.Where(x => x.LoanerEmail.ToUpper().Contains(filter.LoanerEmail.ToUpper())).ToList();
            }

            if (filter.FromTimestamp != null)
            {
                _Response = _Response.Where(x => x.FromTimestamp >= filter.FromTimestamp).ToList();
            }

            if (filter.ExpiryTimestamp != null)
            {
                _Response = _Response.Where(x => x.FromTimestamp <= filter.ExpiryTimestamp).ToList();
            }

            if (filter.CreatedFromTimestamp != null)
            {
                _Response = _Response.Where(x => x.CreatedTimestamp >= filter.CreatedFromTimestamp).ToList();
            }

            if (filter.CreatedToTimestamp != null)
            {
                _Response = _Response.Where(x => x.CreatedTimestamp <= filter.CreatedToTimestamp).ToList();
            }

            if (filter.AcceptedTermsAndConditions != BooleanFilter.All)
            {
                _Response = (filter.AcceptedTermsAndConditions == BooleanFilter.True) ?
                    _Response.Where(x => x.AcceptedTermsAndConditions).ToList() :
                    _Response.Where(x => !x.AcceptedTermsAndConditions).ToList();
            }

            if (filter.FromTimestamp != null)
            {
                _Response = _Response.Where(x => x.FromTimestamp >= filter.FromTimestamp).ToList();
            }

            if (filter.ExpiryTimestamp != null)
            {
                _Response = _Response.Where(x => x.ExpiryTimestamp <= filter.ExpiryTimestamp).ToList();
            }

            if (filter.Statuses != null && filter.Statuses?.Count > 0)
            {
                _Response = _Response.Where(x => filter.Statuses.Contains(x.Status)).ToList();
            }

            IList<LoanHistoryItemViewModel> _Loans = _Response?.Select(x => new LoanHistoryItemViewModel
            {
                UID = x.UID,
                CreatedTimestamp = x.CreatedTimestamp,
                AmendedTimestamp = x.AmendedTimestamp,
                EquipmentList = __Mapper.Map<IList<EquipmentViewModel>>(x.EquipmentList),
                ExpiryTimestamp = x.ExpiryTimestamp,
                LoaneeEmail = x.LoaneeEmail,
                LoanerEmail = x.LoanerEmail,
                AcceptedTermsAndConditions = x.AcceptedTermsAndConditions,
                FromTimestamp = x.FromTimestamp,
                Name = x.Name,
                Status = x.Status
            }).ToList();

            return View("LoanHistoryReport", new LoanHistoryViewModel
            {
                Loans = _Loans,
                Filter = filter
            });
        }
    }
}
