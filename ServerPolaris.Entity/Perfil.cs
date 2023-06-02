using System;
using System.Collections.Generic;

namespace ServerPolaris.Entity;

public partial class Perfil
{
    public long PerfilId { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<PerfilUsuario> PerfilUsuarios { get; set; } = new List<PerfilUsuario>();

    public virtual ICollection<PermisosPerfilModulo> PermisosPerfilModulos { get; set; } = new List<PermisosPerfilModulo>();
}
