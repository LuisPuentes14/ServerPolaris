using System;
using System.Collections.Generic;

namespace ServerPolaris.Entity;

public partial class ModulosWeb
{
    public long ModId { get; set; }

    public long? ModIdPadre { get; set; }

    public long? ModIdHijo { get; set; }

    public string? ModNombre { get; set; }

    public string? ModUrl { get; set; }

    public long? IdTipoModulo { get; set; }

    public virtual TipoModulo? IdTipoModuloNavigation { get; set; }

    public virtual ICollection<PermisosPerfilModulo> PermisosPerfilModulos { get; set; } = new List<PermisosPerfilModulo>();
}
