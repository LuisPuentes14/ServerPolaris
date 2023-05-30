using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ServerPolaris.DAL.DBContext;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ServerPolaris.DAL.Implementacion
{
    public class IndexRepository: IIndexRepository
    {
        //private readonly PolarisServerStringContext _contexto;

        //public IndexRepository(PolarisServerStringContext context)
        //{
        //    this._contexto = context;

        //}
        public async Task<List<ServerPolaris.Entity.Index>> GetInfoIndex(String conexion)
        {
            List<ServerPolaris.Entity.Index> ListCpu = new List<ServerPolaris.Entity.Index>();

           
            try
            {
                using (var connection = new SqlConnection(conexion))
                {
                    connection.Open();

                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = "exec sp_executesql @stmt=N'/*-------------------------------------------------------------------\r\n[SCRIPT] Index manquants  \r\n[DATABASE] Base courante ou toutes les databases \r\n[DESCRIPTION] Liste des index manquants\r\n[MAJ PAR] DATAFLY - Arian Papillon\r\n[DATEMAJ] 20200201\r\n-------------------------------------------------------------------*/\r\n\r\nSET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;\r\nIF OBJECT_ID (''tempdb..#MissingIndex'') IS NOT NULL\r\n    BEGIN\r\n        DROP TABLE\r\n             #MissingIndex\r\n    END;\r\nCREATE TABLE #MissingIndex  (\r\n\t[DBName] [nvarchar](128) NULL,\r\n\t[index_advantage] decimal(18,2) NULL,\r\n\t[last_user_seek] [datetime] NULL,\r\n\t[last_user_scan] [datetime] NULL,\r\n\t[nbrows] [bigint] NULL,\r\n\t[Object_id] INT NULL,\r\n\t[Database.Schema.Table] [nvarchar](4000) NULL,\r\n\t[equality_columns] [nvarchar](4000) NULL,\r\n\t[inequality_columns] [nvarchar](4000) NULL,\r\n\t[included_columns] [nvarchar](4000) NULL,\r\n\t[NbIndexCols] [int] NULL,\r\n\t[NbIncludedCol] [int] NULL,\r\n\t[unique_compiles] [bigint] NULL,\r\n\t[user_seeks] [bigint] NULL,\r\n\t[user_scans] [bigint] NULL,\r\n\t[avg_total_user_cost] [float] NULL,\r\n\t[avg_user_impact] [float] NULL,\r\n\t[CreateCmd] [nvarchar](4000) NULL\r\n) ON [PRIMARY]\r\n\r\nINSERT #MissingIndex\r\n(\r\n    DBName,\r\n    index_advantage,\r\n    last_user_seek,\r\n    last_user_scan,\r\n    nbrows,\r\n\t[Object_id],\r\n    [Database.Schema.Table],\r\n    equality_columns,\r\n    inequality_columns,\r\n    included_columns,\r\n    NbIndexCols,\r\n    NbIncludedCol,\r\n    unique_compiles,\r\n    user_seeks,\r\n    user_scans,\r\n    avg_total_user_cost,\r\n    avg_user_impact,\r\n    CreateCmd\r\n)\r\nSELECT TOP (10)  DB_NAME(mid.database_id) AS DBName\r\n       , user_seeks * avg_total_user_cost * ( avg_user_impact * 0.01 ) AS [index_advantage]\r\n       , migs.last_user_seek\r\n       , migs.last_user_scan\r\n\t   , NULL AS nbrows\r\n\t   , mid.object_id\r\n       , mid.[statement] AS [Database.Schema.Table]\r\n       , mid.equality_columns\r\n       , mid.inequality_columns\r\n       , mid.included_columns\r\n       -- nombre de colonnes index et include\r\n       , COALESCE(\r\n             ( LEN(mid.equality_columns)\r\n               - LEN(REPLACE(mid.equality_columns, '','', '''')) + 1 )\r\n           , 0)\r\n         + COALESCE(\r\n               ( LEN(mid.inequality_columns)\r\n                 - LEN(REPLACE(mid.inequality_columns, '','', '''')) + 1 )\r\n             , 0) AS NbIndexCols\r\n       , COALESCE(\r\n             ( LEN(mid.included_columns)\r\n               - LEN(REPLACE(mid.included_columns, '','', '''')) + 1 )\r\n           , 0) AS NbIncludedCol\r\n       -- statistiques\r\n       , migs.unique_compiles\r\n       , migs.user_seeks\r\n       , migs.user_scans\r\n       , migs.avg_total_user_cost\r\n       , migs.avg_user_impact\r\n       -- commande de création d''index basée sur equality, inequality, include\r\n       -- pour information et analyse seulement !\r\n       , ''CREATE INDEX [ix_''\r\n         + LEFT(REPLACE(\r\n                    REPLACE(\r\n                        SUBSTRING(\r\n                            mid.statement\r\n                          , CHARINDEX(\r\n                                ''.''\r\n                              , mid.statement\r\n                              , CHARINDEX(''.'', mid.statement) + 1) + 1\r\n                          , LEN(mid.statement))\r\n                      , ''[''\r\n                      , '''')\r\n                  , '']''\r\n                  , '''') + ''_'' + CONVERT(VARCHAR(10), GETDATE(), 112) + ''_''\r\n                + LEFT(REPLACE(CAST(NEWID() AS VARCHAR(36)), ''-'', ''_''),4), 120)\r\n         + ''] ON ''\r\n         + SUBSTRING(\r\n               mid.statement\r\n             , CHARINDEX(''.'', mid.statement) + 1\r\n             , CHARINDEX(\r\n                   ''.''\r\n                 , SUBSTRING(\r\n                       mid.statement\r\n                     , CHARINDEX(''.'', mid.statement) + 1\r\n                     , LEN(mid.statement))) - 1) + ''.''\r\n         + SUBSTRING(\r\n               mid.statement\r\n             , CHARINDEX(''.'', mid.statement, CHARINDEX(''.'', mid.statement) + 1)\r\n               + 1\r\n             , LEN(mid.statement)) + '' ('' + COALESCE(mid.equality_columns, '''')\r\n         + CASE WHEN mid.equality_columns IS NOT NULL\r\n                     AND mid.inequality_columns IS NOT NULL THEN '',''\r\n                ELSE ''''\r\n           END + COALESCE(mid.inequality_columns, '''') + '')''\r\n         + COALESCE('' INCLUDE ('' + mid.included_columns + '')'', '''') --+'' WITH (ONLINE=ON)''\r\n       AS CreateCmd\r\n\t --,substring(mid.statement,CHARINDEX(''.'',mid.statement)+1,CHARINDEX(''.'',substring(mid.statement,CHARINDEX(''.'',mid.statement)+1,LEN(mid.statement)))-1)\r\n\t --,substring(mid.statement,CHARINDEX(''.'',mid.statement,CHARINDEX(''.'',mid.statement)+1)+1,LEN(mid.statement))\r\nFROM     sys.dm_db_missing_index_group_stats AS migs WITH ( NOLOCK )\r\n         INNER JOIN sys.dm_db_missing_index_groups AS mig WITH ( NOLOCK ) ON migs.group_handle = mig.index_group_handle\r\n         INNER JOIN sys.dm_db_missing_index_details AS mid WITH ( NOLOCK ) ON mig.index_handle = mid.index_handle\r\nWHERE   mid.database_id = DB_ID() -- activer pour filtrer sur la base courante\r\nORDER BY index_advantage DESC\r\nOPTION ( RECOMPILE );\r\n\r\n--SELECT * FROM #MissingIndex\r\n\r\nDECLARE @DBName NVARCHAR(128),@objectid INT,@SQLCmd NVARCHAR(MAX)\r\nDECLARE TablesCursor CURSOR\r\n    FOR SELECT DISTINCT\r\n          DBName,\r\n\t\t  Object_id\r\n          FROM #MissingIndex;\r\nOPEN TablesCursor;\r\nFETCH NEXT FROM TablesCursor INTO @DBName, @objectid;\r\nWHILE @@FETCH_STATUS = 0\r\n    BEGIN\r\n\t    SET @SQLCmd = ''UPDATE #MissingIndex SET nbrows = (SELECT SUM(row_count) FROM [''+@DBName+''].sys.dm_db_partition_stats WHERE DBName = ''''''+@DBName + '''''' AND object_id = ''+CAST(@objectid AS varchar(15))+'') WHERE Object_id = ''+CAST(@objectid AS varchar(15))\r\n\t\tEXEC sp_executesql @SQLCmd\r\n\t\tFETCH NEXT FROM TablesCursor INTO @DBName, @objectid;\r\n\tEND\r\nCLOSE TablesCursor;\r\nDEALLOCATE TablesCursor;\r\n\r\nSELECT DBName,\r\n      -- index_advantage,\r\n      -- last_user_seek,\r\n      -- last_user_scan,\r\n      -- nbrows,\r\n      -- Object_id,\r\n       [Database.Schema.Table],\r\n       ISNULL(equality_columns,'''') AS equality_columns,\r\n        ISNULL(inequality_columns,'''') AS inequality_columns ,\r\n        ISNULL(included_columns,'''') AS included_columns,\r\n       --NbIndexCols,\r\n       --NbIncludedCol,\r\n       --unique_compiles,\r\n       --user_seeks,\r\n       --user_scans,\r\n       --avg_total_user_cost,\r\n       avg_user_impact,\r\n       CreateCmd \r\n\t   FROM #MissingIndex\r\n',@params=N''";

                        using (SqlDataReader read = await command.ExecuteReaderAsync())
                        {
                            while (await read.ReadAsync())
                            {
                                ListCpu.Add(new ServerPolaris.Entity.Index
                                {
                                    DBName = read.GetString(0),
                                    DatabaseShemaTable = read.GetString(1),
                                    EqualityColumns = read.GetString(2),
                                    inequalityColumns= read.GetString(3),
                                    IncludedColums = read.GetString(4),
                                    AvgUserImpact = read.GetDouble(5).ToString(),
                                    CreateCmd = read.GetString(6),
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
