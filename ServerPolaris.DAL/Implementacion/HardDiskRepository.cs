using Microsoft.Data.SqlClient;
using ServerPolaris.DAL.DBContext;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;


namespace ServerPolaris.DAL.Implementacion
{
    public class HardDiskRepository: IHardDiskRepository
    {
        
        private readonly PolarisServerStringContext _contexto;

        public HardDiskRepository(PolarisServerStringContext context)
        {
            this._contexto = context;

        }
        public async Task<List<HardDisk>> GetInfoHardDisk()
        {
            List<HardDisk> List = new List<HardDisk>();

            try
            {
                using (var connection = new SqlConnection(_contexto.Conexion))
                {
                    connection.Open();

                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = "exec sp_executesql @stmt=N'/*-------------------------------------------------------------------\r\n[SCRIPT]   \r\n[DATABASE] \r\n[DESCRIPTION] \r\n[MAJ PAR] DATAFLY - Arian Papillon\r\n[DATEMAJ]  \r\n-------------------------------------------------------------------*/\r\nDECLARE\r\n   @version varchar(max);\r\nSELECT\r\n       @version = @@version;\r\nIF SUBSTRING(@version,PATINDEX(''% on %'',@version)+4,5) <> ''Linux''\r\nBEGIN\r\n-- Volume information\r\nDECLARE @ole sql_variant\r\nDECLARE @adv sql_variant\r\nSELECT   @ole = value_in_use\r\nFROM     sys.configurations\r\nWHERE    name LIKE ''Ole%''\r\nSELECT   @adv = value_in_use\r\nFROM     sys.configurations\r\nWHERE    name LIKE ''show advanced%''\r\n\r\nIF @ole = 0 \r\n   BEGIN\r\n      IF @adv = 0 \r\n         BEGIN\r\n            EXEC sp_configure ''show advanced'', 1\r\n            RECONFIGURE WITH OVERRIDE\r\n         END\r\n      EXEC sp_configure ''Ole Automation'', 1\r\n      RECONFIGURE WITH OVERRIDE\r\n   END \r\n\r\n\r\nSET NOCOUNT ON\r\n\r\nDECLARE @hr int \r\nDECLARE @fso int \r\nDECLARE @drive char(1) \r\nDECLARE @odrive int \r\nDECLARE @TotalSize varchar(20) \r\nDECLARE @MB bigint ; SET @MB = 1048576 \r\n\r\nCREATE TABLE #drives (drive char(1) PRIMARY KEY, \r\nFreeSpace int NULL, \r\nTotalSize int NULL) \r\n\r\nINSERT #drives(drive,FreeSpace) \r\nEXEC master.dbo.xp_fixeddrives \r\n\r\nEXEC @hr=sp_OACreate ''Scripting.FileSystemObject'',@fso OUT \r\nIF @hr <> 0 EXEC sp_OAGetErrorInfo @fso \r\n\r\nDECLARE dcur CURSOR LOCAL FAST_FORWARD \r\nFOR SELECT drive from #drives \r\nORDER by drive \r\n\r\nOPEN dcur \r\n\r\nFETCH NEXT FROM dcur INTO @drive \r\n\r\nWHILE @@FETCH_STATUS=0 \r\nBEGIN \r\n\r\nEXEC @hr = sp_OAMethod @fso,''GetDrive'', @odrive OUT, @drive \r\nIF @hr <> 0 EXEC sp_OAGetErrorInfo @fso \r\n\r\nEXEC @hr = sp_OAGetProperty @odrive,''TotalSize'', @TotalSize OUT \r\nIF @hr <> 0 EXEC sp_OAGetErrorInfo @odrive \r\n\r\nUPDATE #drives \r\nSET TotalSize=@TotalSize/@MB \r\nWHERE drive=@drive \r\n\r\nFETCH NEXT FROM dcur INTO @drive \r\n\r\nEND \r\n\r\nCLOSE dcur \r\nDEALLOCATE dcur \r\n\r\nEXEC @hr=sp_OADestroy @fso \r\nIF @hr <> 0 EXEC sp_OAGetErrorInfo @fso \r\n\r\nSELECT #drives.drive, \r\nFreeSpace as ''FreeMB'', \r\nTotalSize as ''TotalMB'', \r\nCAST((FreeSpace/(TotalSize*1.0))*100.0 as int) as ''Free_pct'' \r\n,DbSizeMo\r\n,DbGrowthMo\r\n,FreeSpace - DbGrowthMo AS LeftAfterGrowthMo\r\nFROM #drives \r\nLEFT JOIN \r\n(SELECT  LEFT(physical_name, 1) AS drive\r\n\t , SUM(cast(size as bigint)) * 8 / 1024 AS DbSizeMo\r\n\t , SUM(CASE WHEN is_percent_growth = 1 THEN (size*growth/100)*8/1024 \r\n\t WHEN is_percent_growth = 0 THEN growth *8/1024 END) AS DbGrowthMo\r\n\t FROM\t   sys.master_files\r\nGROUP BY LEFT(physical_name, 1)) AS DbFiles\r\nON #drives.drive = DbFiles.drive\r\nORDER BY #drives.drive \r\n\r\nDROP TABLE #drives \r\n\r\nIF @ole = 0 \r\n   BEGIN\r\n      EXEC sp_configure ''Ole Automation'', 0\r\n      RECONFIGURE WITH OVERRIDE\r\n\t IF @adv = 0 \r\n   BEGIN  \r\n      EXEC sp_configure ''show advanced'', 0\r\n      RECONFIGURE WITH OVERRIDE\r\n   END   \r\n   END\r\nEND\r\nELSE\r\nBEGIN\r\n   DECLARE @fixeddrives TABLE (drive VARCHAR(30), FreeMB int)\r\n   INSERT @fixeddrives EXEC xp_fixeddrives\r\n   SELECT drive, NULL AS FreeMB, NULL AS TotalMB, NULL AS Free_pct, NULL AS DbSizeMo, NULL AS DBGrowthMo, NULL AS LeftAfterGrowthMo FROM @fixeddrives\r\nEND;',@params=N''";

                        using (SqlDataReader read = await command.ExecuteReaderAsync())
                        {
                            while (await read.ReadAsync())
                            {
                                List.Add(new HardDisk
                                {
                                    Drive = read.GetString(0),
                                    FreeMB = read.GetInt32(1),
                                    TotalMB = read.GetInt32(2),
                                    Free_pct = read.GetInt32(3),
                                    DbSizeMo = read.GetInt64(4),
                                    DBGrowthMo = read.GetInt32(5),
                                    LeftAfterGrowthMo = read.GetInt32(6)
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

