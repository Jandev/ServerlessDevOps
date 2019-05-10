using System;
using System.Linq;

namespace ServerlessDevOps.Model
{
	public class MessageData
	{
		public string AlertId { get; }
		public string AlertRule { get; }
		public string Severity { get; }
		public DateTime FiredAt { get; }
		public string ResourceId { get; }
		public string Entity { get; }
		public double MetricValue { get; }

		public MessageData(IncomingAzureMonitorCommonAlertSchema alert)
		{
			AlertId = alert.data.essentials.alertId;
			AlertRule = alert.data.essentials.alertRule;
			Severity = alert.data.essentials.severity;
			FiredAt = alert.data.essentials.firedDateTime;
			var firstCondition = alert.data.alertContext.condition.allOf.FirstOrDefault();
			if (firstCondition != null)
			{
				ResourceId = firstCondition.dimensions.Where(d => d.name == "ResourceId").FirstOrDefault()?.value;
				Entity = firstCondition.dimensions.Where(d => d.name == "EntityName").FirstOrDefault()?.value;
				MetricValue = firstCondition.metricValue;
			}
		}


	}
}