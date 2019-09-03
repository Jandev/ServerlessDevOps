using HttpBinding.HttpCommand;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerlessDevOps.Model;
using ServerlessDevOps.Templates;
using System.Threading.Tasks;

namespace ServerlessDevOps
{
	public static class FailingEventsMonitor
	{
		[FunctionName("FailingEventsMonitor")]
		public async static Task Run(
			[QueueTrigger("failing-events", Connection = "FailingEventsQueueConnection")]
			string failingEventMessage,
			[HttpCommand(CommandUrl = "%TeamsWebhookUrl%", HttpMethod = "POST", MediaType = "application/json")]
			IAsyncCollector<HttpCommand> httpCommandCollector,
			ILogger log)
		{
			log.LogInformation($"Executing {nameof(FailingEventsMonitor)}");
			
			var eventGridMessage = JsonConvert.DeserializeObject<EventGridMessage>(failingEventMessage);

			log.LogDebug($"EventGrid message data: `{eventGridMessage.data}`");

			var messageModel = new FailingEventData { Data = eventGridMessage.data };

			var messageCard = new FailingEventCard(messageModel);
			var messageContent = messageCard.TransformText();
			await httpCommandCollector.AddAsync(new HttpCommand(messageContent));
			
			log.LogInformation($"Executed {nameof(FailingEventsMonitor)}");
		}
	}

}
