using Moq;
using OneTimePasswordBusinessLogic;
using OneTimePasswordBusinessLogic.Interfaces;
using OneTimePasswordBusinessLogic.Models;

namespace OneTimePasswordBusinessLogic_Tests
{
    [TestClass]
    public class ApplicationTests
    {
        private Mock<IOneTimePasswordGenerator> generatorMock;
        private Mock<IOneTimePasswordRepository> repositoryMock;
        private Mock<IOneTimePasswordCommunicator> communicatorMock;
        private Mock<IOneTimePasswordConfiguration> configurationMock;
        private OneTimePasswordApplication oneTimePasswordApplication;

        [TestInitialize]
        public void Initialize()
        {
            generatorMock = new Mock<IOneTimePasswordGenerator>();
            repositoryMock = new Mock<IOneTimePasswordRepository>();
            communicatorMock = new Mock<IOneTimePasswordCommunicator>();
            configurationMock = new Mock<IOneTimePasswordConfiguration>();

            oneTimePasswordApplication = new OneTimePasswordApplication(configurationMock.Object, generatorMock.Object, repositoryMock.Object, communicatorMock.Object);
        }

        [TestMethod]
        public async Task VerifyThatAllMethodsAreCalledWhenAPasswordIsGeneratedForAUser()
        {
            var oneTimePassword = new OneTimePassword
            {
                UserId = Guid.NewGuid().ToString(),
                ExpirationDate = DateTime.UtcNow,
                Value = "test"
            };

            generatorMock.Setup(x => x.GenerateForUser(oneTimePassword.UserId, It.IsAny<DateTime>()))
                .Returns(Task.FromResult(oneTimePassword));

            await oneTimePasswordApplication.CreateOneTimePasswordForUser(oneTimePassword.UserId);

            generatorMock.Verify(mock => mock.GenerateForUser(oneTimePassword.UserId, It.IsAny<DateTime>()), Times.Once());
            repositoryMock.Verify(mock => mock.Save(It.Is<OneTimePassword>(x => x.UserId == oneTimePassword.UserId &&
                                                    string.Equals(x.Value, oneTimePassword.Value))),
                                                   Times.Once());
            communicatorMock.Verify(mock => mock.Send(It.IsAny<OneTimePassword>()), Times.Once());
        }

        [TestMethod]
        public async Task VerifyThatOneTimePasswordIsNotValidForOtherUsers()
        {
            var oneTimePassword = new OneTimePassword
            {
                UserId = Guid.NewGuid().ToString(),
                Value = "1234",
                ExpirationDate = DateTime.UtcNow.AddSeconds(30)
            };

            repositoryMock.Setup(x => x.GetValidPasswordForUserId(oneTimePassword.UserId))
                .Returns(Task.FromResult(oneTimePassword));

            var validPasswordForUserId = await oneTimePasswordApplication.GetPasswordWithExpirationForUser(oneTimePassword.UserId);

            Assert.IsNotNull(validPasswordForUserId);
            Assert.IsTrue(await oneTimePasswordApplication.IsOneTimePasswordValidForUser(oneTimePassword.UserId, validPasswordForUserId.Value));
            Assert.IsFalse(await oneTimePasswordApplication.IsOneTimePasswordValidForUser($"{oneTimePassword.UserId}1", validPasswordForUserId.Value));
        }

        [TestMethod]
        public async Task VerifyThatIfPasswordIsExpiredPasswordIsNotConsideredToBeValid()
        {
            var oneTimePassword = new OneTimePassword
            {
                UserId = Guid.NewGuid().ToString(),
                Value = "1234",
                ExpirationDate = DateTime.UtcNow.AddSeconds(-30)
            };

            repositoryMock.Setup(x => x.GetValidPasswordForUserId(oneTimePassword.UserId))
                .Returns(Task.FromResult(oneTimePassword));

            var validPasswordForUserId = await oneTimePasswordApplication.GetPasswordWithExpirationForUser(oneTimePassword.UserId);

            Assert.IsNotNull(validPasswordForUserId);
            Assert.IsFalse(await oneTimePasswordApplication.IsOneTimePasswordValidForUser(oneTimePassword.UserId, validPasswordForUserId.Value));
        }
    }
}