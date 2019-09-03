using HttpBinding.HttpCommand;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerlessDevOps.Model;

namespace ServerlessDevOps
{
	public static class FailingEventsMonitor
	{
		[FunctionName("FailingEventsMonitor")]
		public static void Run(
			[QueueTrigger("failing-events", Connection = "FailingEventsQueueConnection")]
			string failingEventMessage,
			[HttpCommand(CommandUrl = "%TeamsWebhookUrl%", HttpMethod = "POST", MediaType = "application/json")]
			IAsyncCollector<HttpCommand> httpCommandCollector,
			ILogger log)
		{
			var eventGridMessage = JsonConvert.DeserializeObject<EventGridMessage>(failingEventMessage);

			log.LogInformation($"EventGrid message data: `{eventGridMessage.data}`");


			
		}
	}

}
