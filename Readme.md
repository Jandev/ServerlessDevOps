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

Nothing fancy is happening over here.
On a real project, this is where you would place the logic to fix the issue (if it can be automated). 
I'm also returning a reply to the Microsoft Teams message stating the issue has been fixed.
Of course, if some exception has occurred, this is also the place where you can edit a message and stating 
some details on the exception.

## ServicebusAlert

This Azure Function should/will be called by Azure Monitor whenever something is wrong on an Azure Servicebus Queue.
The Azure Monitor should be configured as a Webhook and the incoming message will look similar to the following message.

	{
		"schemaId": "azureMonitorCommonAlertSchema",
		"data": {
			"essentials": {
				"alertId": "/subscriptions/3b3734b4-021a-48b5-a2eb-4be0c7e7f44/providers/Microsoft.AlertsManagement/alerts/542dd931-f7c4-4432-adaa-533877e2e76b9",56h			"alertRule": "More as 100 messages on queues",
				"severity": "Sev3",
				"signalType": "Metric",
				"monitorCondition": "Fired",
				"monitoringService": "Platform",
				"alertTargetIDs": [
					"/subscriptions/3b3734b4-021a-48b5-a2eb-4be0c7e7f44/resourcegroups/binding-trial/providers/microsoft.servicebus/namespaces/functionbindings"
				],
				"originAlertId": "3b3729b4-021a-48b5-a2eb-47be0c7e7f44_Binding-trial_microsoft.insights_metricAlerts_More as 100 messages on queues_-1414896316",
				"firedDateTime": "2019-05-02T19:32:20.7084714Z",
				"description": "",
				"essentialsVersion": "1.0",
				"alertContextVersion": "1.0"
			},
			"alertContext": {
				"properties": null,
				"conditionType": "SingleResourceMultipleMetricCriteria",
				"condition": {
					"windowSize": "PT5M",
					"allOf": [
						{
							"metricName": "Messages",
							"metricNamespace": "Microsoft.ServiceBus/namespaces",
							"operator": "GreaterThan",
							"threshold": "100",
							"timeAggregation": "Average",
							"dimensions": [
								{
									"name": "ResourceId",
									"value": "1b3123b4-022a-48b5-a2eb-48be0c7e7f44:functionbindings"
								},
								{
									"name": "EntityName",
									"value": "correct-implementation-netframework"
								}
							],
							"metricValue": 10000.0
						}
					],
					"windowStartTime": "2019-05-02T19:23:55.645Z",
					"windowEndTime": "2019-05-02T19:28:55.645Z"
				}
			}
		}
	}

The incoming 


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