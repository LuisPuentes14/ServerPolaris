using ServerPolaris.Entity;

namespace ServerPolaris.DAL.Interfaces
{
    public  interface IHardDiskRepository
    {
        Task<List<HardDisk>> GetInfoHardDisk();
    }
}
