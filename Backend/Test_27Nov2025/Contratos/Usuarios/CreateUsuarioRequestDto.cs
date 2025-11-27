    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Usuarios
{
    public class CreateUsuarioRequestDto
    {
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public DateTime fechanacimiento { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string direccion { get; set; }
    }
}
