using ServerlessDevOps.Model;

namespace ServerlessDevOps.Templates
{
	partial class AzureMonitorAlertCard
	{
		private readonly AzureMonitorAlertCardMessageData messageData;
		private readonly string fixServicebusUrl;

		public AzureMonitorAlertCard(AzureMonitorAlertCardMessageData messageData, string fixServicebusUrl)
		{
			this.messageData = messageData;
			this.fixServicebusUrl = fixServicebusUrl;
		}
	}
}
