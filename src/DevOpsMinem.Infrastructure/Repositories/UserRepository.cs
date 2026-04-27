using DevOpsMinem.Domain.Entities;
using DevOpsMinem.Domain.Ports;
using DevOpsMinem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DevOpsMinem.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> ObtenerTodosAsync() =>
        await _context.Users.AsNoTracking().ToListAsync();

    public async Task<User?> ObtenerPorIdAsync(Guid id) =>
        await _context.Users.FindAsync(id);

    public async Task<User?> ObtenerPorEmailAsync(string email) =>
        await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email.ToLower());

    public async Task AgregarAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is not null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
