using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Usuarios
{
    public class UpdateUsuarioRequestDto
    {
        public string nombres { get; private set; }
        public string apellidos { get; private set; }
        public DateTime fechanacimiento { get; private set; }
        public string telefono { get; private set; }
        public string email { get; private set; }
        public string password { get; set; }
        public string dirección { get; private set; }
    }
}
