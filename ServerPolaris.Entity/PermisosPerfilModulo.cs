using System;
using System.Collections.Generic;

namespace ServerPolaris.Entity;

public partial class PermisosPerfilModulo
{
    public long PerId { get; set; }

    public long? PerfilId { get; set; }

    public long? ModId { get; set; }

    public bool? PerAcceder { get; set; }

    public bool? PerInsertar { get; set; }

    public bool? PerActualizar { get; set; }

    public bool? PerEliminar { get; set; }

    public virtual ModulosWeb? Mod { get; set; }

    public virtual Perfil? Perfil { get; set; }
}
