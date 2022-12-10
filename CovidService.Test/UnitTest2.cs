
using CovidService.Contracts;
using CovidService.Controllers;
using CovidService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CovidService.Test
{
    [TestFixture]
    public class UserServicesControllerTests
    {
        private UserServicesController _userServicesController;
        private Mock<IUserServicesContract> _userServicesContractMock;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _userServicesContractMock = new Mock<IUserServicesContract>();
            _userServicesController = new UserServicesController(_userServicesContractMock.Object);
        }

        [Test]
        public async Task Login_ReturnsOkResult_WhenCredentialsAreValid()
        {
            // Arrange
            var signInModel = new SignInModel
            {
                Email = "test@email.com",
                Password = "testpassword"
            };
            _userServicesContractMock
                .Setup(x => x.LoginAsync(signInModel))
                .ReturnsAsync("token");

            // Act
            var result = await _userServicesController.Login(signInModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Login_ReturnsUnauthorizedResult_WhenCredentialsAreInvalid()
        {
            // Arrange
            var signInModel = new SignInModel
            {
                Email = "test@email.com",
                Password = "invalidpassword"
            };
            _userServicesContractMock
                .Setup(x => x.LoginAsync(signInModel))
                .ReturnsAsync(string.Empty);

            // Act
            var result = await _userServicesController.Login(signInModel);

            // Assert
            Assert.IsInstanceOf<UnauthorizedResult>(result);
        }

        [Test]
        public async Task SignUp_ReturnsOkResult_WhenSignUpIsSuccessful()
        {
            // Arrange
            var signUpModel = new SignUpModel
            {
                Email = "test@email.com",
                Password = "testpassword"
            };
            var identityResult = IdentityResult.Success;
            _userServicesContractMock
                .Setup(x => x.SignUpAsync(signUpModel))
                .ReturnsAsync(identityResult);

            // Act
            var result = await _userServicesController.SignUp(signUpModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task SignUp_ReturnsUnauthorizedResult_WhenSignUpFails()
        {
            // Arrange
            var signUpModel = new SignUpModel
            {
                Email = "test@email.com",
                Password = "testpassword"
            };
            var identityResult = IdentityResult.Failed();
            _userServicesContractMock
                .Setup(x => x.SignUpAsync(signUpModel))
                .ReturnsAsync(identityResult);

            // Act
            var result = await _userServicesController.SignUp(signUpModel);

            // Assert
            Assert.IsInstanceOf<UnauthorizedResult>(result);
        }
    }

}
