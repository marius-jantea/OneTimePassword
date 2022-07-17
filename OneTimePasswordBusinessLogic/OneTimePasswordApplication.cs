using OneTimePasswordBusinessLogic.Interfaces;
using OneTimePasswordBusinessLogic.Models;

namespace OneTimePasswordBusinessLogic
{
    public class OneTimePasswordApplication
    {
        private readonly IOneTimePasswordConfiguration configuration;
        private readonly IOneTimePasswordGenerator oneTimePasswordGenerator;
        private readonly IOneTimePasswordRepository oneTimePasswordRepository;
        private readonly IOneTimePasswordCommunicator oneTimePasswordCommunicator;

        public OneTimePasswordApplication(IOneTimePasswordConfiguration configuration, IOneTimePasswordGenerator oneTimePasswordGenerator, IOneTimePasswordRepository oneTimePasswordRepository, IOneTimePasswordCommunicator oneTimePasswordCommunicator)
        {
            this.configuration = configuration;
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

        public async Task<OneTimePasswordWithExpirationInSeconds?> GetPasswordWithExpirationForUser(string userId)
        {
            var validPassword = await oneTimePasswordRepository.GetValidPasswordForUserId(userId);
            if (validPassword == null)
            {
                return default(OneTimePasswordWithExpirationInSeconds);
            }

            return new OneTimePasswordWithExpirationInSeconds
            {
                Value = validPassword.Value,
                ExpirationInSeconds =
                validPassword.ExpirationDate.Subtract(DateTime.UtcNow).Seconds
            };
        }

        private DateTime GetExpirationDate()
        {
            return DateTime.UtcNow.AddSeconds(configuration.MaximumNumberOfSecondsForValidity);
        }
    }
}