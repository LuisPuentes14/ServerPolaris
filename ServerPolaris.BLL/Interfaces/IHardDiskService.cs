using ServerPolaris.Entity;

namespace ServerPolaris.BLL.Interfaces
{
    public interface IHardDiskService
    {
        Task<List<HardDisk>> GetInfoHardDisk();
    }
}
