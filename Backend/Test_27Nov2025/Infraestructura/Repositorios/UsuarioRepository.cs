using System.Data;
using Contratos.General;
using Dapper;
using Dominio.Entidades;
using Dominio.IRepositorios;
using Infraestructura.Data;

namespace Infraestructura.Repositorios;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UsuarioRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<ResultadoDto<Usuario?>> GetUser(int id, CancellationToken ct = default)
    {
        const string sql = """
            select
                id,
                nombres,
                apellidos,
                fechanacimiento,
                direccion,
                password,
                telefono,
                email,
                estado,
                fechacreacion,
                fechamodificacion
            from usuarios
            where id = @id
            """;

        using var conn = _connectionFactory.CreateConnection();
        var resp = await conn.QuerySingleOrDefaultAsync<Usuario>(new CommandDefinition(sql, new { id }, cancellationToken: ct));
        return ResultadoDto<Usuario?>.Success(resp);
    }

    public async Task<ResultadoDto<IReadOnlyList<Usuario?>>> GetUsers(CancellationToken ct = default)
    {
        const string sql = """
            select
                id,
                nombres,
                apellidos,
                fechanacimiento,
                direccion,
                password,
                telefono,
                email,
                estado,
                fechacreacion,
                fechamodificacion
            from usuarios
            order by id
            """;

        using var conn = _connectionFactory.CreateConnection();
        var result = await conn.QueryAsync<Usuario>(new CommandDefinition(sql, cancellationToken: ct));
       var resp = result.AsList();
        return ResultadoDto<IReadOnlyList<Usuario?>>.Success(resp);
    }

    public async Task<ResultadoDto<Usuario?>> CreateUser(Usuario obj, CancellationToken ct = default)
    {
        
        
        const string sql = """
            insert into usuarios
                (nombres, apellidos, fechanacimiento, direccion, password, telefono, email, estado)
            values
                (@Nombres, @Apellidos, @FechaNacimiento, @Direccion, @Password, @Telefono, @Email, @Estado)
            returning id;
            """;

        using var conn = _connectionFactory.CreateConnection();
        var id = await conn.ExecuteScalarAsync<int>(new CommandDefinition(sql, obj, cancellationToken: ct));
        
        var entidad = await GetUser(id, ct);
        if (entidad.Exitoso == false) return ResultadoDto<Usuario?>.Failure("Error al obtener el usuario.");
        if (entidad.Datos is null) return ResultadoDto<Usuario?>.Failure("No existe el usuario con el id especificado.");
        
        return ResultadoDto<Usuario?>.Success(entidad.Datos);
    }

    public async Task<ResultadoDto<Usuario?>> UpdateUser(Usuario obj, CancellationToken ct = default)
    {
        var entidad = await GetUser(obj.Id, ct);
        if (entidad.Exitoso == false) return ResultadoDto<Usuario?>.Failure("Error al obtener el usuario.");
        if (entidad.Datos is null) return ResultadoDto<Usuario?>.Failure("No existe el usuario con el id especificado.");
        
        const string sql = """
            update usuarios
            set
                nombres = @Nombres,
                apellidos = @Apellidos,
                fechanacimiento = @FechaNacimiento,
                direccion = @Direccion,
                telefono = @Telefono,
                email = @Email,
                estado = @Estado
            where id = @Id;
            """;

        using var conn = _connectionFactory.CreateConnection();
        await conn.ExecuteAsync(new CommandDefinition(sql, obj, cancellationToken: ct));
        
        return ResultadoDto<Usuario?>.Success(entidad.Datos);
    }

    public async Task<ResultadoDto<Usuario?>> DeleteUser(int id, CancellationToken ct = default)
    {
        if (id <= 0) return ResultadoDto<Usuario?>.Failure("El id debe ser mayor a cero.");
            
        var entidad = await GetUser(id, ct);
        if (entidad.Exitoso == false) return ResultadoDto<Usuario?>.Failure("Error al obtener el usuario.");
        if (entidad.Datos is null) return ResultadoDto<Usuario?>.Failure("No existe el usuario con el id especificado.");
        
        
        const string sql = """
                           update usuarios
                           set
                               estado = 'I'
                           where id = @Id;
                           """;
        
        using var conn = _connectionFactory.CreateConnection();
        await conn.ExecuteAsync(new CommandDefinition(sql, new { id }, cancellationToken: ct));
        
        return ResultadoDto<Usuario?>.Success(entidad.Datos);
    }
    
    public async Task<ResultadoDto<Usuario?> > GetByTelefono(string telefono, CancellationToken ct = default)
    {
        const string sql = """
                           select
                               id,
                               nombres,
                               apellidos,
                               fechanacimiento,
                               direccion,
                               password,
                               telefono,
                               email,
                               estado,
                               fechacreacion,
                               fechamodificacion
                           from usuarios
                           where telefono = @telefono
                           """;

        using var conn = _connectionFactory.CreateConnection();
        var resp = await conn.QuerySingleOrDefaultAsync<Usuario>(new CommandDefinition(sql, new { telefono }, cancellationToken: ct));
        return ResultadoDto<Usuario?>.Success(resp);
    }
}