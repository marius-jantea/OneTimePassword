using OneTimePasswordBusinessLogic.Models;

namespace OneTimePasswordBusinessLogic.Interfaces
{
    public interface IOneTimePasswordGenerator
    {
        Task<OneTimePassword> GenerateForUser(string userId, DateTime expirationDate);
    }
}
