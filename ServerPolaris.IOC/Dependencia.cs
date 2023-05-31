

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.BLL.Implementacion;
using ServerPolaris.DAL.DBContext;
using ServerPolaris.DAL.Interfaces;
using SistemaVenta.DAL.Implementacion;
using ServerPolaris.DAL.Implementacion;

namespace ServerPolaris.IOC
{
    public static class Dependencia
    {

        public static void InyectarDependencia(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<PolarisServerContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("CadenaSQL"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            // polaris Service
            services.AddScoped<ICPUService, CPUService>();
            services.AddScoped<ICPURepositoy, CPURepository>();
            services.AddScoped<IHardDiskRepository, HardDiskRepository>();
            services.AddScoped<IHardDiskService, HardDiskService>();
            services.AddScoped<IRamRepository, RamRepository>();
            services.AddScoped<IRamService, RamService>();
            services.AddScoped<IServerRepository, ServerRepository>();
            services.AddScoped<IServerService, ServerService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<ILogClienteService, LogClienteService>();
            services.AddScoped<ITipoLogService, TipoLogService>();
            services.AddScoped<IDataBaseService, DataBaseService>();
            services.AddScoped<IIndexRepository, IndexRepository>(); 
            services.AddScoped<IIndexService, IndexService>(); 
            services.AddScoped<IFileDataBaseRepository, FilesDataBaseRepository>();
            services.AddScoped<IFilesDataBaseService, FilesDataBaseService>();
        }
    }
}
