using System.Threading;
using System.Threading.Tasks;

namespace ELMS.WEB.Background.Interfaces
{
    public interface IEquipmentWorker
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}
