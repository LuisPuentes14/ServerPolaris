using ServerPolaris.BLL.Interfaces;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;


namespace ServerPolaris.BLL.Implementacion
{
    public class CPUService : ICPUService
    {
        private readonly ICPURepositoy _repositorio;
        public CPUService(ICPURepositoy _repositorio) { 
            this._repositorio = _repositorio;
        }

        public async Task<List<CPU>> GetInfoCPU()
        {
            List<CPU> listCPU = await _repositorio.GetInfoCPU();
            return listCPU;
        }



    }
}
