namespace Dominio.Entidades;

public class Usuario
{
    public int Id { get; private set; }
    public string Nombres { get; private set; }
    public string Apellidos { get; private set; }
    public DateTime FechaNacimiento { get; private set; }
    public string Direccion { get; private set; }
    public string Password { get; private set; }
    public string Telefono { get; private set; }
    public string Email { get; private set; }
    public string Estado { get; private set; } // "A" o "I"
    public DateTime FechaCreacion { get; private set; }
    public DateTime? FechaModificacion { get; private set; }

    // Constructor para crear nuevos usuarios
    public Usuario(
        string nombres,
        string apellidos,
        DateTime fechaNacimiento,
        string direccion,
        string password,
        string telefono,
        string email,
        string estado = "A")
    {
        Nombres = nombres;
        Apellidos = apellidos;
        FechaNacimiento = fechaNacimiento;
        Direccion = direccion;
        Password = password;
        Telefono = telefono;
        Email = email;
        Estado = estado;
        FechaCreacion = DateTime.UtcNow;
    }

    // Constructor vacío para Dapper
    public Usuario()
    {
        Nombres = string.Empty;
        Apellidos = string.Empty;
        Direccion = string.Empty;
        Password = string.Empty;
        Telefono = string.Empty;
        Email = string.Empty;
        Estado = "A";
    }

    public void Activar() => Estado = "A";
    public void Inactivar() => Estado = "I";

    public void Actualizar(
        string nombres,
        string apellidos,
        DateTime fechaNacimiento,
        string direccion,
        string telefono,
        string email)
    {
        Nombres = nombres;
        Apellidos = apellidos;
        FechaNacimiento = fechaNacimiento;
        Direccion = direccion;
        Telefono = telefono;
        Email = email;
        FechaModificacion = DateTime.UtcNow;
    }

    public void CambiarPassword(string nuevoHash)
    {
        Password = nuevoHash;
        FechaModificacion = DateTime.UtcNow;
    }
}

