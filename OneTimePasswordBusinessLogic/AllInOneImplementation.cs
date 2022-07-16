using OneTimePasswordBusinessLogic.Interfaces;
using OneTimePasswordBusinessLogic.Models;

namespace OneTimePasswordBusinessLogic
{
    public class AllInOneImplementation : IOneTimePasswordGenerator, IOneTimePasswordCommunicator, IOneTimePasswordRepository
    {
        private static readonly Dictionary<string, OneTimePassword> passwords = new Dictionary<string, OneTimePassword>();

        public Task<OneTimePassword> GenerateForUser(string userId, DateTime expirationDate)
        {
            Random random = new Random();
            var randomNumber = random.Next(0, 1000000);

            var newPassword = new OneTimePassword() { UserId = userId, ExpirationDate = expirationDate, Value = randomNumber.ToString("000000") };
            return Task.FromResult(newPassword);
        }

        public Task<OneTimePassword> GetValidPasswordForUserId(string userId)
        {
            if (!passwords.ContainsKey(userId)) return Task.FromResult(default(OneTimePassword));
            var cached = passwords[userId];
            if (DateTime.UtcNow >= cached.ExpirationDate)
            {
                passwords.Remove(userId);
                return Task.FromResult(default(OneTimePassword));
            }
            return Task.FromResult(cached);
        }

        public Task Save(OneTimePassword oneTimePassword)
        {
            passwords[oneTimePassword.UserId] = oneTimePassword;
            return Task.CompletedTask;
        }

        public Task Send(OneTimePassword oneTimePassword)
        {
            return Task.CompletedTask;
        }
    }
}
