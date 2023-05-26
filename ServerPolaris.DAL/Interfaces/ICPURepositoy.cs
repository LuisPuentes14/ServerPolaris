using ServerPolaris.Entity;

namespace ServerPolaris.DAL.Interfaces
{
    public interface ICPURepositoy
    {
        Task<List<CPU>> GetInfoCPU();
    }
}
