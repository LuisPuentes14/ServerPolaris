using ServerPolaris.BLL.Interfaces;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;


namespace ServerPolaris.BLL.Implementacion
{
    public class CPUService : ICPUService
    {
        private readonly ICPURepositoy _repositorio;
        private readonly IMenuService _IMenuService;
        public CPUService(ICPURepositoy _repositorio, IMenuService IMenuService) { 
            this._repositorio = _repositorio;
            this._IMenuService = IMenuService;
        }

        public async Task<List<CPU>> GetInfoCPU()
        {

            List<Menu> list = await _IMenuService.Menu();

            List<CPU> listCPU = await _repositorio.GetInfoCPU();
            return listCPU;
        }



    }
}
