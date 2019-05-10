using HttpBinding.HttpCommand;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerlessDevOps.Model;
using ServerlessDevOps.Templates;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ServerlessDevOps
{
	public static class ServicebusAlert
	{
		[FunctionName("ServicebusAlert")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			[HttpCommand(CommandUrl = "%TeamsWebhookUrl%", HttpMethod = "POST", MediaType = "application/json")]
			IAsyncCollector<HttpCommand> httpCommandCollector,
			ILogger log)
		{
			log.LogInformation($"Executing {nameof(ServicebusAlert)}.");

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

			log.LogInformation(requestBody);

			var messageData = GetMessageData(requestBody);
			log.LogDebug($"Alert identifier is `{messageData.AlertId}`.");

			var messageCard = CreateMessageCard(messageData);
			log.LogDebug($"Created message card body: {Environment.NewLine}`{messageCard}`.");

			await httpCommandCollector.AddAsync(new HttpCommand(messageCard));

			log.LogInformation($"Executed {nameof(ServicebusAlert)}.");

			return new OkResult();
		}

		private static MessageData GetMessageData(string requestBody)
		{
			var alert = JsonConvert.DeserializeObject<IncomingAzureMonitorCommonAlertSchema>(requestBody);
			var messageData = new MessageData(alert);
			return messageData;
		}

		private static string CreateMessageCard(MessageData messageData)
		{
			var fixFailingServicebusUrl = Environment.GetEnvironmentVariable("FixFailingServicebusUrl", EnvironmentVariableTarget.Process);

			var messageCard = new AzureMonitorAlertCard(messageData, fixFailingServicebusUrl);
			var messageContent = messageCard.TransformText();
			return messageContent;
		}
	}
}
