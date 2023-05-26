using ServerPolaris.BLL.Interfaces;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;


namespace ServerPolaris.BLL.Implementacion
{
    public class HardDiskService: IHardDiskService
    {
        private readonly IHardDiskRepository _repositorio;
        public HardDiskService(IHardDiskRepository _repositorio)
        {
            this._repositorio = _repositorio;
        }

        public async Task<List<HardDisk>> GetInfoHardDisk()
        {
            List<HardDisk> list = await _repositorio.GetInfoHardDisk();
            return list;
        }


    }
}
