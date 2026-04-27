using DevOpsMinem.Application.DTOs;
using DevOpsMinem.Application.Services;
using DevOpsMinem.Domain.Entities;
using DevOpsMinem.Domain.Ports;
using FluentAssertions;
using Moq;

namespace DevOpsMinem.Tests.Application;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repoMock;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _repoMock = new Mock<IUserRepository>();
        _service = new UserService(_repoMock.Object);
    }

    [Fact]
    public async Task ObtenerTodosAsync_DebeRetornarListaDtos()
    {
        var users = new List<User>
        {
            User.Crear("Ana", "García", "ana@minem.gob.pe", "Admin"),
            User.Crear("Luis", "Pérez", "luis@minem.gob.pe", "Usuario")
        };
        _repoMock.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(users);

        var result = await _service.ObtenerTodosAsync();

        result.Should().HaveCount(2);
        result.Should().AllSatisfy(u => u.Should().BeOfType<UserDto>());
    }

    [Fact]
    public async Task CrearAsync_ConEmailDuplicado_DebeLanzarInvalidOperationException()
    {
        var existente = User.Crear("Otro", "Usuario", "ana@minem.gob.pe", "Usuario");
        _repoMock.Setup(r => r.ObtenerPorEmailAsync("ana@minem.gob.pe"))
                 .ReturnsAsync(existente);

        var dto = new CrearUserDto("Ana", "García", "ana@minem.gob.pe", "Admin");
        var act = async () => await _service.CrearAsync(dto);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*ana@minem.gob.pe*");
    }

    [Fact]
    public async Task CrearAsync_ConDatosValidos_DebeRetornarUserDto()
    {
        _repoMock.Setup(r => r.ObtenerPorEmailAsync(It.IsAny<string>()))
                 .ReturnsAsync((User?)null);
        _repoMock.Setup(r => r.AgregarAsync(It.IsAny<User>()))
                 .Returns(Task.CompletedTask);

        var dto = new CrearUserDto("Ana", "García", "ana@minem.gob.pe", "Admin");
        var result = await _service.CrearAsync(dto);

        result.Email.Should().Be("ana@minem.gob.pe");
        result.Nombre.Should().Be("Ana");
        result.Activo.Should().BeTrue();
        _repoMock.Verify(r => r.AgregarAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_ConIdInexistente_DebeRetornarNull()
    {
        _repoMock.Setup(r => r.ObtenerPorIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync((User?)null);

        var result = await _service.ObtenerPorIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task EliminarAsync_ConIdInexistente_DebeRetornarFalse()
    {
        _repoMock.Setup(r => r.ObtenerPorIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync((User?)null);

        var result = await _service.EliminarAsync(Guid.NewGuid());

        result.Should().BeFalse();
        _repoMock.Verify(r => r.EliminarAsync(It.IsAny<Guid>()), Times.Never);
    }
}
