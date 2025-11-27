using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Usuarios
{
    public  class UsuarioDto
    {
        public UsuarioDto(int uId, string uNombres, string uApellidos, DateTime uFechaNacimiento, string uDireccion, string uTelefono, string uEmail, string uEstado, DateTime uFechaCreacion, DateTime? uFechaModificacion)
        {
            Id = uId;
            Nombres = uNombres;
            Apellidos = uApellidos;
            FechaNacimiento = uFechaNacimiento;
            Email = uEmail;
            Telefono = uTelefono;
            Estado = uEstado;
            Direccion = uDireccion;
            Password = "";
        }

        public int Id { get; private set; }
        public string Nombres { get; private set; }
        public string Apellidos { get; private set; }
        public bool session_active { get; set; }
        public DateTime FechaNacimiento { get; private set; }
        public string Email { get; private set; }
        public string Telefono { get; private set; }
        public string Password { get; set; }
        public string Direccion { get; set; }
        public string Estado { get; private set; }  // A/I
    }
}
