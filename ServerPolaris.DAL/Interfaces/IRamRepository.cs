
using ServerPolaris.Entity;

namespace ServerPolaris.DAL.Interfaces
{
    public interface IRamRepository
    {
        Task<List<Ram>> GetInfoRam();
    }
}
