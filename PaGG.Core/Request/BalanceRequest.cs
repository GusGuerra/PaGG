using System.Runtime.Serialization;

namespace PaGG.Core.Request
{
	[DataContract]
	public class BalanceRequest
	{
		[DataMember]
		public long Amount { get; set; }
	}
}
