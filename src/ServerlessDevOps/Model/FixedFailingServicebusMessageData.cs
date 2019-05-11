namespace ServerlessDevOps.Model
{
	public class FixedFailingServicebusMessageData
	{
		public string ResourceId { get; }
		public string Entity { get; }

		public FixedFailingServicebusMessageData(string resourceId, string entity)
		{
			this.ResourceId = resourceId;
			this.Entity = entity;
		}
	}
}