using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Usuarios
{
    public  class UsuarioDto
    {
        public int Id { get; private set; }
        public string Nombres { get; private set; }
        public string Apellidos { get; private set; }
        public bool session_active { get; set; }
        public DateTime FechaNacimiento { get; private set; }
        public string Email { get; private set; }
        public string Telefono { get; private set; }
        public string Password { get; set; }
        public string address { get; set; }
        public string Estado { get; private set; }  // A/I
    }
}
