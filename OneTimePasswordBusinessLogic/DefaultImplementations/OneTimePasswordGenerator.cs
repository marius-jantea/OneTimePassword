using OneTimePasswordBusinessLogic.Interfaces;
using OneTimePasswordBusinessLogic.Models;

namespace OneTimePasswordBusinessLogic.DefaultImplementations
{
    internal class OneTimePasswordGenerator: IOneTimePasswordGenerator
    {
        public Task<OneTimePassword> GenerateForUser(string userId, DateTime expirationDate)
        {
            Random random = new Random();
            var randomNumber = random.Next(0, 1000000);

            var newPassword = new OneTimePassword() { UserId = userId, ExpirationDate = expirationDate, Value = randomNumber.ToString("000000") };
            return Task.FromResult(newPassword);
        }
    }
}
