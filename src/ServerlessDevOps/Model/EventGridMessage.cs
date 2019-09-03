using System;

namespace ServerlessDevOps.Model
{
	public class EventGridMessage
	{
		public string id { get; set; }
		public string subject { get; set; }
		public string data { get; set; }
		public string eventType { get; set; }
		public DateTime eventTime { get; set; }
		public string dataVersion { get; set; }
		public string metadataVersion { get; set; }
		public string topic { get; set; }
	}

}
