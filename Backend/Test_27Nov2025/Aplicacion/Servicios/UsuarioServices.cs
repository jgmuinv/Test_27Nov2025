using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Aplicacion.Interfaces;
using Contratos.General;
using Contratos.Login;
using Contratos.Usuarios;
using Dominio.Entidades;
using Dominio.IRepositorios;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using System.Linq;

namespace Aplicacion.Servicios;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repo;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private readonly IConfiguration _config;

    public UsuarioService(IUsuarioRepository repo, IPasswordHasher<Usuario> passwordHasher, IConfiguration config)
    {
        _repo = repo;
        _passwordHasher = passwordHasher;
        _config = config;
    }

    public async Task<ResultadoDto<UsuarioDto?>> GetUserAsync(int id, CancellationToken ct = default)
    {
        var user = await _repo.GetUser(id, ct);
        var resp = user.Datos is null ? null : Map(user.Datos);
        return ResultadoDto<UsuarioDto?>.Success(resp);
    }

    public async Task<ResultadoDto<IReadOnlyList<UsuarioResponseDto?>>> GetUsersAsync(CancellationToken ct = default)
    {
        var users = await _repo.GetUsers(ct);
        var resp = users?.Datos?
            .Select(u => u is null ? null : new UsuarioDto(
                u.Id,
                u.Nombres,
                u.Apellidos,
                u.FechaNacimiento,
                u.Direccion,
                u.Telefono,
                u.Email,
                u.Estado,
                u.FechaCreacion,
                u.FechaModificacion))
            .Where(dto => dto is not null)
            .Select(dto => new UsuarioResponseDto(dto!))
            .ToList();
        return resp is null ? ResultadoDto<IReadOnlyList<UsuarioResponseDto?>>.Failure("No se encontraron usuarios.") : ResultadoDto<IReadOnlyList<UsuarioResponseDto?>>.Success(resp);
    }

    public async Task<ResultadoDto<UsuarioDto?>> CreateUserAsync(CreateUsuarioRequestDto request, CancellationToken ct = default)
    {
        var usuario = new Usuario(
            request.nombres,
            request.apellidos,
            request.fechanacimiento,
            request.direccion,
            password: string.Empty,
            request.telefono,
            request.email);

        var hash = _passwordHasher.HashPassword(usuario, request.password);
        usuario.CambiarPassword(hash);

        var creado = await _repo.CreateUser(usuario, ct);
       if (creado.Exitoso == false) return ResultadoDto<UsuarioDto?>.Failure(creado.Errores);
       if (creado.Datos is null) return ResultadoDto<UsuarioDto?>.Failure("No se pudo crear el usuario.");

        var resp = Map(creado.Datos);
        return ResultadoDto<UsuarioDto?>.Success(resp);
    }

    public async Task<ResultadoDto<UsuarioDto?> > UpdateUserAsync(int id, UpdateUsuarioRequestDto request, CancellationToken ct = default)
    {
        var usuario = await _repo.GetUser(id, ct);
        if (usuario.Datos is null) return ResultadoDto<UsuarioDto?>.Failure("No existe el usuario con el id especificado.");

        usuario.Datos.Actualizar(
            request.nombres,
            request.apellidos,
            request.fechanacimiento, 
            request.dirección,
            request.telefono,
            request.email);

        await _repo.UpdateUser(usuario.Datos, ct);
        var actualizado = await _repo.GetUser(id, ct) ?? usuario;
        if (actualizado.Datos is null) return ResultadoDto<UsuarioDto?>.Failure("No se pudo actualizar el usuario.");
        var resp = Map(actualizado.Datos);
        return ResultadoDto<UsuarioDto?>.Success(resp);
    }

    public async Task<ResultadoDto<UsuarioResponseDto?>> DeleteUserAsync(int id, CancellationToken ct = default)
    {
       var resp = await _repo.DeleteUser(id, ct);
       if (resp.Exitoso == false) return ResultadoDto<UsuarioResponseDto?>.Failure(resp.Errores);
       if (resp.Datos is null) return ResultadoDto<UsuarioResponseDto?>.Failure("No se pudo eliminar el usuario.");
      var salida = new UsuarioResponseDto(Map(resp.Datos));
       return ResultadoDto<UsuarioResponseDto?>.Success(salida); 
    }
    
    public async Task<ResultadoDto<LoginResponse?>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _repo.GetByTelefono(request.Telefono, ct);
        if (user.Datos is null)
            return ResultadoDto<LoginResponse?>.Failure("Usuario no encontrado.");

        var result = _passwordHasher.VerifyHashedPassword(
            user.Datos,
            user.Datos.Password,
            request.Password);

        if (result == PasswordVerificationResult.Failed)
            return ResultadoDto<LoginResponse?>.Failure("Credenciales inválidas.");

        var token = GenerateToken(user.Datos);
        var dto = new UsuarioDto(
            user.Datos.Id,
            user.Datos.Nombres,
            user.Datos.Apellidos,
            user.Datos.FechaNacimiento,
            user.Datos.Direccion,
            user.Datos.Telefono,
            user.Datos.Email,
            user.Datos.Estado,
            user.Datos.FechaCreacion,
            user.Datos.FechaModificacion);

        var resp = new LoginResponse(dto, token, "Bearer");
        return ResultadoDto<LoginResponse?>.Success(resp);
    }

    private string GenerateToken(Usuario user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:secret"] ?? throw new InvalidOperationException("Jwt:secret missing")));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Email),
            new("telefono", user.Telefono)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    

    private static UsuarioDto Map(Usuario u) =>
        new(
            u.Id,
            u.Nombres,
            u.Apellidos,
            u.FechaNacimiento,
            u.Direccion,
            u.Telefono,
            u.Email,
            u.Estado,
            u.FechaCreacion,
            u.FechaModificacion);
}