using Microsoft.Data.SqlClient;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.DAL.Implementacion
{
    public class FilesDataBaseRepository : IFileDataBaseRepository
    {
        public async Task<List<FilesDataBase>> GetInfoFilesDataBase(string conexion)
        {
            List<FilesDataBase> ListFiles = new List<FilesDataBase>();

            try
            {
                using (var connection = new SqlConnection(conexion))
                {
                    connection.Open();

                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = "exec sp_executesql @stmt=N'-- Calculates average stalls per read, per write, and per total input/output for each database file  (Query 26) (IO Stalls by File)\r\nSELECT \r\n\tDB_NAME(fs.database_id) AS [Database Name], \r\n\tCONVERT(DECIMAL(18,2), mf.size/128.0) AS [File Size (MB)], \r\n\tmf.physical_name\r\nFROM sys.dm_io_virtual_file_stats(null,null) AS fs\r\nINNER JOIN sys.master_files AS mf WITH (NOLOCK)\r\nON fs.database_id = mf.database_id\r\nAND fs.[file_id] = mf.[file_id]\r\nWHERE fs.database_id = DB_ID() ',@params=N''";

                        using (SqlDataReader read = await command.ExecuteReaderAsync())
                        {
                            while (await read.ReadAsync())
                            {
                                ListFiles.Add(new FilesDataBase
                                {
                                    DataBaseName = read.GetString(0),
                                    FileSizeMB = read.GetDecimal(1).ToString(),
                                    PhysicalNeme = read.GetString(2)                                   
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return ListFiles;
        }



    }
}
