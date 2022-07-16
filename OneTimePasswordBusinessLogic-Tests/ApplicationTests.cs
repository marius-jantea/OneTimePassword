using Moq;
using OneTimePasswordBusinessLogic;
using OneTimePasswordBusinessLogic.Interfaces;
using OneTimePasswordBusinessLogic.Models;

namespace OneTimePasswordBusinessLogic_Tests
{
    [TestClass]
    public class ApplicationTests
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var generatorMock = new Mock<IOneTimePasswordGenerator>();
            var repositoryMock = new Mock<IOneTimePasswordRepository>();
            var communicatorMock = new Mock<IOneTimePasswordCommunicator>();

            var userId = Guid.NewGuid();
            var oneTimePasswordGenerated = new OneTimePassword
            {
                UserId = userId,
                ExpirationDate = DateTime.UtcNow,
                Value = "test"
            };

            generatorMock.Setup(x => x.GenerateForUser(oneTimePasswordGenerated.UserId, It.IsAny<DateTime>()))
                .Returns(Task.FromResult(oneTimePasswordGenerated));


            var oneTimePasswordApplication = new OneTimePasswordApplication(generatorMock.Object, repositoryMock.Object, communicatorMock.Object);

            await oneTimePasswordApplication.CreateOneTimePasswordForUser(oneTimePasswordGenerated.UserId);

            generatorMock.Verify(mock => mock.GenerateForUser(oneTimePasswordGenerated.UserId, It.IsAny<DateTime>()), Times.Once());
            repositoryMock.Verify(mock => mock.Save(It.Is<OneTimePassword>(x => x.UserId == userId &&
                                                    string.Equals(x.Value, oneTimePasswordGenerated.Value))), 
                                                   Times.Once());
            communicatorMock.Verify(mock => mock.Send(It.IsAny<OneTimePassword>()), Times.Once());
        }
    }
}