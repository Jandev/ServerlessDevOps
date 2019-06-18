using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ServerlessDevOps.Authorization;

namespace ServerlessDevOps
{
	public static class AuthorizationTest
	{
		[FunctionName("AuthorizationTest")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] 
			HttpRequest request,
			ILogger log)
		{
			log.LogInformation($"Excuting {nameof(AuthorizationTest)}.");
			
			var bearerToken = request.Headers["Authorization"].ToString();
			log.LogDebug($"Found bearer token `{bearerToken}`");

			var baseUrl = $"{request.Scheme}{Uri.SchemeDelimiter}{request.Host.Value}";

			var validationResult = new ActionableMessageTokenValidationResult();
			try
			{
				var tokenValidator = new ActionableMessageTokenValidator();
				validationResult = await tokenValidator.ValidateTokenAsync(bearerToken.Replace("Bearer ", ""), baseUrl);
			}
			catch(Exception ex)
			{
				log.LogError(ex, "Validation failed");
			}

			if(validationResult.ValidationSucceeded)
			{
				log.LogInformation($"Token is valid! With sender `{validationResult.Sender}` and performer `{validationResult.ActionPerformer}`.");
			}
			else
			{
				log.LogWarning($"{validationResult.Exception}");
			}
			

			log.LogInformation($"Excuted {nameof(AuthorizationTest)}.");

			return new OkResult();
		}
	}
}
