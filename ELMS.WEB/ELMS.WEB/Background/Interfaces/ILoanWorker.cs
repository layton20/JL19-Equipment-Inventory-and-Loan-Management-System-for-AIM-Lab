using System.Threading;
using System.Threading.Tasks;

namespace ELMS.WEB.Background.Interfaces
{
    public interface ILoanWorker
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}
