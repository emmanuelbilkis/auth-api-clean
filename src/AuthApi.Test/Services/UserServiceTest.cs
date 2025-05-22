using AuthApi.Interfaces.IRepository;
using AuthApi.Interfaces.IService;
using AuthApi.Models.Db;
using AuthApi.Services.User;
using Moq;
using NUnit.Framework;

namespace AuthApi.Test.Services
{
    [TestFixture]
    public class UserServiceTest
    {
        private Mock<IUserRepository> _mockRepository;
        private Mock<ITokenService> _mockTokenService;
        private Mock<IEmailService> _mockEmailService;
        //private Mock<ILogger<UserService>> _mockLogger;
        //private Mock<AppDbContext> _mockContext; // Solo si lo usás en tus tests

        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IUserRepository>();
            _mockTokenService = new Mock<ITokenService>();
            _mockEmailService = new Mock<IEmailService>();  

            _userService = new UserService(
                _mockRepository.Object,
                _mockTokenService.Object,
                _mockEmailService.Object
                //,_mockLogger.Object,
               // _mockContext.Object
            );
        }

        [Test]
        public async Task GetAll_WhenRepositoryReturnsUsers_ReturnsSuccessResult()
        {
            // Arrange
            var mockUsers = new List<UserModel>
        {
            new UserModel { Email = "user1@example.com" },
            new UserModel { Email = "user2@example.com" }
        };
            _mockRepository.Setup(r => r.GetAll()).ReturnsAsync(mockUsers);

            // Act
            var result = await _userService.GetAll();

            // Assert
            //Assert.That(actualObject, constraint, message, params object[] args);
            
            Assert.Multiple(() =>
            {  // assert -- afirmar - yo quiero q afirmes q lo q tengo y esero conincida sino no pasa el test y mensaje opcional 
                Assert.That(result.IsSuccessful, Is.True);
                Assert.That(result.Value.Count, Is.EqualTo(2), "No esta cumpliendo con la cantidad esperada {2}");
            }); // si un aseert falla , continua con los demas con multiple. 
            

            // a continuacion probe q no se detiene en el primer error q falla con el Multiple
            /*
            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Count, Is.EqualTo(3), "No esta cumpliendo con la cantidad esperada(2).");
                Assert.That(result.IsSuccessful, Is.False);
            });*/
        }


    }
}
