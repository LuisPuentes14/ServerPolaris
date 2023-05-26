using Microsoft.Data.SqlClient;
using ServerPolaris.DAL.DBContext;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;


namespace ServerPolaris.DAL.Implementacion
{
    public class CPURepository : ICPURepositoy
    {
        private readonly PolarisServerStringContext _contexto;       
        
        public CPURepository(PolarisServerStringContext context)
        {
            this._contexto = context;

        }
        public async Task<List<CPU>> GetInfoCPU()
        {
            List<CPU> ListCpu = new List<CPU>();
           
            try
            {
                using (var connection = new SqlConnection(_contexto.Conexion))
                {
                    connection.Open();

                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = "exec sp_executesql @stmt=N'DECLARE @sqlcmd VARCHAR(MAX)\r\nIF EXISTS (SELECT * WHERE CONVERT(varchar(128), SERVERPROPERTY(''ProductVersion'')) >=  ''10.50'' and left(cast(serverproperty(''productversion'') as varchar), 1)<''7'')\r\nBEGIN\r\nSET @sqlcmd = ''\r\nDECLARE @ts_now bigint = (SELECT cpu_ticks/(cpu_ticks/ms_ticks)FROM sys.dm_os_sys_info); \r\n\r\nSELECT TOP(256) SQLProcessUtilization AS [SQL Server Process CPU Utilization], \r\n               SystemIdle AS [System Idle Process], \r\n               100 - SystemIdle - SQLProcessUtilization AS [Other Process CPU Utilization], \r\n               DATEADD(ms, -1 * (@ts_now - [timestamp]), GETDATE()) AS [Event Time] \r\nFROM ( \r\n\t  SELECT record.value(''''(./Record/@id)[1]'''', ''''int'''') AS record_id, \r\n\t\t\trecord.value(''''(./Record/SchedulerMonitorEvent/SystemHealth/SystemIdle)[1]'''', ''''int'''') \r\n\t\t\tAS [SystemIdle], \r\n\t\t\trecord.value(''''(./Record/SchedulerMonitorEvent/SystemHealth/ProcessUtilization)[1]'''', \r\n\t\t\t''''int'''') \r\n\t\t\tAS [SQLProcessUtilization], [timestamp] \r\n\t  FROM ( \r\n\t\t\tSELECT [timestamp], CONVERT(xml, record) AS [record] \r\n\t\t\tFROM sys.dm_os_ring_buffers WITH (NOLOCK)\r\n\t\t\tWHERE ring_buffer_type = N''''RING_BUFFER_SCHEDULER_MONITOR'''' \r\n\t\t\tAND record LIKE N''''%<SystemHealth>%'''') AS x \r\n\t  ) AS y \r\nORDER BY record_id DESC OPTION (RECOMPILE);''\r\nEXEC (@sqlcmd)\r\nEND',@params=N''";

                        using (SqlDataReader read = await command.ExecuteReaderAsync())
                        {
                            while (await read.ReadAsync())
                            {
                                ListCpu.Add(new CPU
                                {
                                    SqlServerCPUUtilization = read.GetInt32(0),
                                    LdleProcess = read.GetInt32(1),
                                    OtherCPUUtilization = read.GetInt32(2),
                                    DateTime = read.GetDateTime(3)
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
