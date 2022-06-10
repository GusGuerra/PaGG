using PaGG.Core.Models;
using PaGG.Core.Utilities;
using System.Runtime.Serialization;

namespace PaGG.Core.Response
{
    [DataContract]
    public class BillingOptionResponse
    {
        private readonly char[] WhitespaceCharacter = new char[1] { ' ' };
        private const int UnmaskedSuffix = 4;

        public BillingOptionResponse(BillingOption source)
        {
            BankName = source.BankName;
            CardHolder = source.CardHolder.Split(WhitespaceCharacter)[0];
            CardNumber = GeneralUtils.MaskStringPrefix(UnmaskedSuffix, source.CardNumber);
        }

        [DataMember]
        public string BankName { get; set; }
        [DataMember]
        public string CardHolder { get; set; }
        [DataMember]
        public string CardNumber { get; set; }
    }
}
