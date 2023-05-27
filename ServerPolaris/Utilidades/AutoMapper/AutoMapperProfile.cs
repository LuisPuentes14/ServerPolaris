
using AutoMapper;
using ServerPolaris.Entity;
using ServerPolaris.Models.ViewModels;
using System.Globalization;

namespace SistemaVenta.AplicacionWeb.Utilidades.AutoMapper
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {

            #region Tipo log
            CreateMap<VMTipoLog, TipoLog>().ReverseMap();
            #endregion

            #region Log
            //Objeto Destino / Objeto origen
            CreateMap<Log, VMLog>();
               
            //Objeto Destino / Objeto origen
            CreateMap<VMLog, Log>().ReverseMap()
                .ForMember(destino =>
                   destino.ClienteName,
                   opt => opt.MapFrom(origen => origen.Cliente.ClienteName)
               )
               .ForMember(destino =>
                   destino.TipoLogDescripcion,
                   opt => opt.MapFrom(origen => origen.LogIdTipoLogNavigation.TipoLogDescripcion)
               );

            #endregion


            #region Cliente
            CreateMap<Cliente, VMCliente>().ReverseMap();
            CreateMap<VMCliente, Cliente>().ReverseMap();
            #endregion

            #region Server
            CreateMap<Server, VMServer>().ReverseMap();
            #endregion


            #region Ram
            CreateMap<Ram, VMRam>().ReverseMap();
            #endregion

            #region Hard Disk
            CreateMap<HardDisk, VMHardDisk>().ReverseMap();
            #endregion

            #region CPU
            CreateMap<CPU, VMCPU>().ReverseMap();
            #endregion

           
        }
    }

}
