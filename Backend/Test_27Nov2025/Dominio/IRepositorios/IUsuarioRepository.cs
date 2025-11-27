using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.IRepositorios
{
    public interface IUsuarioRepository
    {
        // CRUD básico

        Task<Usuario?> GetUser(int id, CancellationToken ct = default);
        Task<IReadOnlyList<Usuario>> GetUsers(int? id, string? nombre, CancellationToken ct = default);
        Task CreateUser(Usuario obj, CancellationToken ct = default);
        Task UpdateUser(Usuario obj, CancellationToken ct = default);
        Task DeleteUser(int id, CancellationToken ct = default);
    }
}
