using ServerlessDevOps.Model;

namespace ServerlessDevOps.Templates
{
	partial class AzureMonitorAlertCard
	{
		private readonly MessageData messageData;
		private readonly string fixServicebusUrl;

		public AzureMonitorAlertCard(MessageData messageData, string fixServicebusUrl)
		{
			this.messageData = messageData;
			this.fixServicebusUrl = fixServicebusUrl;
		}
	}
}
