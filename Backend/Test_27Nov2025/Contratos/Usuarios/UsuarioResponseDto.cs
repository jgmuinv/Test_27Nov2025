using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Usuarios
{
    public class UsuarioResponseDto
    {
        public int id { get; private set; }
        public string nombres { get; private set; }
        public string apellidos { get; private set; }
        public DateTime fechanacimiento { get; private set; }
        public string telefono { get; private set; }
        public string email { get; private set; }
        public string direccion { get; private set; }
        
        // Constructor que mapea desde UsuarioDto
        public UsuarioResponseDto(UsuarioDto dto)
        {
            id = dto.Id;
            nombres = dto.Nombres;
            apellidos = dto.Apellidos;
            fechanacimiento = dto.FechaNacimiento;
            telefono = dto.Telefono;
            email = dto.Email;
            direccion = dto.Direccion;
        }
    }
}
