using System.Threading;
using System.Threading.Tasks;

namespace ELMS.WEB.Background.Interfaces
{
    public interface IEmailWorker
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}