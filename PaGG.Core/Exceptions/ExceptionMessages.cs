namespace PaGG.Core.Exceptions
{
    public static class ExceptionMessages
    {
        public const string TransactionNotFound = "A transaction with the specified id could not be found";
        public const string AccountNotFound = "An account with the specified id could not be found";
        public const string InvalidTransactionId = "The specified transaction id is invalid";
        public static readonly string InvalidAccountId = "The specified account id is invalid";
    }
}
