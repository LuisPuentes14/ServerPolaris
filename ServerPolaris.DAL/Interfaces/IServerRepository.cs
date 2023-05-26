
using ServerPolaris.Entity;


namespace ServerPolaris.DAL.Interfaces
{
    public interface IServerRepository
    {
        Task<List<Server>> GetInfoSever();
    }
}
