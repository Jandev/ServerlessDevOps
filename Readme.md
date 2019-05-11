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