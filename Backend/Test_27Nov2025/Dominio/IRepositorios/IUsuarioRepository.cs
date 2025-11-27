using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contratos.General;

namespace Dominio.IRepositorios
{
    public interface IUsuarioRepository
    {
        // CRUD básico
        Task<ResultadoDto<Usuario?>> GetUser(int id, CancellationToken ct = default);
        Task<ResultadoDto<IReadOnlyList<Usuario?>>> GetUsers(CancellationToken ct = default);
        Task<ResultadoDto<Usuario?>> CreateUser(Usuario obj, CancellationToken ct = default);
        Task<ResultadoDto<Usuario?>> UpdateUser(Usuario obj, CancellationToken ct = default);
        Task<ResultadoDto<Usuario?>> DeleteUser(int id, CancellationToken ct = default);
        Task<ResultadoDto<Usuario?> > GetByTelefono(string telefono, CancellationToken ct = default);

    }
}
