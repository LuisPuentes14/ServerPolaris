using ServerPolaris.BLL.Interfaces;
using ServerPolaris.Entity;
using ServerPolaris.DAL.Interfaces;


namespace ServerPolaris.BLL.Implementacion
{
    public class ServerService: IServerService
    {

        private readonly IServerRepository _repositorio;
        public ServerService(IServerRepository _repositorio)
        {
            this._repositorio = _repositorio;
        }

        public async Task<List<Server>> GetInfoSever()
        {
            List<Server> list = await _repositorio.GetInfoSever();
            return list;
        }


    }
}
