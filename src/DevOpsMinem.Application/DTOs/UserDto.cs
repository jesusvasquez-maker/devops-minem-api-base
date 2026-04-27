namespace DevOpsMinem.Application.DTOs;

public record UserDto(
    Guid Id,
    string Nombre,
    string Apellido,
    string Email,
    string Rol,
    DateTime FechaRegistro,
    bool Activo
);

public record CrearUserDto(
    string Nombre,
    string Apellido,
    string Email,
    string Rol
);

public record ActualizarUserDto(
    string Nombre,
    string Apellido,
    string Rol
);
