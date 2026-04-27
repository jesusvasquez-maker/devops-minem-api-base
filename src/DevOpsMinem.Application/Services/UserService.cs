using DevOpsMinem.Application.DTOs;
using DevOpsMinem.Domain.Entities;
using DevOpsMinem.Domain.Ports;

namespace DevOpsMinem.Application.Services;

public class UserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UserDto>> ObtenerTodosAsync()
    {
        var users = await _repository.ObtenerTodosAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserDto?> ObtenerPorIdAsync(Guid id)
    {
        var user = await _repository.ObtenerPorIdAsync(id);
        return user is null ? null : MapToDto(user);
    }

    public async Task<UserDto> CrearAsync(CrearUserDto dto)
    {
        var existente = await _repository.ObtenerPorEmailAsync(dto.Email);
        if (existente is not null)
            throw new InvalidOperationException($"Ya existe un usuario con el email {dto.Email}.");

        var user = User.Crear(dto.Nombre, dto.Apellido, dto.Email, dto.Rol);
        await _repository.AgregarAsync(user);
        return MapToDto(user);
    }

    public async Task<UserDto?> ActualizarAsync(Guid id, ActualizarUserDto dto)
    {
        var user = await _repository.ObtenerPorIdAsync(id);
        if (user is null) return null;

        user.Actualizar(dto.Nombre, dto.Apellido, dto.Rol);
        await _repository.ActualizarAsync(user);
        return MapToDto(user);
    }

    public async Task<bool> EliminarAsync(Guid id)
    {
        var user = await _repository.ObtenerPorIdAsync(id);
        if (user is null) return false;

        await _repository.EliminarAsync(id);
        return true;
    }

    private static UserDto MapToDto(User u) =>
        new(u.Id, u.Nombre, u.Apellido, u.Email, u.Rol, u.FechaRegistro, u.Activo);
}
