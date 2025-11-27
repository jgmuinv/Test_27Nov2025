using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string Nombres { get; private set; }
        public string Apellidos { get; private set; }
        public DateTime FechaNacimiento { get; private set; }
        public string Email { get; private set; }
        public string Telefono { get; private set; }
        public string Estado { get; private set; }  // A/I
    }
}
