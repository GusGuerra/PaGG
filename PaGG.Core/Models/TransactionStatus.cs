namespace PaGG.Core.Models
{
    public static class TransactionStatus
    {
        public const string Processing = "Processing";
        public const string Authorizing = "Authorizing";
        public const string Authorized = "Authorized";
        public const string Denied = "Denied";
        public const string Canceled = "Canceled";
    }
}
