
using AutoMapper;
using ServerPolaris.Entity;
using ServerPolaris.AplicacionWeb.Models.ViewModels;

namespace SistemaVenta.AplicacionWeb.Utilidades.AutoMapper
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            #region Server
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
