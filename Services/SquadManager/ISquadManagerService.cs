using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TF47_Backend.Services.SquadManager
{
    public interface ISquadManagerService
    {
        Task<bool> WriteSquadXml(long squadId, CancellationToken cancellationToken);
        Task<bool> UpdateSquadImage(long squadId, Stream image, CancellationToken cancellationToken);
        Task<bool> DeleteSquad(long squadId, CancellationToken cancellationToken);
    }
}