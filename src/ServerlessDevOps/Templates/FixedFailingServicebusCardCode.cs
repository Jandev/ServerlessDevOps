using ServerlessDevOps.Model;

namespace ServerlessDevOps.Templates
{
	public partial class FixedFailingServicebusCard
	{
		private FixedFailingServicebusMessageData messageData;

		public FixedFailingServicebusCard(FixedFailingServicebusMessageData messageData)
		{
			this.messageData = messageData;
		}
	}
}
