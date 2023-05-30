using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.Models.ViewModels;

namespace ServerPolaris.Controllers
{
    public class DataBaseClienteViewController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _DataBaseService;
        private readonly IIndexService _IndexService;



        public DataBaseClienteViewController(IMapper mapper, IDataBaseService DataBaseService)
        {
            _mapper = mapper;
            _DataBaseService = DataBaseService;
        }

     
        public IActionResult ViewDataBase(int idDb)
        {
          
            return View();
        }




    }
}
