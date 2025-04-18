// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.10.3

using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.ApplicationInsights.Core;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder.TraceExtensions;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MTCWhoBotPrototype
{
    public class AdapterWithErrorHandler : BotFrameworkHttpAdapter
    {
        private IBotTelemetryClient _adapterBotTelemetryClient;

        public AdapterWithErrorHandler(IConfiguration configuration, 
            ILogger<BotFrameworkHttpAdapter> logger,
            TelemetryInitializerMiddleware telemetryInitializerMiddleware, IBotTelemetryClient botTelemetryClient,
            ConversationState conversationState = null)
            : base(configuration, logger)
        {
            Use(telemetryInitializerMiddleware);
            Use(new ShowTypingMiddleware());

            //Use telemetry client so that we can trace exceptions into Application Insights
            _adapterBotTelemetryClient = botTelemetryClient;

            OnTurnError = async (turnContext, exception) =>
            {
                // Track exceptions into Application Insights
                // Set up some properties for our exception tracing to give more information
                var properties = new Dictionary<string, string>
                { { "Bot exception caught in", $"{ nameof(AdapterWithErrorHandler) } - { nameof(OnTurnError) }" } };

                // Send the exception telemetry
                _adapterBotTelemetryClient.TrackException(exception, properties);


                // Log any leaked exception from the application.
                //TODO: in production environment, you should consider logging this to AI. Visit https://aka.ms/bottelemetry to see how to add telemetry capture to your bot.
                logger.LogError(exception, $"[OnTurnError] unhandled error : {exception.Message}");

                // Send a message to the user
                var errorMessageText = "The bot encountered an error or bug.";
                var errorMessage = MessageFactory.Text(errorMessageText, errorMessageText, InputHints.ExpectingInput);
                await turnContext.SendActivityAsync(errorMessage);

                errorMessageText = "To continue to run this bot, please fix the bot source code.";
                errorMessage = MessageFactory.Text(errorMessageText, errorMessageText, InputHints.ExpectingInput);
                await turnContext.SendActivityAsync(errorMessage);

                if (conversationState != null)
                {
                    try
                    {
                        // Delete the conversationState for the current conversation to prevent the
                        // bot from getting stuck in a error-loop caused by being in a bad state.
                        // ConversationState should be thought of as similar to "cookie-state" in a Web pages.
                        await conversationState.DeleteAsync(turnContext);
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, $"Exception caught on attempting to Delete ConversationState : {e.Message}");
                    }
                }

                // Send a trace activity, which will be displayed in the Bot Framework Emulator
                await turnContext.TraceActivityAsync("OnTurnError Trace", exception.Message, "https://www.botframework.com/schemas/error", "TurnError");
            };
        }
    }
}
