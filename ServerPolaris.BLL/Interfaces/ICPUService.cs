using ServerPolaris.Entity;


namespace ServerPolaris.BLL.Interfaces
{
    public interface ICPUService
    {
        Task<List<CPU>> GetInfoCPU();
    }
}
