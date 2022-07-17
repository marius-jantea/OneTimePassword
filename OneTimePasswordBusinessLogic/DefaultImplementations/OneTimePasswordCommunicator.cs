using OneTimePasswordBusinessLogic.Interfaces;
using OneTimePasswordBusinessLogic.Models;

namespace OneTimePasswordBusinessLogic.DefaultImplementations
{
    internal class OneTimePasswordCommunicator : IOneTimePasswordCommunicator
    {
        public Task Send(OneTimePassword oneTimePassword)
        {
            return Task.CompletedTask;
        }
    }
}
