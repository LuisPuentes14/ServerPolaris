using Microsoft.Data.SqlClient;
using ServerPolaris.DAL.DBContext;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;


namespace ServerPolaris.DAL.Implementacion
{
    public class RamRepository : IRamRepository
    {
        private readonly PolarisServerStringContext _contexto;

        public RamRepository(PolarisServerStringContext context)
        {
            this._contexto = context;

        }
        public async Task<List<Ram>> GetInfoRam()
        {
            List<Ram> ListCpu = new List<Ram>();

            try
            {
                using (var connection = new SqlConnection(_contexto.Conexion))
                {
                    connection.Open();

                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = "exec sp_executesql @stmt=N'DECLARE @sqlcmd VARCHAR(MAX)\r\nIF EXISTS (SELECT * WHERE CONVERT(varchar(128), SERVERPROPERTY(''ProductVersion'')) LIKE ''9%'')\r\nBEGIN\r\nSET @sqlcmd = ''\r\nSELECT '''''''' AS [Physical Memory (MB)], \r\n       '''''''' AS [Available Memory (MB)], \r\n       '''''''' AS [Total Page File (MB)], \r\n\t   '''''''' AS [Available Page File (MB)], \r\n\t   '''''''' AS [System Cache (MB)],\r\n       '''''''' AS [System Memory State]\r\n;''\r\nEND\r\nELSE\r\nBEGIN\r\nSET @sqlcmd = ''\r\nSELECT total_physical_memory_kb/1024 AS [Physical Memory (MB)], \r\n       available_physical_memory_kb/1024 AS [Available Memory (MB)], \r\n       total_page_file_kb/1024 AS [Total Page File (MB)], \r\n\t   available_page_file_kb/1024 AS [Available Page File (MB)], \r\n\t   system_cache_kb/1024 AS [System Cache (MB)],\r\n       system_memory_state_desc AS [System Memory State]\r\nFROM sys.dm_os_sys_memory WITH (NOLOCK) OPTION (RECOMPILE);''\r\nEND\r\nEXEC (@sqlcmd) ',@params=N''";

                        using (SqlDataReader read = await command.ExecuteReaderAsync())
                        {
                            while (await read.ReadAsync())
                            {
                                ListCpu.Add(new Ram
                                {
                                    PhysicalMemoryMB = read.GetInt64(0),
                                    AvailableMemoryMB = read.GetInt64(1),
                                    ToatalPageFileMB = read.GetInt64(2),
                                    AvailablePageFileMB = read.GetInt64(3),
                                    SystemCacheMB = read.GetInt64(4),
                                    SysteMemoryState = read.GetString(5)
                                });
                            }
                        }
                    }
                }



            }
            catch (Exception ex)
            {
            }


            return ListCpu;

        }
    }
}
