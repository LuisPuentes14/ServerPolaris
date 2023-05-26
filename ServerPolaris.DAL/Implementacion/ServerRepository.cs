using Microsoft.Data.SqlClient;
using ServerPolaris.DAL.DBContext;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;


namespace ServerPolaris.DAL.Implementacion
{
    public class ServerRepository : IServerRepository
    {

        private readonly PolarisServerStringContext _contexto;

        public ServerRepository(PolarisServerStringContext context)
        {
            this._contexto = context;

        }
        public async Task<List<Server>> GetInfoSever()
        {
            List<Server> List = new List<Server>();

            try
            {
                using (var connection = new SqlConnection(_contexto.Conexion))
                {
                    connection.Open();

                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = "exec sp_executesql @stmt=N'\r\nSELECT SERVERPROPERTY(''MachineName'') AS [MachineName], \r\nSERVERPROPERTY(''Edition'') AS [Edition], \r\nSERVERPROPERTY(''ProductVersion'') AS [ProductVersion],\r\nSERVERPROPERTY(''Collation'') AS [Collation], \r\n@@VERSION AS VERSION;',@params=N''";

                        using (SqlDataReader read = await command.ExecuteReaderAsync())
                        {
                            while (await read.ReadAsync())
                            {
                                List.Add(new Server
                                {
                                    MachineName = read.GetString(0),
                                    Edition = read.GetString(1),
                                    ProductVersion = read.GetString(2),
                                    Collation = read.GetString(3),
                                    Version = read.GetString(4)                                   
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return List;

        }
    }
}
