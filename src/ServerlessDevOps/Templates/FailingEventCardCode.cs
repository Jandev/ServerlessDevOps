using ServerlessDevOps.Model;

namespace ServerlessDevOps.Templates
{
	public partial class FailingEventCard
	{
		private readonly FailingEventData failingEventData;

		public FailingEventCard(FailingEventData failingEventData)
		{
			this.failingEventData = failingEventData;
		}
	}
}
