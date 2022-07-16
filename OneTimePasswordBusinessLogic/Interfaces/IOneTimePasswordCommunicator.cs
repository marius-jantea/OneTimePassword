using OneTimePasswordBusinessLogic.Models;

namespace OneTimePasswordBusinessLogic.Interfaces
{
    public interface IOneTimePasswordCommunicator
    {
        Task Send(OneTimePassword oneTimePassword);
    }
}
