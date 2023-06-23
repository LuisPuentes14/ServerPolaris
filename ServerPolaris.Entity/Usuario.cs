namespace ServerPolaris.Entity;

public partial class Usuario
{
    public long UsuId { get; set; }

    public string? UsuNombre { get; set; }

    public string? UsuEmail { get; set; }

    public string? UsuLogin { get; set; }

    public string? UsuPassword { get; set; }

    public long? EstadoId { get; set; }

    public DateTime? UsuFechaExpPassword { get; set; }

    public DateTime? UsuFechaCreacion { get; set; }

    public DateTime? UsuFechaActualizacion { get; set; }

    public virtual TipoEstadoUsuario? Estado { get; set; }

    public virtual ICollection<PerfilUsuario> PerfilUsuarios { get; set; } = new List<PerfilUsuario>();
    
}
