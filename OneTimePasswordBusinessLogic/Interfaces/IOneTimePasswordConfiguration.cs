namespace OneTimePasswordBusinessLogic.Interfaces
{
    public interface IOneTimePasswordConfiguration
    {
        public int MaximumNumberOfSecondsForValidity { get; }
    }
}
