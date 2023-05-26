
using ServerPolaris.Entity;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.DAL.Interfaces;



namespace ServerPolaris.BLL.Implementacion
{
    public class ClienteService : IClienteService
    {
        private readonly IGenericRepository<Cliente> _repositorio;       
     

        public ClienteService(IGenericRepository<Cliente> repositorio)
        {
            _repositorio = repositorio;          
        }

        public async Task<List<Cliente>> Lista()
        {
            IQueryable<Cliente> query = await _repositorio.Consultar();
            return query.ToList();
        }

        public async Task<Cliente> Crear(Cliente entidad)
        {
            Cliente producto_existe = await _repositorio.Obtener(c => c.ClienteName == entidad.ClienteName);

            if(producto_existe != null)
            throw new TaskCanceledException("El cliente ya existe.");

            try
            {               

                Cliente cliente_creado = await _repositorio.Crear(entidad);

                if (cliente_creado.ClienteId == 0) 
                    throw new TaskCanceledException("No se pudo crear el cliente");

                IQueryable<Cliente> query = await _repositorio.Consultar(c => c.ClienteId == cliente_creado.ClienteId);

                cliente_creado = query.First();

                return cliente_creado;
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Cliente> Editar(Cliente entidad)
        {         

            try
            {
                IQueryable<Cliente> queryCliente = await _repositorio.Consultar(c => c.ClienteId == entidad.ClienteId);

                Cliente cliente_para_editar = queryCliente.First();

                cliente_para_editar.ClienteName= entidad.ClienteName;           

                bool respuesta = await _repositorio.Editar(cliente_para_editar);

                if(!respuesta)
                    throw new TaskCanceledException("No se pudo editar el producto.");

                Cliente producto_editado = queryCliente.First();

                return producto_editado;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(int idCliente)
        {
            try
            {
                Cliente cliente_encontrado = await _repositorio.Obtener(c => c.ClienteId == idCliente);

                if(cliente_encontrado == null)
                    throw new TaskCanceledException("El producto no existe.");               

                bool respuesta = await _repositorio.Eliminar(cliente_encontrado);               

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        
    }
}
