using OneTimePasswordBusinessLogic.Interfaces;
using OneTimePasswordBusinessLogic.Models;

namespace OneTimePasswordBusinessLogic.DefaultImplementations
{
    internal class OneTimePasswordRepository : IOneTimePasswordRepository
    {
        private static readonly Dictionary<string, OneTimePassword> passwords = new Dictionary<string, OneTimePassword>();

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

    }
}
