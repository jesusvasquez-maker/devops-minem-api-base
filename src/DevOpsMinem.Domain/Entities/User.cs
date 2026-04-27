namespace DevOpsMinem.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Apellido { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Rol { get; private set; } = string.Empty;
    public DateTime FechaRegistro { get; private set; }
    public bool Activo { get; private set; }

    // Constructor para EF Core
    private User() { }

    public static User Crear(string nombre, string apellido, string email, string rol)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre es requerido.", nameof(nombre));
        if (string.IsNullOrWhiteSpace(apellido))
            throw new ArgumentException("El apellido es requerido.", nameof(apellido));
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("El email no es válido.", nameof(email));

        return new User
        {
            Id = Guid.NewGuid(),
            Nombre = nombre.Trim(),
            Apellido = apellido.Trim(),
            Email = email.Trim().ToLower(),
            Rol = string.IsNullOrWhiteSpace(rol) ? "Usuario" : rol.Trim(),
            FechaRegistro = DateTime.UtcNow,
            Activo = true
        };
    }

    public void Actualizar(string nombre, string apellido, string rol)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre es requerido.", nameof(nombre));
        if (string.IsNullOrWhiteSpace(apellido))
            throw new ArgumentException("El apellido es requerido.", nameof(apellido));

        Nombre = nombre.Trim();
        Apellido = apellido.Trim();
        Rol = string.IsNullOrWhiteSpace(rol) ? Rol : rol.Trim();
    }

    public void Desactivar() => Activo = false;
}
