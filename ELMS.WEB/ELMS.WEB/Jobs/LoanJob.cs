using ELMS.WEB.Managers.Admin.Interfaces;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Managers.Loan.Interface;
using ELMS.WEB.Models.Admin.Response;
using ELMS.WEB.Models.Equipment.Response;
using ELMS.WEB.Models.Loan.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Jobs
{
    public class LoanJob : IJob
    {
        private readonly IServiceProvider __ServiceProvider;
        private readonly IConfiguration __Configuration;
        private readonly string WORKER_NAME = "Loan Job Worker";

        public LoanJob(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            __ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            __Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Debug.WriteLine($"{WORKER_NAME}: Task Beginning");

            await PrunePendingLoansAsync();
            await PruneDanglingLoanEquipmentRecordsAsync();
            await UpdateExpiredLoansStatusesAsync();

            Debug.WriteLine($"{WORKER_NAME}: Task Complete");
        }

        private async Task PrunePendingLoansAsync()
        {
            Debug.WriteLine($"{WORKER_NAME}: Pruning pending loans");

            using (var scope = __ServiceProvider.CreateScope())
            {
                ILoanManager _LoanManager = scope.ServiceProvider.GetRequiredService<ILoanManager>();
                IConfigurationManager _ConfigurationManager = scope.ServiceProvider.GetRequiredService<IConfigurationManager>();
                ConfigurationResponse _ExpiryDaysResponse = await _ConfigurationManager.GetByNormalizedNameAsync(__Configuration.GetValue<string>("Configuration:Loan:PENDING_LOAN_EXPIRY_DAYS"));

                IList<LoanResponse> _PendingLoans = new List<LoanResponse>();

                if (int.TryParse(_ExpiryDaysResponse.Value, out int _ExpiryDays))
                {
                    _PendingLoans = (await _LoanManager.GetAsync()).Where(x => !x.AcceptedTermsAndConditions && (DateTime.Now - x.CreatedTimestamp).TotalDays >= 3).ToList();
                }
                else
                {
                    _PendingLoans = (await _LoanManager.GetAsync()).Where(x => !x.AcceptedTermsAndConditions && (DateTime.Now - x.CreatedTimestamp).TotalDays >= 3).ToList();
                }

                if (_PendingLoans != null && _PendingLoans.Count > 0)
                {
                    foreach (LoanResponse loan in _PendingLoans)
                    {
                        await _LoanManager.DeleteAsync(loan.UID);
                    }

                    Debug.WriteLine($"{WORKER_NAME}: Pruned {_PendingLoans.Count} pending loans.");
                }
            }
        }

        private async Task PruneDanglingLoanEquipmentRecordsAsync()
        {
            using (var scope = __ServiceProvider.CreateScope())
            {
                ILoanManager _LoanManager = scope.ServiceProvider.GetRequiredService<ILoanManager>();
                IList<Guid> _LoanUIDs = (await _LoanManager.GetAsync()).Select(x => x.UID).ToList();
                ILoanEquipmentManager _LoanEquipmentManager = scope.ServiceProvider.GetRequiredService<ILoanEquipmentManager>();
                IList<Guid> _UnassociatedLoanEquipmentUIDList = (await _LoanEquipmentManager.GetAsync()).Where(x => !_LoanUIDs.Contains(x.LoanUID)).Select(x => x.UID).ToList();

                foreach (Guid uid in _UnassociatedLoanEquipmentUIDList)
                {
                    await _LoanEquipmentManager.DeleteAsync(uid);
                }

                if (_UnassociatedLoanEquipmentUIDList?.Count > 0)
                {
                    Debug.WriteLine($"{WORKER_NAME}: Removed {_UnassociatedLoanEquipmentUIDList.Count} dangling LoanEquipment records.");
                }
            }
        }

        private async Task UpdateExpiredLoansStatusesAsync()
        {
            using (var scope = __ServiceProvider.CreateScope())
            {
                ILoanManager _LoanManager = scope.ServiceProvider.GetRequiredService<ILoanManager>();

                int updatedLoans = 0;

                foreach (LoanResponse loan in await _LoanManager.GetAsync())
                {
                    if (await _LoanManager.GetExpiryDate(loan.UID) <= DateTime.Now && (loan.Status != Enums.Loan.Status.Complete || loan.Status != Enums.Loan.Status.ManualComplete))
                    {
                        await _LoanManager.ChangeStatusAsync(loan.UID, Enums.Loan.Status.Complete);

                        IEquipmentManager _EquipmentManager = scope.ServiceProvider.GetRequiredService<IEquipmentManager>();

                        foreach (EquipmentResponse equipment in loan.EquipmentList.Where(x => x.Status != Enums.Equipment.Status.Available))
                        {
                            await _EquipmentManager.UpdateStatusAsync(equipment.UID, Enums.Equipment.Status.Available);
                        }

                        updatedLoans++;
                    }
                }

                Debug.WriteLine($"LoanWorker: {updatedLoans} loans have expired and their statuses have been updated to 'Expired'.");
            }
        }
    }
}