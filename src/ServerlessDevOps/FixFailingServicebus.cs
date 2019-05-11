using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerlessDevOps.Model;
using ServerlessDevOps.Templates;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ServerlessDevOps
{
	public static class FixFailingServicebus
	{
		[FunctionName("FixFailingServicebus")]
		public static async Task<HttpResponseMessage> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
			HttpRequest req,
			ILogger log)
		{
			log.LogInformation($"Executing {nameof(FixFailingServicebus)}.");

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			var incomingFixServicebusModel = JsonConvert.DeserializeObject<IncomingFixServicebusModel>(requestBody);

			log.LogInformation($"Resource `{incomingFixServicebusModel.ResourceId}` - Entity `{incomingFixServicebusModel.Entity}` is in need of fixing...");

			var result = new HttpResponseMessage
			{
				Content = new StringContent(GetContent(incomingFixServicebusModel)),
				Headers =
				{
					{ "CARD-ACTION-STATUS", $"Resource `{incomingFixServicebusModel.ResourceId}` - Entity `{incomingFixServicebusModel.Entity}` is fixed now."},
					{ "CARD-UPDATE-IN-BODY", "true" }
				},
				StatusCode = HttpStatusCode.OK
			};
			result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

			log.LogInformation($"Executed {nameof(FixFailingServicebus)}.");

			return result;
		}

		private static string GetContent(IncomingFixServicebusModel incomingFixServicebusModel)
		{
			var messageData = new FixedFailingServicebusMessageData(incomingFixServicebusModel.ResourceId, incomingFixServicebusModel.Entity);

			var messageCard = new FixedFailingServicebusCard(messageData);
			var messageContent = messageCard.TransformText();
			return messageContent;
		}
	}
}
