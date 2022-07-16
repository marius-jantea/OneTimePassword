using OneTimePasswordBusinessLogic.Interfaces;
using OneTimePasswordBusinessLogic.Models;

namespace OneTimePasswordBusinessLogic
{
    public class AllInOneImplementation : IOneTimePasswordGenerator, IOneTimePasswordCommunicator, IOneTimePasswordRepository
    {
        private readonly static IList<OneTimePassword> passwords = new List<OneTimePassword>();

        public Task<OneTimePassword> GenerateForUser(string userId, DateTime expirationDate)
        {
            var newPassword = new OneTimePassword() { UserId = userId, ExpirationDate = expirationDate, Value = passwords.Count().ToString() };
            return Task.FromResult(newPassword);
        }

        public Task<OneTimePassword?> GetValidPasswordForUserId(string userId)
        {
            var password = passwords.Where(x => string.Equals(x.UserId, userId) && x.ExpirationDate > DateTime.UtcNow).OrderByDescending(x => x.ExpirationDate).FirstOrDefault();
            return Task.FromResult(password);
        }

        public Task Save(OneTimePassword oneTimePassword)
        {
            passwords.Add(oneTimePassword);
            return Task.CompletedTask;
        }

        public Task Send(OneTimePassword oneTimePassword)
        {
            return Task.CompletedTask;
        }
    }
}
