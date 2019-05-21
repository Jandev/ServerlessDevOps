# What's this?

A small sample project to show how you can work with serverless Azure technologies in order to
do some realtime DevOps work.

Instead of doing a lot of manual labour, your own logic can handle a lot of mundane tasks for you.


This is still a work in progress...

## Configuration

In order for this project to work, you do need some configuration parameters.
A `local.settings.json` file should look something like this

	{
		"IsEncrypted": false,
		"Values": {
			"AzureWebJobsStorage": "UseDevelopmentStorage=true",
			"FUNCTIONS_WORKER_RUNTIME": "dotnet",
			"TeamsWebhookUrl": "[WebhookUrlToATeamsChannelConfiguredInMicrosoftTeams]",
			"FixFailingServicebusUrl": "[TheEndpointToWhateverFixesYourServicebus]"
		}
	}

The `TeamsWebhookUrl` is a webhook URL configured in Microsoft Teams which a the service will `POST`
to whenever an alert is received.

The `FixFailingServicebusUrl` is the URL which is invoked when the button is pressed in the
Microsoft Teams message to fix the Service Bus issue. In this project we're only making an endpoint
which does nothing, but you probably know what to do when implementing this in your own solution.

# The Functions

## FixFailingServicebus

## ServicebusAlert

## TimeoutTest

This function is meant to test which is the maximum timeout a response message can take in order to be processed within Teams.
You should POST a message to the Microsoft Teams webhook with the following format.

	{
		"@type": "MessageCard",
		"@context": "https://schema.org/extensions",
		"summary": "Testing the timeout",
		"themeColor": "0078D7",
		"sections": [
			{
				"activityImage": "https://jan-v.nl/Media/logo.png",
				"activityTitle": "Timeout test",
				"activitySubtitle":"Testing, testing...",
				"facts": [
					{
						"name": "Timeout (miliseconds):",
						"value": "5000"
					}
				],
				"text": "The response will return with a timeout of 5000 miliseconds.",
				"potentialAction": [
					{
						"@type": "HttpPOST",
						"name": "TimeoutTest",
						"target": "https://[yourFunctionApp].azurewebsites.net/api/TimeoutTest?code=[FunctionKey]",
						"body": "{\"timeout\": 5000 }"
					}
				]
			}
		]
	}

This message will have a button triggering the `TimeoutTest` Azure Function with a specified timeout of 5000 miliseconds.
If you want a higher timeout, you can make it any amount you like.

When the Azure Function returns, it'll create a reply to the original message.