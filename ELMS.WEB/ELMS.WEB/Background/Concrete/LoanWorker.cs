using ELMS.WEB.Background.Interfaces;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Managers.Loan.Interface;
using ELMS.WEB.Models.Equipment.Response;
using ELMS.WEB.Models.Loan.Response;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ELMS.WEB.Background.Concrete
{
    public class LoanWorker : ILoanWorker
    {
        private readonly ILogger __Logger;
        private readonly IServiceProvider __ServiceProvider;
        private readonly string WORKER_NAME = "LoanWorker";

        public LoanWorker(ILogger<LoanWorker> logger, IServiceProvider serviceProvider)
        {
            __Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            __ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(await UpdateExpiredLoansStatusesAsync());
                await Task.Delay(await PruneDanglingLoanEquipmentRecordsAsync());
                await Task.Delay(await PrunePendingLoansAsync());
            }
        }

        private async Task<int> PrunePendingLoansAsync()
        {
            int delaySeconds = 1000 * 60 * 60;

            __Logger.LogInformation($"{WORKER_NAME}: Pruning pending loans");

            using (var scope = __ServiceProvider.CreateScope())
            {
                ILoanManager _LoanManager = scope.ServiceProvider.GetRequiredService<ILoanManager>();

                IList<LoanResponse> _PendingLoans = (await _LoanManager.GetAsync()).Where(x => !x.AcceptedTermsAndConditions && (DateTime.Now - x.CreatedTimestamp).TotalDays >= 3).ToList();

                if (_PendingLoans != null && _PendingLoans.Count > 0)
                {
                    foreach (LoanResponse loan in _PendingLoans)
                    {
                        await _LoanManager.DeleteAsync(loan.UID);
                    }

                    __Logger.LogInformation($"{WORKER_NAME}: Pruned {_PendingLoans.Count} pending loans.");
                }
                else
                {
                    __Logger.LogInformation($"{WORKER_NAME}: No pending loans found to prune.");
                }

                // IConfiguration to retrieve Lead delay time for Pruning
            }

            __Logger.LogInformation($"LoanWorker: Pruning pending loans completed. Task scheduled again in {delaySeconds} milliseconds.");

            return delaySeconds;
        }

        private async Task<int> PruneDanglingLoanEquipmentRecordsAsync()
        {
            int delaySeconds = 1000 * 60 * 60 * 6;

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
                    __Logger.LogInformation($"{WORKER_NAME}: Removed {_UnassociatedLoanEquipmentUIDList.Count} dangling LoanEquipment records.");
                }
            }

            __Logger.LogInformation($"LoanWorker: Pruning dangling Loan-equipment records completed. Task scheduled again in {delaySeconds} milliseconds.");

            return delaySeconds;
        }

        private async Task<int> UpdateExpiredLoansStatusesAsync() 
        {
            int delaySeconds = 1000 * 60 * 60 * 2;

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

                __Logger.LogInformation($"LoanWorker: {updatedLoans} loans have expired and their statuses have been updated to 'Expired'. Task scheduled again in {delaySeconds} milliseconds.");
            }

            return delaySeconds;
        }

    }
}
