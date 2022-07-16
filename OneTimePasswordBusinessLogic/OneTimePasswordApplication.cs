using OneTimePasswordBusinessLogic.Interfaces;
using OneTimePasswordBusinessLogic.Models;

namespace OneTimePasswordBusinessLogic
{
    public class OneTimePasswordApplication
    {
        private readonly IOneTimePasswordGenerator oneTimePasswordGenerator;
        private readonly IOneTimePasswordRepository oneTimePasswordRepository;
        private readonly IOneTimePasswordCommunicator oneTimePasswordCommunicator;

        public OneTimePasswordApplication(IOneTimePasswordGenerator oneTimePasswordGenerator, IOneTimePasswordRepository oneTimePasswordRepository, IOneTimePasswordCommunicator oneTimePasswordCommunicator)
        {
            this.oneTimePasswordGenerator = oneTimePasswordGenerator;
            this.oneTimePasswordRepository = oneTimePasswordRepository;
            this.oneTimePasswordCommunicator = oneTimePasswordCommunicator;
        }

        public async Task CreateOneTimePasswordForUser(string userId)
        {
            var expirationDate = GetExpirationDate();
            var newOneTimePassword = await oneTimePasswordGenerator.GenerateForUser(userId, expirationDate);
            await oneTimePasswordRepository.Save(newOneTimePassword);
            await oneTimePasswordCommunicator.Send(newOneTimePassword);
        }

        public async Task<bool> IsOneTimePasswordValidForUser(string userId, string value)
        {
            var oneTimePasswordForUser = await oneTimePasswordRepository.GetValidPasswordForUserId(userId);
            return oneTimePasswordForUser != null && string.Equals(oneTimePasswordForUser.Value, value);
        }

        public async Task<OneTimePassword?> GetValidPasswordForUser(string userId)
        {
            return await oneTimePasswordRepository.GetValidPasswordForUserId(userId);
        }

        private DateTime GetExpirationDate()
        {
            return DateTime.UtcNow.AddSeconds(30);
        }
    }
}