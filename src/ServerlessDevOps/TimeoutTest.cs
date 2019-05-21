using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ServerlessDevOps.Templates;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ServerlessDevOps
{
	public static class TimeoutTest
	{
		[FunctionName(nameof(TimeoutTest))]
		public static async Task<HttpResponseMessage> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			log.LogInformation($"Excuting {nameof(TimeoutTest)}.");

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			var request = JsonConvert.DeserializeAnonymousType(requestBody, new { Timeout = 0 });

			if (request.Timeout > 0)
			{
				Thread.Sleep(request.Timeout);
			}
			var result = new HttpResponseMessage
			{
				Content = new StringContent(GetContent(request.Timeout)),
				Headers =
				{
					{ "CARD-ACTION-STATUS", $"Timeout of `{request.Timeout}` miliseconds has expired."},
					{ "CARD-UPDATE-IN-BODY", "true" }
				},
				StatusCode = HttpStatusCode.OK
			};
			result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

			log.LogInformation($"Excuted {nameof(TimeoutTest)}.");

			return result;
		}

		private static string GetContent(int timeout)
		{
			var messageCard = new TimeoutCard(timeout);
			var messageContent = messageCard.TransformText();
			return messageContent;
		}
	}
}
