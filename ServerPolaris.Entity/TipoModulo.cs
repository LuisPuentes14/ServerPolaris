using System;
using System.Collections.Generic;

namespace ServerPolaris.Entity;

public partial class TipoModulo
{
    
    public long IdTipoModulo { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<ModulosWeb> ModulosWebs { get; set; } = new List<ModulosWeb>();
}
