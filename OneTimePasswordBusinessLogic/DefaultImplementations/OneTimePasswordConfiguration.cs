using OneTimePasswordBusinessLogic.Interfaces;

namespace OneTimePasswordBusinessLogic.DefaultImplementations
{
    internal class OneTimePasswordConfiguration : IOneTimePasswordConfiguration
    {
        public int MaximumNumberOfSecondsForValidity => 30;
    }
}
