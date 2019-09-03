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
			log.LogInformation($"Executing {nameof(CreateFailingEvents)}");

			var rng = new Random();
			var number = rng.Next(20);
			if (number % 3 == 0)
			{
				log.LogDebug($"Found number {number}");
				outputCollector.AddAsync(
					new Event
					{
						Data = $"Something failed at {DateTime.UtcNow}, please help us...",
						EventType = nameof(CreateFailingEvents),
						Subject = "Jandev/ServerlessDevOps/FailingEvent"
					});
			}
			log.LogInformation($"Executed {nameof(CreateFailingEvents)}");
		}
	}
}
