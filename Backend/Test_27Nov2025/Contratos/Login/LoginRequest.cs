using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Login
{
    public class LoginRequest
    {
        [Required, StringLength(8)]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Password)]
        [StringLength(120, MinimumLength = 4, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; } // Se adiciona propiedad para al ser implementado en el Front se pueda enviar al usuario autenticado a la sección solicitada automáticamente.
    }
}
