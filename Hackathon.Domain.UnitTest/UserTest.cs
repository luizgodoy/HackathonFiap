using Hackathon.Core.DTO;
using Hackathon.Core.Models;
using Hackathon.Data.Interfaces;
using Hackathon.Domain.Interfaces;
using Hackathon.Domain.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Hackathon.Domain.UnitTest
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepository;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly SignInManager<User> _signInManager;
        private readonly Mock<RoleManager<IdentityRole<Guid>>> _mockRoleManager;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly UserServices _userService;

        public UserServiceTests()
        {
            // Mock IUserRepository
            _mockRepository = new Mock<IUserRepository>();

            _mockUserManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                null, null, null, null, null, null, null, null
            );
            
            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockUserClaimsPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            var mockIdentityOptions = new Mock<IOptions<IdentityOptions>>();
            var mockLogger = new Mock<ILogger<SignInManager<User>>>();
            var mockAuthenticationSchemeProvider = new Mock<IAuthenticationSchemeProvider>();

            _signInManager = new SignInManager<User>(
                _mockUserManager.Object,
                mockHttpContextAccessor.Object,
                mockUserClaimsPrincipalFactory.Object,
                mockIdentityOptions.Object,
                mockLogger.Object,
                mockAuthenticationSchemeProvider.Object
            );

            _mockRoleManager = new Mock<RoleManager<IdentityRole<Guid>>>(
                new Mock<IRoleStore<IdentityRole<Guid>>>().Object,
                null, null, null, null
            );

            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(config => config["Jwt:Key"]).Returns("qwer-asdf-zxcv-1234-!@#$-chave-super-secreta");

            _userService = new UserServices(_mockRepository.Object, _mockUserManager.Object, _signInManager, _mockRoleManager.Object, _mockConfiguration.Object);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnException_WhenNameIsEmpty()
        {
            // Arrange
            var user = new User
            {
                CPF = "12345678901",
                Email = "user@clinicaX.com",
                Password = "Secure123!",
                Role = Role.Patient
            };

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => _userService.Create(user));

            //Assert
            Assert.Equal("O nome do usuário é obrigatório", ex.Message);
        }

        [Fact]
        public async Task CreateDoctor_ShouldReturnException_WhenCRMIsEmpty()
        {
            // Arrange
            var doctor = new User
            {
                Name = "Dr. João",
                CPF = "12345678901",
                Email = "doctor@clinicaX.com",
                Password = "DrJ0901001",
                Role = Role.Doctor
            };

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => _userService.Create(doctor));

            //Assert
            Assert.Equal("O CRM é obrigatório para médicos", ex.Message);
        }

        [Fact]
        public async Task CreatePatient_ShouldReturnException_WhenCRMIsProvided()
        {
            // Arrange
            var patient = new User
            {
                Name = "Maria",
                CPF = "12345678901",
                Email = "patient@clinicaX.com",
                Password = "@!MAddsa123!",
                Role = Role.Patient,
                CRM = "123456" // Paciente não pode ter CRM
            };

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => _userService.Create(patient));

            //Assert
            Assert.Equal("Pacientes não devem possuir CRM", ex.Message);
        }

        [Fact]
        public async Task CreateDoctor_ShouldCreateSuccessfully()
        {
            // Arrange
            var doctor = new User
            {
                Name = "Dr. João",
                CPF = "12345678901",
                Email = "doctor@clinicaX.com",
                UserName = "doctor@clinicaX.com",
                NormalizedEmail = ("doctor@clinicaX.com").ToUpper(),
                Password = "MAddsa123AA!",
                Role = Role.Doctor,
                CRM = "123456"
            };

            // Act
            _mockRepository.Setup(repo => repo.Create(It.IsAny<User>())).Returns(Task.CompletedTask);
            await _userService.Create(doctor);

            //Assert
            _mockRepository.Verify(repo => repo.Create(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnException_WhenUserNotFound()
        {
            _mockRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync((User)null);
            var ex = await Assert.ThrowsAsync<Exception>(() => _userService.GetById(Guid.NewGuid()));
            Assert.Equal("Usuário não encontrado", ex.Message);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnException_WhenPasswordIsWeak()
        {
            // Arrange
            var user = new User
            {
                Name = "Teste",
                CPF = "12345678901",
                Email = "teste@clinicaX.com",
                Password = "12345", 
                Role = Role.Patient
            };

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => _userService.Create(user));
            
            //Assert
            Assert.Equal("A senha deve conter pelo menos uma letra, um número e um caractere especial, com no mínimo 6 caracteres", ex.Message);
        }
    }
}