using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.Entity
{
    public class UsuarioPerfils
    {
        public long UsuId { get; set; }

        public string? UsuNombre { get; set; }

        public string? UsuEmail { get; set; }

        public string? UsuLogin { get; set; }

        public string? UsuPassword { get; set; } = "";
        public List<Perfil> Perfils { get; set; } = new List<Perfil>();

        public long? EstadoId { get; set; }

        public bool isUpdatePassword { get; set; } = false;


    }
}
