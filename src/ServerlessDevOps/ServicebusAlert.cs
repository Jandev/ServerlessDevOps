using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ServerlessDevOps.Model;
using Newtonsoft.Json;
using ServerlessDevOps.Templates;

namespace ServerlessDevOps
{
	public static class ServicebusAlert
	{
		[FunctionName("ServicebusAlert")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			log.LogInformation($"Executing {nameof(ServicebusAlert)}.");

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

			log.LogInformation(requestBody);
			
			var messageData = GetMessageData(requestBody);
			
			var messageCard = CreateMessageCard(messageData);

			log.LogInformation($"Executed {nameof(ServicebusAlert)}.");

			return new OkResult();
		}

		private static MessageData GetMessageData(string requestBody)
		{
			var alert = JsonConvert.DeserializeObject<AzureMonitorCommonAlertSchema>(requestBody);
			var messageData = new MessageData(alert);
			return messageData;
		}

		private static string CreateMessageCard(MessageData messageData)
		{
			var messageCard = new AzureMonitorAlertCard(messageData);
			var messageContent = messageCard.TransformText();
			return messageContent;
		}
	}
}
