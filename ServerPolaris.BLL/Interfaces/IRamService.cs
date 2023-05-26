using ServerPolaris.Entity;


namespace ServerPolaris.BLL.Interfaces
{
    public interface IRamService
    {
        Task<List<Ram>> GetInfoRam();
    }
}
