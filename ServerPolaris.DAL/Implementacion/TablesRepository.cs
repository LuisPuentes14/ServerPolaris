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
    public class TablesRepository: ITablesRepository
    {
        public async Task<List<Tables>> GetInfoTables(string conexion)
        {
            List<Tables> ListTables = new List<Tables>();

            try
            {
                using (var connection = new SqlConnection(conexion))
                {
                    connection.Open();

                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = "exec sp_executesql @stmt=N'SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;\r\nSELECT  [Schema]\r\n       ,TableName as TableName      \r\n       ,rows\r\n       ,[Size (MB)]\r\n      \r\nFROM    ( SELECT --t.TABLE_CATALOG as ''DATABASE''\r\n                    t.TABLE_SCHEMA AS ''Schema''\r\n                   ,t.TABLE_NAME AS TableName\r\n                   ,OBJECT_ID(''['' + t.TABLE_SCHEMA + ''].['' + t.TABLE_NAME\r\n                              + '']'') AS Id\r\n                   ,SUM(CASE WHEN i.index_id < 2 AND a.type = 1 THEN p.rows ELSE 0 END) AS rows\r\n                   ,CAST (CAST (SUM(CAST (a.total_pages AS BIGINT)) AS DECIMAL(18,\r\n                                                              2)) * 8192\r\n                    / 1024 / 1024 AS DECIMAL(18, 2)) AS ''Size (MB)''\r\n                   ,OBJECTPROPERTY(OBJECT_ID(''['' + t.TABLE_SCHEMA + ''].[''\r\n                                             + t.TABLE_NAME + '']''),\r\n                                   ''TableHasPrimaryKey'') AS HasPK     --- pas pour les vues\r\n                   ,( SELECT    OBJECTPROPERTY(object_id, ''CnstIsClustKey'')\r\n                      FROM      sys.objects\r\n                      WHERE     parent_object_id = OBJECT_ID(''[''\r\n                                                             + t.TABLE_SCHEMA\r\n                                                             + ''].[''\r\n                                                             + t.TABLE_NAME\r\n                                                             + '']'')\r\n                                AND type = ''PK''\r\n                    ) AS HasClustPK\r\n\t\t\t\t\t,ISNULL(OBJECTPROPERTY(OBJECT_ID(''['' + t.TABLE_SCHEMA + ''].[''\r\n                                             + t.TABLE_NAME + '']''), ''IsView''), \r\n                           0) AS IsIdxView\r\n\t\t\t\t\t,ISNULL(OBJECTPROPERTY(OBJECT_ID(''['' + t.TABLE_SCHEMA + ''].[''\r\n                                             + t.TABLE_NAME + '']''), ''TableIsMemoryOptimized''),\r\n                           0) AS IsMemoryOptimized\r\n\t\t\t\t\t, COUNT(DISTINCT p.partition_number) AS PartitionsNb   \r\n                   , CASE WHEN OBJECTPROPERTY(OBJECT_ID(''['' + t.TABLE_SCHEMA + ''].[''\r\n                                             + t.TABLE_NAME + '']''), ''IsView'') =1 THEN 1 ELSE OBJECTPROPERTY(OBJECT_ID(''['' + t.TABLE_SCHEMA + ''].[''\r\n                                             + t.TABLE_NAME + '']''),\r\n                                   ''TableHasClustIndex'') END AS HasClustIndex\r\n                   ,( SELECT    COUNT(*)\r\n                      FROM      sys.indexes\r\n                      WHERE     object_id = OBJECT_ID(''['' + t.TABLE_SCHEMA\r\n                                                      + ''].['' + t.TABLE_NAME\r\n                                                      + '']'')\r\n                                AND index_id > 1\r\n                                AND type = 2\r\n                    ) AS NbNonClustIndexes\r\n                   ,( SELECT    COUNT(*)\r\n                      FROM      sys.stats\r\n                      WHERE     object_id = OBJECT_ID(''['' + t.TABLE_SCHEMA\r\n                                                      + ''].['' + t.TABLE_NAME\r\n                                                      + '']'')\r\n                    ) AS NbStats\r\n                   ,OBJECTPROPERTY(OBJECT_ID(''['' + t.TABLE_SCHEMA + ''].[''\r\n                                             + t.TABLE_NAME + '']''),\r\n                                   ''TableHasActiveFulltextIndex'') AS HasFullText\r\n                   ,OBJECTPROPERTY(OBJECT_ID(''['' + t.TABLE_SCHEMA + ''].[''\r\n                                             + t.TABLE_NAME + '']''),\r\n                                   ''TableHasRowGuidCol'') AS HasRowGuidCol\r\n                   ,OBJECTPROPERTY(OBJECT_ID(''['' + t.TABLE_SCHEMA + ''].[''\r\n                                             + t.TABLE_NAME + '']''),\r\n                                   ''TableHasTextImage'') AS HasTextImage\r\n                   ,OBJECTPROPERTY(OBJECT_ID(''['' + t.TABLE_SCHEMA + ''].[''\r\n                                             + t.TABLE_NAME + '']''),\r\n                                   ''TableHasTimeStamp'') AS HasTimeStamp\r\n                   ,OBJECTPROPERTY(OBJECT_ID(''['' + t.TABLE_SCHEMA + ''].[''\r\n                                             + t.TABLE_NAME + '']''),\r\n                                   ''TableHasIdentity'') AS HasIdentity\r\n          FROM      INFORMATION_SCHEMA.TABLES t\r\n                    JOIN sys.indexes i ON OBJECT_ID(''['' + t.TABLE_SCHEMA\r\n                                                    + ''].['' + t.TABLE_NAME\r\n                                                    + '']'') = i.object_id\r\n                    JOIN sys.partitions p ON i.object_id = p.object_id\r\n                                             AND i.index_id = p.index_id\r\n                    JOIN sys.allocation_units a ON p.partition_id = a.container_id\r\n--                    JOIN sys.data_spaces s ON a.data_space_id = s.data_space_id\r\n          WHERE     (t.TABLE_TYPE = ''BASE TABLE'' OR  t.TABLE_TYPE = ''VIEW'')\r\n                    AND OBJECTPROPERTY(OBJECT_ID(''['' + t.TABLE_SCHEMA + ''].[''\r\n                                                 + t.TABLE_NAME + '']''),\r\n                                       ''IsMSShipped'') = 0\r\n                    AND t.TABLE_NAME <> ''sysdiagrams'' -- seulement tables utilisateur\r\n                    AND t.TABLE_NAME <> ''dtproperties''\r\n          GROUP BY  OBJECT_ID(''['' + t.TABLE_SCHEMA + ''].'' + t.TABLE_NAME)\r\n                   ,t.TABLE_CATALOG\r\n                   ,t.TABLE_SCHEMA\r\n                   ,t.TABLE_NAME\r\n--                   ,p.rows\r\n        ) AS TablesProperties\r\nORDER BY [Schema]\r\n       ,rows DESC\r\n       ,TableName ASC;',@params=N''";

                        using (SqlDataReader read = await command.ExecuteReaderAsync())
                        {
                            while (await read.ReadAsync())
                            {
                                ListTables.Add(new Tables
                                {
                                    Schema = read.GetString(0),
                                    NameTable = read.GetString(1),
                                    Rows = read.GetInt64(2),
                                    SizeMB = read.GetDecimal(3)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return ListTables;
        }




    }
}
