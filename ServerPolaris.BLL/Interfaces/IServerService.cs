using ServerPolaris.Entity;


namespace ServerPolaris.BLL.Interfaces
{
    public interface IServerService
    {
        Task<List<Server>> GetInfoSever();
    }
}
