using PaGG.Core.Request;

namespace PaGG.Core.Models
{
    public class BillingOption
    {
        public string BankName { get; set; }
        public string CardHolder { get; set; }
        public string CardExpiration { get; set; }
        public string CardNumber { get; set; }

        public BillingOption(BillingOptionRequest source)
        {
            BankName = source.BankName;
            CardExpiration = source.CardExpiration;
            CardHolder = source.CardHolder;
            CardNumber = source.CardNumber;
        }
    }
}
