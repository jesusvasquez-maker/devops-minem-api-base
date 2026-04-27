using DevOpsMinem.Domain.Entities;
using FluentAssertions;

namespace DevOpsMinem.Tests.Domain;

public class UserTests
{
    [Fact]
    public void Crear_ConDatosValidos_DebeRetornarUserActivo()
    {
        var user = User.Crear("Ana", "García", "ana@minem.gob.pe", "Administrador");

        user.Nombre.Should().Be("Ana");
        user.Apellido.Should().Be("García");
        user.Email.Should().Be("ana@minem.gob.pe");
        user.Rol.Should().Be("Administrador");
        user.Activo.Should().BeTrue();
        user.Id.Should().NotBeEmpty();
        user.FechaRegistro.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Theory]
    [InlineData("", "García", "ana@minem.gob.pe", "Rol")]
    [InlineData("Ana", "", "ana@minem.gob.pe", "Rol")]
    [InlineData("Ana", "García", "email-invalido", "Rol")]
    public void Crear_ConDatosInvalidos_DebeLanzarArgumentException(
        string nombre, string apellido, string email, string rol)
    {
        var act = () => User.Crear(nombre, apellido, email, rol);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Actualizar_ConDatosValidos_DebeModificarCampos()
    {
        var user = User.Crear("Ana", "García", "ana@minem.gob.pe", "Usuario");
        user.Actualizar("María", "López", "Administrador");

        user.Nombre.Should().Be("María");
        user.Apellido.Should().Be("López");
        user.Rol.Should().Be("Administrador");
    }

    [Fact]
    public void Desactivar_DebeSetearActivoEnFalse()
    {
        var user = User.Crear("Ana", "García", "ana@minem.gob.pe", "Usuario");
        user.Desactivar();
        user.Activo.Should().BeFalse();
    }

    [Fact]
    public void Crear_SinRol_DebeAsignarRolPorDefecto()
    {
        var user = User.Crear("Ana", "García", "ana@minem.gob.pe", "");
        user.Rol.Should().Be("Usuario");
    }
}
