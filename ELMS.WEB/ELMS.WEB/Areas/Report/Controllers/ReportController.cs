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
        private readonly ILoanExtensionManager __LoanExtensionManager;

        public ReportController(IMapper mapper, IEquipmentManager equipmentManager, ILoanManager loanManager, ILoanExtensionManager loanExtensionManager)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __EquipmentManager = equipmentManager ?? throw new ArgumentNullException(nameof(equipmentManager));
            __LoanManager = loanManager ?? throw new ArgumentNullException(nameof(loanManager));
            __LoanExtensionManager = loanExtensionManager ?? throw new ArgumentNullException(nameof(loanExtensionManager));
        }

        [HttpGet]
        [Authorize(Policy = "ViewReportPolicy")]
        public async Task<IActionResult> EquipmentValueReportAsync()
        {
            EquipmentValueReportViewModel _Model = new EquipmentValueReportViewModel();

            return View(new EquipmentValueReportViewModel
            {
                Filter = new EquipmentValueReportFilterViewModel(),
                ReportItems = __Mapper.Map<IList<EquipmentValueReportItemViewModel>>(await __EquipmentManager.GetAsync())
            });
        }

        [HttpPost]
        [Authorize(Policy = "FilterReportPolicy")]
        public async Task<IActionResult> EquipmentValueReportFilterAsync(EquipmentValueReportFilterViewModel filter)
        {
            IList<EquipmentResponse> _EquipmentResponses = await __EquipmentManager.GetAsync();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                _EquipmentResponses = _EquipmentResponses.Where(x => x.Name.ToUpper().Contains(filter.Name.ToUpper())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(filter.SerialNumber))
            {
                _EquipmentResponses = _EquipmentResponses.Where(x => x.SerialNumber.ToUpper().Contains(filter.SerialNumber.ToUpper())).ToList();
            }

            if (filter.PurchaseDateFrom != null)
            {
                _EquipmentResponses = _EquipmentResponses.Where(x => x.PurchaseDate >= filter.PurchaseDateFrom).ToList();
            }

            if (filter.PurchaseDateTo != null)
            {
                filter.PurchaseDateTo = filter.PurchaseDateTo.Date.AddDays(1).AddSeconds(-1);
                _EquipmentResponses = _EquipmentResponses.Where(x => x.PurchaseDate <= filter.PurchaseDateTo).ToList();
            }

            if (filter.WarrantyExpirationDateFrom != null)
            {
                _EquipmentResponses = _EquipmentResponses.Where(x => x.PurchaseDate >= filter.WarrantyExpirationDateFrom).ToList();
            }

            if (filter.WarrantyExpirationDateTo != null)
            {
                filter.WarrantyExpirationDateTo = filter.WarrantyExpirationDateTo.AddDays(1).AddSeconds(-1);
                _EquipmentResponses = _EquipmentResponses.Where(x => x.PurchaseDate <= filter.WarrantyExpirationDateTo).ToList();
            }

            if (filter.PurchasePriceFrom > 0)
            {
                _EquipmentResponses = _EquipmentResponses.Where(x => x.PurchasePrice >= filter.PurchasePriceFrom).ToList();
            }

            if (filter.PurchasePriceTo < 999999)
            {
                _EquipmentResponses = _EquipmentResponses.Where(x => x.PurchasePrice <= filter.PurchasePriceTo).ToList();
            }

            if (filter.ReplacementPriceFrom > 0)
            {
                _EquipmentResponses = _EquipmentResponses.Where(x => x.ReplacementPrice >= filter.PurchasePriceFrom).ToList();
            }

            if (filter.ReplacementPriceTo < 999999)
            {
                _EquipmentResponses = _EquipmentResponses.Where(x => x.ReplacementPrice <= filter.PurchasePriceFrom).ToList();
            }

            if (filter.Statuses != null && filter.Statuses?.Count > 0)
            {
                _EquipmentResponses = _EquipmentResponses.Where(x => filter.Statuses.Contains(x.Status)).ToList();
            }

            return View("EquipmentValueReport", new EquipmentValueReportViewModel
            {
                ReportItems = __Mapper.Map<IList<EquipmentValueReportItemViewModel>>(_EquipmentResponses),
                Filter = filter
            });
        }

        [HttpGet]
        [Authorize(Policy = "ViewReportPolicy")]
        public async Task<IActionResult> LoanHistoryReportAsync()
        {
            LoanHistoryViewModel _Model = new LoanHistoryViewModel
            {
                Loans = __Mapper.Map<IList<LoanHistoryItemViewModel>>(await __LoanManager.GetAsync())
            };

            return View(_Model);
        }

        [HttpPost]
        [Authorize(Policy = "FilterReportPolicy")]
        public async Task<IActionResult> LoanHistoryReportFilterAsync(LoanHistoryReportFilterViewModel filter)
        {
            IList<LoanResponse> _Response = await __LoanManager.GetAsync();

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
                filter.ExpiryTimestamp = filter.ExpiryTimestamp.Date.AddDays(1).AddSeconds(-1);
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