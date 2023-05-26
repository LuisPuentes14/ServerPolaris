using ServerPolaris.BLL.Interfaces;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;


namespace ServerPolaris.BLL.Implementacion
{
    public class RamService : IRamService
    {
        private readonly IRamRepository _repositorio;
        public RamService(IRamRepository _repositorio)
        {
            this._repositorio = _repositorio;
        }

        public async Task<List<Ram>> GetInfoRam()
        {
            List<Ram> list = await _repositorio.GetInfoRam();
            return list;
        }
    }
}
