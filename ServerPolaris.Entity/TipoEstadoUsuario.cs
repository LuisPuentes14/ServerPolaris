using System;
using System.Collections.Generic;

namespace ServerPolaris.Entity;

public partial class TipoEstadoUsuario
{
    public long EstadoId { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
