// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.10.3

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using Microsoft.Teams.Apps.SubmitIdea.Models.Card;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MTCWhoBotPrototype.Dialogs
{
    public class BookingDialog : CancelAndHelpDialog
    {
        private const string DestinationStepMsgText = "Where would you like to travel to?";
        private const string OriginStepMsgText = "Where are you traveling from?";

        public BookingDialog()
            : base(nameof(BookingDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new DateResolverDialog());
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                DestinationStepAsync,
                EngagementTypeStepAsync,
                OriginStepAsync,
                TravelDateStepAsync,
                ConfirmStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> DestinationStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookingDetails = (BookingDetails)stepContext.Options;

            if (bookingDetails.Destination == null)
            {
                var promptMessage = MessageFactory.Text(DestinationStepMsgText, DestinationStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(bookingDetails.Destination, cancellationToken);
        }

        private async Task<DialogTurnResult> EngagementTypeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var request = (BookingDetails)stepContext.Options;

            if (request.EngagementType == null)
            {
                var choices = new[] { "Envisioning workshop", "Strategy briefing", "Architecture design session",
                    "Rapid prototype", "Hackathon", "Hands-on lab" };
                //var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
                //{
                //    Actions = choices.Select(choice => new AdaptiveSubmitAction
                //    {
                //        Title = choice,
                //        Data = choice,
                //    }).ToList<AdaptiveAction>()
                //};

                var typesCard = new Microsoft.Teams.Apps.SubmitIdea.Models.Card.ListCard
                {
                    Title = "Select an engagement type:",
                    Items = new List<Microsoft.Teams.Apps.SubmitIdea.Models.Card.ListItem>() {
                        new ListItem()
                        {
                            Title = "Envisioning workshop",
                            Subtitle = "Help your customer comprehensively identify and prioritize their business problems, solutions and opportunities for innovation.",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/EnvisioningWorkshop.png",
                            Type = "resultItem",
                            Tap = new CardAction() { 
                                Title = "Envisioning workshop", 
                                Text = "Envisioning workshop", 
                                Type = ActionTypes.PostBack, 
                                Value = "Envisioning workshop" }
                        },
                        new ListItem()
                        {
                            Title = "Strategy briefing",
                            Subtitle = "Dive into your customer's goals and challenges through strategic and technical discussion.",
                            Type = "resultItem",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/StrategyBriefing.png",
                            Tap = new CardAction() {
                                Title = "Strategy briefing",
                                Text = "Strategy briefing",
                                Type = ActionTypes.ImBack, 
                                Value = "Strategy briefing"}
                        },
                        new ListItem()
                        {
                            Title = "Architecture design session",
                            Subtitle = "Show your customer how their ambitions can be achieved with a customized Microsoft solutions architecture.",
                            Type = "resultItem",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/ads.png",
                            Tap = new CardAction() {
                                Title = "Architecture design session",
                                Text = "Architecture design session",
                                Type = ActionTypes.ImBack,
                                Value = "Architecture design session"}
                        },
                        new ListItem()
                        {
                            Title = "Rapid prototype",
                            Subtitle = "Build out aspects of your client's proposed solution in an in-depth, hands-on engagement.",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/rapidprototype.png",
                            Tap = new CardAction() {
                                Title = "Rapid prototype",
                                Text = "Rapid prototype",
                                Type = ActionTypes.ImBack,
                                Value = "Rapid prototype"}
                        },
                        new ListItem()
                        {
                            Title = "Hackathon",
                            Subtitle = "Enable your customer to collaborate in a rapid, iterative fashion with the experts in applying creative technology solutions to business challenges.",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/hackathon.png",
                             Tap = new CardAction() {
                                Title = "Hackathon",
                                Text = "Hackathon",
                                Type = ActionTypes.ImBack,
                                Value = "Hackathon"}
                        },
                        new ListItem()
                        {
                            Title = "Hands-on lab",
                            Subtitle = "Get your cusomer hands-on with the technology in immersive exploration sessions including step-by-step  walkthroughs.",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/hol.png",
                             Tap = new CardAction() {
                                Title = "Hands-on lab",
                                Text = "Hands-on lab",
                                Type = ActionTypes.ImBack,
                                Value = "Hands-on lab"}
                        }
                    },
                };

                var listItemsWithIcons = new Attachment()
                {
                    ContentType = "application/vnd.microsoft.teams.card.list",
                    Content = typesCard
                };

                // list with icons
                var listItemsWithIcons2 = new Attachment()
                {
                    ContentType = "application/vnd.microsoft.teams.card.list",
                    Content = new MTCRequestBot.Cards.ListCard()
                    {
                        Title = "Select an engagement type:",
                        ListItems = new List<MTCRequestBot.Cards.ListItemBase>(){
                        new MTCRequestBot.Cards.ResultListItem()
                        {
                            Title = "Envisioning workshop",
                            Subtitle = "Help your customer comprehensively identify and prioritize their business problems, solutions and opportunities for innovation.",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/EnvisioningWorkshop.png",
                            Tap = new CardAction() {
                                Title = "Envisioning workshop",
                                Text = "Envisioning workshop",
                                Type = ActionTypes.ImBack,
                                Value = "Envisioning workshop" }
                        },
                        new MTCRequestBot.Cards.ResultListItem()
                        {
                            Title = "Strategy briefing",
                            Subtitle = "Dive into your customer's goals and challenges through strategic and technical discussion.",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/StrategyBriefing.png",
                            Tap = new CardAction() { Type = ActionTypes.ImBack, Value = "Strategy briefing"}
                        },
                        new MTCRequestBot.Cards.ResultListItem()
                        {
                            Title = "Architecture design session",
                            Subtitle = "Show your customer how their ambitions can be achieved with a customized Microsoft solutions architecture.",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/ads.png"
                        },
                        new MTCRequestBot.Cards.ResultListItem()
                        {
                            Title = "Rapid prototype",
                            Subtitle = "Build out aspects of your client's proposed solution in an in-depth, hands-on engagement.",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/rapidprototype.png"
                        },
                        new MTCRequestBot.Cards.ResultListItem()
                        {
                            Title = "Hackathon",
                            Subtitle = "Enable your customer to collaborate in a rapid, iterative fashion with the experts in applying creative technology solutions to business challenges.",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/hackathon.png"
                        },
                        new MTCRequestBot.Cards.ResultListItem()
                        {
                            Title = "Hands-on lab",
                            Subtitle = "Get your cusomer hands-on with the technology in immersive exploration sessions including step-by-step  walkthroughs.",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/hol.png"
                        }
                    }
                    }
                };

                return await stepContext.PromptAsync(nameof(ChoicePrompt), new PromptOptions
                {
                    Prompt = (Activity)MessageFactory.Attachment(listItemsWithIcons2),
                    //Prompt = (Activity)MessageFactory.Attachment(new Attachment
                    //{
                    //    ContentType = AdaptiveCard.ContentType,
                    //    Content = JObject.FromObject(card)
                    //}),
                    Choices = ChoiceFactory.ToChoices(choices),
                    Style = ListStyle.None
                }, cancellationToken);
            }
            

            return await stepContext.NextAsync(request.EngagementType, cancellationToken);
        }

        //private async Task SendValueToDialogAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        //{
        //    var json = JsonConvert.SerializeObject(turnContext.Activity.Value);
        //    turnContext.Activity.Text = json;
        //    var dc = await _di
        //}

        private async Task<DialogTurnResult> OriginStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookingDetails = (BookingDetails)stepContext.Options;

            bookingDetails.EngagementType = ((FoundChoice)stepContext.Result).Value;

            if (bookingDetails.Origin == null)
            {
                var promptMessage = MessageFactory.Text(OriginStepMsgText, OriginStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(bookingDetails.Origin, cancellationToken);
        }

        private async Task<DialogTurnResult> TravelDateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookingDetails = (BookingDetails)stepContext.Options;

            bookingDetails.Origin = (string)stepContext.Result;

            if (bookingDetails.TravelDate == null || IsAmbiguous(bookingDetails.TravelDate))
            {
                return await stepContext.BeginDialogAsync(nameof(DateResolverDialog), bookingDetails.TravelDate, cancellationToken);
            }

            return await stepContext.NextAsync(bookingDetails.TravelDate, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookingDetails = (BookingDetails)stepContext.Options;

            bookingDetails.TravelDate = (string)stepContext.Result;

            try
            {
                var confirmationCard = CreateAdaptiveCardAttachment();
                var response = MessageFactory.Attachment(confirmationCard);
                await stepContext.Context.SendActivityAsync(response, cancellationToken);
            }
            catch (Exception ex)
            {
                //catch the exception
                await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions
                {
                    Prompt = MessageFactory.Text(ex.Message)
                }, cancellationToken);
            }

            var messageText = $"Please confirm, You have booked {bookingDetails.EngagementType} on: {bookingDetails.TravelDate}. Is this correct?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
                var bookingDetails = (BookingDetails)stepContext.Options;     
                return await stepContext.EndDialogAsync(bookingDetails, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        // Load attachment from embedded resource.
        private Attachment CreateAdaptiveCardAttachment()
        {
            var cardResourcePath = GetType().Assembly.GetManifestResourceNames().First(name => name.EndsWith("requestConfirmationCard.json"));

            using (var stream = GetType().Assembly.GetManifestResourceStream(cardResourcePath))
            {
                using (var reader = new StreamReader(stream))
                {
                    var adaptiveCard = reader.ReadToEnd();
                    return new Attachment()
                    {
                        ContentType = AdaptiveCard.ContentType,
                        Content = JsonConvert.DeserializeObject(adaptiveCard),
                    };
                }
            }
        }

        private static bool IsAmbiguous(string timex)
        {
            var timexProperty = new TimexProperty(timex);
            return !timexProperty.Types.Contains(Constants.TimexTypes.Definite);
        }
    }
}
