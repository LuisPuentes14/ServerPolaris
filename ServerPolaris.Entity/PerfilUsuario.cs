using System;
using System.Collections.Generic;

namespace ServerPolaris.Entity;

public partial class PerfilUsuario
{
    public long PerfilUsuarioId { get; set; }

    public long? PerfilId { get; set; }

    public long? UsuId { get; set; }

    public virtual Perfil? Perfil { get; set; }

    public virtual Usuario? Usu { get; set; }
}
