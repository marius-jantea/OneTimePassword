using OneTimePasswordBusinessLogic.Models;

namespace OneTimePasswordBusinessLogic.Interfaces
{
    public interface IOneTimePasswordRepository
    {
        Task Save(OneTimePassword oneTimePassword);
        Task<OneTimePassword?> GetValidPasswordForUserId(string userId);
    }
}
