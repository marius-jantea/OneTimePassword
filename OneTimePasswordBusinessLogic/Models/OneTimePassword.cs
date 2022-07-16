namespace OneTimePasswordBusinessLogic.Models
{
    public class OneTimePassword
    {
        public string UserId { get; set; }
        public string Value { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
