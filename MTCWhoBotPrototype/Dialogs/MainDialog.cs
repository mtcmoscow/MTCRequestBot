// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.10.3

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards.Templating;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using MTCRequestBot;
using MTCRequestBot.Cards;
using MTCRequestBot.CognitiveModels;
using MTCWhoBotPrototype.Helpers;
//using MTCWhoBotPrototype.CognitiveModels;
using MTCWhoBotPrototype.Model;
using MTCWhoBotPrototype.Translation;
using Newtonsoft.Json;

namespace MTCWhoBotPrototype.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly MTCRequestRecognizer _luisRecognizer;
        private readonly MicrosoftTranslator _translator;
        protected readonly ILogger Logger;

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(MTCRequestRecognizer luisRecognizer, 
            MTCRequestDialog mtcRequestDialog, 
            IBotTelemetryClient telemetryClient, MicrosoftTranslator translator,
            ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _luisRecognizer = luisRecognizer;
            _translator = translator ?? throw new ArgumentNullException(nameof(translator));
            Logger = logger;
            
            // Set the telemetry client for this and all child dialogs
            this.TelemetryClient = telemetryClient;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(mtcRequestDialog);
            AddDialog(new WaterfallDialog(nameof(MainDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(MainDialog);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_luisRecognizer.IsConfigured)
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("NOTE: LUIS is not configured. To enable all capabilities, add 'LuisAppId', 'LuisAPIKey' and 'LuisAPIHostName' to the appsettings.json file.", inputHint: InputHints.IgnoringInput), cancellationToken);

                return await stepContext.NextAsync(null, cancellationToken);
            }

            // Cards are sent as Attachments in the Bot Framework.
            // So we need to create a list of attachments for the reply activity.
            var attachments = new List<Attachment>();
            var reply = MessageFactory.Attachment(attachments);
            //reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments.Add(AdaptiveCardHelper.GetHeroCard().ToAttachment());
            //reply.Attachments.Add(AdaptiveCardHelper.GetMTCHeroCard().ToAttachment());
            //reply.Attachments.Add(AdaptiveCardHelper.GetThumbnailCard().ToAttachment());
            //reply.Attachments.Add(AdaptiveCardHelper.GetReceiptCard().ToAttachment()); // Doesn't support msteams???
            //await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            

            //var responseActivityList = stepContext.Context.Activity.CreateReply();
            //responseActivityList.TextFormat = TextFormatTypes.Markdown;
            //responseActivityList.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            //responseActivityList.Attachments.Add(AdaptiveCardHelper.GetMTCMoscowTeam());
            //await stepContext.Context.SendActivityAsync(responseActivityList);

            //foreach (var card in AdaptiveCardHelper.AdaptiveCardsDemo)
            //{
            //    var cardAttachment = AdaptiveCardHelper.CreateAdaptiveCardAttachmentFromFile(card);
            //    await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(cardAttachment), cancellationToken);
            //}
            

            // Use the text provided in FinalStepAsync or the default if it is the first time.
            var messageText = stepContext.Options?.ToString() ?? "Say something like \"I would like to have a briefing in MTC Moscow next Monday\"";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            //var filePath = Path.Combine(".", "Resources", "ReturningUserIntroCard.json");
            //var welcomeCard = AdaptiveCardHelper.CreateAdaptiveCardAttachmentFromFile(filePath);
            

            //var welcomeCard = card.ToAttachment();
            //var response = MessageFactory.Attachment(welcomeCard, ssml: "Welcome to Bot Framework!");
            //await stepContext.Context.SendActivityAsync(response, cancellationToken);

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_luisRecognizer.IsConfigured)
            {
                // LUIS is not configured, we just run the BookingDialog path with an empty BookingDetailsInstance.
                //return await stepContext.BeginDialogAsync(nameof(BookingDialog), new BookingDetails(), cancellationToken);
            }
            LogActivityTelemetry(stepContext.Context.Activity);

            //string teamId2 = TeamsHelper.ParseTeamIdFromDeepLink(@"https://teams.microsoft.com/l/team/19%3a31780574632d42feafc9319a9db6d627%40thread.skype/conversations?groupId=30f22177-36f9-4527-8879-d1cb5e6f6928&tenantId=72f988bf-86f1-41af-91ab-2d7cd011db47");        
            //var confirmationCard2 = new AdaptiveCardHelper().CreateAdaptiveCardAttachment("requestCard");

            //var resourceResponse2 = await TeamsHelper.SendCardToTeamAsync(stepContext.Context, confirmationCard2, teamId2, cancellationToken).ConfigureAwait(false);
            //await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(confirmationCard2), cancellationToken);

            //R0https://teams.microsoft.com/l/team/19%3A4aac7a7ca1b643d58e7df45dbf88aafa%40thread.tacv2/conversations?groupId=ada25de6-760b-41c9-b651-323c5cc1a87e&tenantId=a028b92b-f807-4d50-a2b4-5235dd043056
            //MSFThttps://teams.microsoft.com/l/team/19%3a31780574632d42feafc9319a9db6d627%40thread.skype/conversations?groupId=30f22177-36f9-4527-8879-d1cb5e6f6928&tenantId=72f988bf-86f1-41af-91ab-2d7cd011db47
            //var confirmationCard3 = new AdaptiveCardHelper().CreateAdaptiveCardAttachment("requestCard");
            //var resourceResponse2 = await TeamsHelper.SendCardToTeamAsync(stepContext.Context, confirmationCard2, teamId2, cancellationToken).ConfigureAwait(false);
            //await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(confirmationCard3), cancellationToken);

            //var confirmationCard4 = new AdaptiveCardHelper().CreateAdaptiveCardAttachment("requestNotificationCard");
            //var resourceResponse2 = await TeamsHelper.SendCardToTeamAsync(stepContext.Context, confirmationCard4, teamId2, cancellationToken).ConfigureAwait(false);
            //await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(confirmationCard4), cancellationToken);
                     

            if (stepContext.Context.Activity.Text.ToLower().Contains("retail"))
            {
                var people = new List<ListItemBase>();
                var pp = await OneMTCApiHelper.GetAll();
                var items = pp.Users;
                var industrySME = from ta in items
                                  where ta.Industry.ToLower().Contains("retail")
                                  select ta;

                foreach (var item in industrySME)
                {
                    people.Add(new PersonListItem()
                    {
                        ID = item.Alias + "@microsoft.com",
                        Title = item.FullName + " (" + item.Location + ")",
                        Subtitle = item.Qualifier,
                        Tap = new CardAction() { Type = ActionTypes.ImBack, Value = item.Alias + "@microsoft.com" }
                    });
                }

                var attachmentList = new Attachment()
                {
                    ContentType = ListCard.ContentType,
                    Content = new ListCard
                    {
                        Title = "The following people are retail industry experts:",
                        ListItems = people                        
                    }
                };
                var responseActivityList = stepContext.Context.Activity.CreateReply();
                responseActivityList.TextFormat = TextFormatTypes.Markdown;
                responseActivityList.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                responseActivityList.Attachments.Add(attachmentList);
                await stepContext.Context.SendActivityAsync(responseActivityList);

                //var expertsCard = new AdaptiveCardHelper().CreateAdaptiveCardAttachment("expertsListCard");
                //await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(expertsCard), cancellationToken);
                return await stepContext.NextAsync(null, cancellationToken);
            }

            else if (stepContext.Context.Activity.Text.Contains("power bi") && stepContext.Context.Activity.Text.Contains("moscow"))
            {
                var ta = new PersonCard()
                {
                    Upn = "Anna.Sviridova@microsoft.com",
                    JobTitle = "TA from MTC Moscow",
                    Buttons = new List<CardAction>()
                            {
                                new CardAction() { Title = "Request", Type = ActionTypes.ImBack, Value = "i would like to have a briefing in mtc moscow next friday" },
                                new CardAction() { Title = "MTC Moscow Info", Type = ActionTypes.ImBack, Value = "show mtc facility information" }
                            },
                };

                var responseActivity = stepContext.Context.Activity.CreateReply();
                responseActivity.TextFormat = TextFormatTypes.Markdown;                
                responseActivity.Attachments.Add(ta.ToAttachment());
                await stepContext.Context.SendActivityAsync(responseActivity);
                return await stepContext.NextAsync(null, cancellationToken);
            }            
            else if (stepContext.Context.Activity.Text.Contains("power bi"))
            {
                var people = new List<ListItemBase>()
                        {
                            //new SectionListItem() { Title = "MTC Director" },
                            //new PersonListItem() {ID = "olegka@microsoft.com", Title = "Oleg Karacharov", Subtitle = "MTC Director - Russia, Moscow", Tap = new CardAction() { Type = ActionTypes.ImBack, Value = "whois gsheldon@microsoft.com"} },
                            //new SectionListItem() { Title = "MTC Moscow TAs" },
                            new PersonListItem() {ID = "Anna.Sviridova@microsoft.com", Title = "Anna Sviridova (Moscow)", Subtitle = "Power BI, AI, SQL Server", Tap = new CardAction() { Type = ActionTypes.ImBack, Value = "power bi in moscow"} },
                            
                            //new PersonListItem() {ID = "v-seziuz@microsoft.com", Title = "Sergei Ziuzev", Subtitle = "Briefing Coordinator, IoT", Tap = new CardAction() { Type = ActionTypes.ImBack, Value = "whois vchawla@microsoft.com"} }
                        };
                var pp = await OneMTCApiHelper.GetAll();
                var items = pp.Users;
                var appDevs = from s in items
                              where s.Skills.Contains("Power BI")
                              select s;


                foreach (var item in appDevs)
                {
                    people.Add(new PersonListItem()
                    {
                        ID = item.Alias + "@microsoft.com", 
                        Title = item.FullName + " (" + item.Location + ")", 
                        Subtitle = item.Qualifier, 
                        Tap = new CardAction() { Type = ActionTypes.ImBack, Value = item.Alias + "@microsoft.com" } 
                    });
                }

                var attachmentList = new Attachment()
                {
                    ContentType = ListCard.ContentType,
                    Content = new ListCard
                    {
                        Title = "The following people know about 'Power BI'(list-card):",
                        ListItems = people                        
                    }
                };
                var responseActivityList = stepContext.Context.Activity.CreateReply();
                responseActivityList.TextFormat = TextFormatTypes.Markdown;
                responseActivityList.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                responseActivityList.Attachments.Add(attachmentList);
                await stepContext.Context.SendActivityAsync(responseActivityList);

                //var expertsCard = new AdaptiveCardHelper().CreateAdaptiveCardAttachment("expertsListCard");
                //await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(expertsCard), cancellationToken);
                return await stepContext.NextAsync(null, cancellationToken);
            }
            if (stepContext.Context.Activity.Text.Contains("facility"))
            {
                var expertsCard = new AdaptiveCardHelper().CreateAdaptiveCardAttachment("mtcInfoCard");
                await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(expertsCard), cancellationToken);
                return await stepContext.NextAsync(null, cancellationToken);
            }

            stepContext.Context.Activity.Text 
                = await _translator.TranslateAsync(stepContext.Context.Activity.Text, 
                /*TranslationSettings.DefaultLanguage*/"en", cancellationToken);

            

            // Call LUIS and gather any potential mtc request details. (Note the TurnContext has the response to the prompt.)
            MTCRequest luisResult = await _luisRecognizer.RecognizeAsync<MTCRequest>(stepContext.Context, cancellationToken);
            switch (luisResult.TopIntent().intent)
            {
                case MTCRequest.Intent.MTCRequest:
                    var mtcRequestDetail = new MTCRequestDetail
                    {
                        Location = luisResult.Location,
                        EngagementType = luisResult.EngagementType,
                        Date = luisResult.Date,
                        LeadTA = "mibon@microsoft.com"
                    };
                    return await stepContext.BeginDialogAsync(nameof(MTCRequestDialog), mtcRequestDetail, cancellationToken);

                case MTCRequest.Intent.BookFlight:
                    // await ShowWarningForUnsupportedCities(stepContext.Context, luisResult, cancellationToken);

                    // Initialize BookingDetails with any entities we may have found in the response.
                    //var bookingDetails = new BookingDetails()
                    //{
                    //    // Get destination and origin from the composite entities arrays.
                    //    Destination = luisResult.ToEntities.Airport,
                    //    Origin = luisResult.FromEntities.Airport,
                    //    TravelDate = luisResult.TravelDate,
                    //};

                    // Run the BookingDialog giving it whatever details we have from the LUIS call, it will fill out the remainder.
                    //return await stepContext.BeginDialogAsync(nameof(BookingDialog), bookingDetails, cancellationToken);

                    

                    break;

                case MTCRequest.Intent.GetWeather:
                    // We haven't implemented the GetWeatherDialog so we just display a TODO message.
                    var getWeatherMessageText = "TODO: get weather flow here";
                    var getWeatherMessage = MessageFactory.Text(getWeatherMessageText, getWeatherMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(getWeatherMessage, cancellationToken);

                    

                    break;

                default:
                    //string teamId = "19%3A4aac7a7ca1b643d58e7df45dbf88aafa%40thread.tacv2";//"19:31780574632d42feafc9319a9db6d627@thread.skype"; // ParseTeamIdFromDeepLink("https://teams.microsoft.com/l/team/19%3ad6104fe56678426798ec162ecb9bb6ab%40thread.tacv2/conversations?groupId=f6131445-9f5e-4ac6-b9a3-242273231818&tenantId=72f988bf-86f1-41af-91ab-2d7cd011db47");
                    //var confirmationCard = new AdaptiveCardHelper().CreateAdaptiveCardAttachment("newRequestNotificationCard");
                    //var resourceResponse = await TeamsHelper.SendCardToTeamAsync(stepContext.Context, confirmationCard, teamId, cancellationToken).ConfigureAwait(false);
                    
                    // Catch all for unhandled intents
                    var didntUnderstandMessageText = $"Sorry, I didn't get that. Please try asking in a different way (intent was {luisResult.TopIntent().intent})";
                    var didntUnderstandMessage = MessageFactory.Text(didntUnderstandMessageText, didntUnderstandMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(didntUnderstandMessage, cancellationToken);
                    break;
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }       
              
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // If the child dialog ("MTCRequestDialog") was cancelled, the user failed to confirm or if the intent wasn't BookFlight
            // the Result here will be null.
            if (stepContext.Result is MTCRequestDetail result)
            {
                // Now we have all the booking details call the booking service.
                // If the call to the booking service was successful tell the user.

                var timeProperty = new TimexProperty(result.Date);
                var travelDateMsg = timeProperty.ToNaturalLanguage(DateTime.Now);
                var messageText = $"I have you booked {result.Location} {result.EngagementType} on {travelDateMsg}";
                var message = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(message, cancellationToken);
            }
            else // actionable adaptive card // пользователь что-то хочет поменять
            {
                // перенес в MainDialog
            }

            // Restart the main dialog with a different message the second time around
            var promptMessage = "What else can I do for you?";            
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }

        /// <summary>
        /// Log telemetry about the incoming activity.
        /// </summary>
        /// <param name="activity">The activity</param>
        private void LogActivityTelemetry(Activity activity)
        {
            var fromObjectId = activity.From?.AadObjectId; //activity.From?.Properties["aadObjectId"]?.ToString();
            var userFullName = activity.Name;
            var clientInfoEntity = activity.Entities?.Where(e => e.Type == "clientInfo")?.FirstOrDefault();
            var channelData = activity.GetChannelData<TeamsChannelData>();

            var properties = new Dictionary<string, string>
            {
                { "ActivityId", activity.Id },
                { "ActivityType", activity.Type },
                { "UserAadObjectId", fromObjectId },
                { "UserFullName", userFullName },
                {
                    "ConversationType",
                    string.IsNullOrWhiteSpace(activity.Conversation?.ConversationType) ? "personal" : activity.Conversation.ConversationType
                },
                { "ConversationId", activity.Conversation?.Id },
                { "TeamId", channelData?.Team?.Id },
                { "Locale", clientInfoEntity?.Properties["locale"]?.ToString() },
                { "Country", clientInfoEntity?.Properties["country"]?.ToString() },
                { "TimeZone", clientInfoEntity?.Properties["timezone"]?.ToString() },
                { "Platform", clientInfoEntity?.Properties["platform"]?.ToString() }
            };
            this.TelemetryClient.TrackEvent("UserActivity", properties);
        }

    }
}
