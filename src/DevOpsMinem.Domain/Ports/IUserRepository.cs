using DevOpsMinem.Domain.Entities;

namespace DevOpsMinem.Domain.Ports;

public interface IUserRepository
{
    Task<IEnumerable<User>> ObtenerTodosAsync();
    Task<User?> ObtenerPorIdAsync(Guid id);
    Task<User?> ObtenerPorEmailAsync(string email);
    Task AgregarAsync(User user);
    Task ActualizarAsync(User user);
    Task EliminarAsync(Guid id);
}
