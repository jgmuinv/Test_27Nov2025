using Contratos.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Login
{
    public sealed record LoginResponse(
        UsuarioDto user,
        string? access_token,
        string? token_type
    );
}
