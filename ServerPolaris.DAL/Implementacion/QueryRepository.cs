using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using ServerPolaris.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.DAL.Implementacion
{
    public class QueryRepository : IQueryRepository
    {
        public async Task<DataTable> query(string conexion, string comando)
        {           
            DataTable dt = new DataTable();

            try
            {
                using (var connection = new SqlConnection(conexion))
                {
                    connection.Open();

                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = comando;

                        using (SqlDataReader read = await command.ExecuteReaderAsync())
                        {
                            dt.Load(read);
                        }
                    }
                }

            }
            catch (Exception ex)
            {


            }


            return dt;
        }




    }
}
