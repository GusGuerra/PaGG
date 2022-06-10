using PaGG.Core.Request;
using System;

namespace PaGG.Core.Models
{
    public class BillingOption
    {
        private readonly char[] WhitespaceCharacter = new char[1] { ' ' };

        public string BankName { get; set; }
        public string CardHolder { get; set; }
        public string CardExpiration { get; set; }
        public string CardNumber { get; set; }

        public bool IsValid()
        {
            if (!CardHolder.Contains(WhitespaceCharacter[0]))
                return false;

            if (!DateTime.TryParse(CardExpiration, out _))
                return false;

            if (CardNumber.Length != 19)
                return false;

            foreach (char c in CardNumber)
                if (c != WhitespaceCharacter[0] && (c < '0' || c > '9'))
                    return false;

            string[] splitCardNumber = CardNumber.Split(WhitespaceCharacter[0]);

            if (splitCardNumber.Length != 4)
                return false;

            foreach (string numbers in splitCardNumber)
                if (numbers.Length != 4) return false;

            return true;
        }

        public BillingOption(BillingOptionRequest source)
        {
            BankName = source.BankName;
            CardExpiration = source.CardExpiration;
            CardHolder = source.CardHolder;
            CardNumber = source.CardNumber;
        }
    }
}
