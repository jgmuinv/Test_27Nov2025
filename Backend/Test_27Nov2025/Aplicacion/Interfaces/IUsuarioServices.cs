using Contratos.General;
using Contratos.Login;
using Contratos.Usuarios;

namespace Aplicacion.Interfaces;

public interface IUsuarioService
{
    Task<ResultadoDto<UsuarioDto?>> GetUserAsync(int id, CancellationToken ct = default);
    Task<ResultadoDto<IReadOnlyList<UsuarioResponseDto?>>> GetUsersAsync(CancellationToken ct = default);
    Task<ResultadoDto<UsuarioDto?> > CreateUserAsync(CreateUsuarioRequestDto request, CancellationToken ct = default);
    Task<ResultadoDto<UsuarioDto?> > UpdateUserAsync(int id, UpdateUsuarioRequestDto request, CancellationToken ct = default);
    Task<ResultadoDto<UsuarioResponseDto?>> DeleteUserAsync(int id, CancellationToken ct = default);
    Task<ResultadoDto<LoginResponse?>> LoginAsync(LoginRequest request, CancellationToken ct = default);

}