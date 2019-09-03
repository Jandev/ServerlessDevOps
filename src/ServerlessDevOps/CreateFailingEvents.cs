using System;
using AzureFunctions.EventGrid;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ServerlessDevOps
{
	public static class CreateFailingEvents
	{
		private const string Every5Seconds = "*/5 * * * * *";

		[FunctionName(nameof(CreateFailingEvents))]
		public static void Run(
			[TimerTrigger(Every5Seconds)]TimerInfo myTimer,
			[EventGrid(
				// The endpoint of your Event Grid Topic, this should be specified in your application settings of the Function App
				TopicEndpoint = "EventGridBindingSampleTopicEndpoint", 
				// This is the secret key to connect to your Event Grid Topic. To be placed in the application settings.
				TopicKey = "EventGridBindingSampleTopicKey")]
			IAsyncCollector<Event> outputCollector,
			ILogger log)
		{
			log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

			var rng = new Random();
			if (rng.Next(20) % 3 == 0)
			{
				outputCollector.AddAsync(
					new Event
					{
						Data = "Something failed, please help us...",
						EventType = nameof(CreateFailingEvents),
						Subject = "Jandev/ServerlessDevOps/FailingEvent"
					});
			}

		}
	}
}
