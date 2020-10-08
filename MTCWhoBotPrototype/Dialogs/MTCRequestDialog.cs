using AdaptiveCards;
using AdaptiveCards.Templating;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using MTC.Bot.DataAccessLayer.Mock;
using MTC.Bot.DataAccessLayer.POCO;
using MTCRequestBot.Cards;
using MTCWhoBotPrototype.Helpers;
using MTCWhoBotPrototype.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MTCWhoBotPrototype.Dialogs
{
    public class MTCRequestDialog : CancelAndHelpDialog
    {
        public MTCRequestDialog() : base(nameof(MTCRequestDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new DateResolverDialog());
            //AddDialog(new EngagementTypeDialog());

            AddDialog(
                new WaterfallDialog(nameof(MTCRequestDialog), new WaterfallStep[]
                {
                    //DisplayCardAsync,
                    //HandleResponseAsync,

                    RegionStepAsync,
                    LocationStepAsync,                   
                    EngagementTypeStepAsync,
                    DateStepAsync,
                    ConfirmStepAsync,                    
                    HandleConfirmationScreenResponseAsync
                }));

            InitialDialogId = nameof(MTCRequestDialog);
        }

        private async Task<DialogTurnResult> RegionStepAsync(WaterfallStepContext stepCtx, CancellationToken cancelToken)
        {
            var requestDetails = (MTCRequestDetail)stepCtx.Options;
            if (requestDetails.Location != null && requestDetails.Region == null)
            {
                requestDetails.Region = DalMockHelper.GetRegionByLocation(requestDetails.Location);
            }
            if (requestDetails.Region == null && requestDetails.Location == null)
            {
                var regions = DalMockHelper.Regions;
                var listItems = new List<MTCRequestBot.Cards.ListItemBase>();
                foreach (var item in regions)
                {
                    listItems.Add(new MTCRequestBot.Cards.ResultListItem()
                    {
                        Title = item.Title,
                        Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/region.png",
                        Tap = new CardAction()
                        {
                            Title = item.Title,
                            Text = item.Title,
                            Type = ActionTypes.ImBack,
                            Value = item.Title
                        }
                    }) ;
                }
                var regionAttachment = new Attachment()
                {
                    ContentType = "application/vnd.microsoft.teams.card.list",
                    Content = new ListCard
                    {
                        Title = "What region is the MTC located?",
                        ListItems = listItems
                    }
                };

                
                return await stepCtx.PromptAsync(nameof(ChoicePrompt), new PromptOptions
                {
                    Prompt = (Activity)MessageFactory.Attachment(regionAttachment),
                    Choices = ChoiceFactory.ToChoices(regions.ConvertAll(item=>item.Title)),
                    Style = ListStyle.None
                }, cancelToken);
            }
            return await stepCtx.NextAsync(requestDetails.Region, cancelToken);
        }

        private async Task<DialogTurnResult> LocationStepAsync(WaterfallStepContext stepCtx, CancellationToken cancelToken)
        {
            var requestDetails = (MTCRequestDetail)stepCtx.Options;
            if (stepCtx.Result is FoundChoice)
            {
                requestDetails.Region = ((FoundChoice)stepCtx.Result).Value;
            }
            
            if (requestDetails.Location == null)
            {
                var locations = DalMockHelper.GetLocations(requestDetails.Region);  
                Attachment attachment = null;
                if (locations.Count < 15)
                {
                    var listItems = new List<MTCRequestBot.Cards.ListItemBase>();
                    foreach (var item in locations)
                    {
                        listItems.Add(new MTCRequestBot.Cards.ResultListItem()
                        {
                            Title = item.Title,
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/mtclocation.png",
                            Tap = new CardAction()
                            {
                                Title = item.Title,
                                Text = item.Title,
                                Type = ActionTypes.ImBack,
                                Value = item.Title
                            }
                        });
                    }
                    attachment = new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.teams.card.list",
                        Content = new ListCard
                        {
                            Title = $"Choose the location for this engagement.",
                            ListItems = listItems
                        }
                    };

                    return await stepCtx.PromptAsync(nameof(ChoicePrompt), new PromptOptions
                    {
                        Prompt = (Activity)MessageFactory.Attachment(attachment),
                        Choices = ChoiceFactory.ToChoices(locations.ConvertAll(item => item.Title)),
                        Style = ListStyle.None
                    }, cancelToken);
                }
                else
                {
                    // show editable adaptive card here
                    var filePath = Path.Combine(".", "Resources", "MTCLocationsCard.json");
                    var templateJson = new AdaptiveCardHelper().GetStringFromJsonAdaptiveCardFile(filePath);

                    // Create a Template instance from the template payload
                    AdaptiveCardTemplate template = new AdaptiveCardTemplate(templateJson);

                    // You can use any serializable object as your data
                    var myData = new
                    {                       
                        MTCLocations = DalMockHelper.GetLocations(requestDetails.Region)                       
                    };

                    // "Expand" the template - this generates the final Adaptive Card payload
                    string cardJson = template.Expand(myData);
                    var confirmationCard = new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(cardJson),
                    };

                    // Create the text prompt
                    var opts = new PromptOptions
                    {
                        Prompt = new Activity
                        {
                            Attachments = new List<Attachment>() { confirmationCard },
                            Type = ActivityTypes.Message,
                            //Text = "waiting for user input...", // You can comment this out if you don't want to display any text. Still works.
                        }
                    };

                    // Display a Text Prompt and wait for input
                    return await stepCtx.PromptAsync(nameof(TextPrompt), opts);
                }  
            }

            return await stepCtx.NextAsync(requestDetails.Location, cancelToken);
        }
        
        private async Task<DialogTurnResult> EngagementTypeStepAsync(WaterfallStepContext stepCtx, CancellationToken cancelToken)
        {
            var requestDetails = (MTCRequestDetail)stepCtx.Options;
            if (requestDetails.Location == null) 
            {
                if (stepCtx.Result is FoundChoice)
                {
                    requestDetails.Location = ((FoundChoice)stepCtx.Result).Value;
                }
                else
                {
                    var json = JObject.Parse(stepCtx.Result.ToString());
                    var location = json["MTCLocation"].ToString();
                    await stepCtx.Context.SendActivityAsync(location);
                    requestDetails.Location = location;
                }
            }
            
            

            if (requestDetails.EngagementType == null)
            {
                var choices = new[] { "Envisioning workshop", "Strategy briefing", "Architecture design session",
                    "Rapid prototype", "Hackathon", "Hands-on lab" };

                var engagementTypes = new List<MTCRequestBot.Cards.ListItemBase>();
                foreach (var et in DalMockHelper.GetEngagementTypes())
                {
                    var item = new MTCRequestBot.Cards.ResultListItem()
                    {
                        Title = et.DisplayName,
                        // Subtitle = "Help your customer comprehensively identify and prioritize their business problems, solutions and opportunities for innovation.",
                        Icon = et.Icon,
                        Tap = new CardAction()
                        {
                            Title = et.DisplayName,
                            Text = et.DisplayName,
                            Type = ActionTypes.ImBack,
                            Value = et.Value
                        }
                    };
                    engagementTypes.Add(item);
                }

                // list with icons
                var listItemsWithIcons2 = new Attachment()
                {
                    ContentType = "application/vnd.microsoft.teams.card.list",
                    Content = new MTCRequestBot.Cards.ListCard()
                    {
                        Title = "Please select the type of meeting for this request:",
                        ListItems = engagementTypes
                    }
                };

                return await stepCtx.PromptAsync(nameof(ChoicePrompt), new PromptOptions
                {
                    Prompt = (Activity)MessageFactory.Attachment(listItemsWithIcons2),                    
                    Choices = ChoiceFactory.ToChoices(choices),
                    Style = ListStyle.None
                }, cancelToken);
            }

            return await stepCtx.NextAsync(requestDetails.EngagementType, cancelToken);
        }

        private async Task<DialogTurnResult> DateStepAsync(WaterfallStepContext stepCtx, CancellationToken cancelToken)
        {
            var requestDetails = (MTCRequestDetail)stepCtx.Options;
            if (requestDetails.EngagementType == null && stepCtx.Result is FoundChoice)
            {
                requestDetails.EngagementType = ((FoundChoice)stepCtx.Result).Value.ToLower();
            }
            

            if (requestDetails.Date == null || IsAmbiguous(requestDetails.Date))
            {
                return await stepCtx.BeginDialogAsync(nameof(DateResolverDialog), requestDetails.Date, cancelToken);
            }

            return await stepCtx.NextAsync(requestDetails.Date, cancelToken);
        }

        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepCtx, CancellationToken cancelToken)
        {
            var requestDetails = (MTCRequestDetail)stepCtx.Options;
            if (requestDetails.Date == null)
            {
                requestDetails.Date = (string)stepCtx.Result;
            }
            

            //var messageText = $"Please confirm: Engagement type: {requestDetails.EngementType}, Location: {requestDetails.Location} Date: {requestDetails.Date}. Is this correct?";
            //var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            //return await stepCtx.PromptAsync(nameof(ConfirmPrompt),
            //    new PromptOptions { Prompt = promptMessage }, cancelToken);


            // show editable adaptive card here
            var templateJson = new AdaptiveCardHelper().GetStringFromJsonAdaptiveCardEmbeddedResource("requestConfirmationCard");

            // Create a Template instance from the template payload
            AdaptiveCardTemplate template = new AdaptiveCardTemplate(templateJson);

            // You can use any serializable object as your data
            var myData = new
            {
                Title = "",
                OppId = "no opp",
                MeetingGoals = "",
                StartDate = requestDetails.Date,
                EndDate = Convert.ToDateTime(requestDetails.Date).AddDays(1),
                EngagementType = requestDetails.EngagementType,
                EngagementTypes = DalMockHelper.GetEngagementTypes(),
                MTCLocations = DalMockHelper.GetLocations("all"),
                MTCLocation = requestDetails.Location
            };

            // "Expand" the template - this generates the final Adaptive Card payload
            string cardJson = template.Expand(myData);
            var confirmationCard = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(cardJson),
            };

            // Create the text prompt
            var opts = new PromptOptions
            {
                Prompt = new Activity
                {
                    Attachments = new List<Attachment>() { confirmationCard },
                    Type = ActivityTypes.Message,
                    //Text = "waiting for user input...", // You can comment this out if you don't want to display any text. Still works.
                }
            };

            // Display a Text Prompt and wait for input
            return await stepCtx.PromptAsync(nameof(TextPrompt), opts);
        }
       
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepCtx, CancellationToken cancelToken)
        {
            var confirmed = (bool)stepCtx.Result;
            var requestDetails = (MTCRequestDetail)stepCtx.Options;

            if (confirmed)
            {
                //TODO: create request in CRM
                return await stepCtx.EndDialogAsync(requestDetails, cancelToken);
            }
            else
            {
                // show editable adaptive card here
                var templateJson = new AdaptiveCardHelper().GetStringFromJsonAdaptiveCardEmbeddedResource("requestConfirmationCard");
                
                // Create a Template instance from the template payload
                AdaptiveCardTemplate template = new AdaptiveCardTemplate(templateJson);

                // You can use any serializable object as your data
                var myData = new
                {
                    Title = "",
                    OppId = "no opp",
                    MeetingGoals = "",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(1),
                    EngagementType = requestDetails.EngagementType,
                    EngagementTypes = DalMockHelper.GetEngagementTypes(),
                    MTCLocations = DalMockHelper.GetLocations("all"),
                    MTCLocation = requestDetails.Location
                };

                // "Expand" the template - this generates the final Adaptive Card payload
                string cardJson = template.Expand(myData);
                var confirmationCard = new Attachment()
                {
                    ContentType = "application/vnd.microsoft.card.adaptive",
                    Content = JsonConvert.DeserializeObject(cardJson),
                };

                // Create the text prompt
                var opts = new PromptOptions
                {
                    Prompt = new Activity
                    {
                        Attachments = new List<Attachment>() { confirmationCard },
                        Type = ActivityTypes.Message,
                        //Text = "waiting for user input...", // You can comment this out if you don't want to display any text. Still works.
                    }
                };

                // Display a Text Prompt and wait for input
                return await stepCtx.PromptAsync(nameof(TextPrompt), opts);
                
                
            }
            //return await stepCtx.EndDialogAsync(null, cancelToken);
        }

        private async Task<DialogTurnResult> HandleConfirmationScreenResponseAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Do something with step.result
            // Adaptive Card submissions are objects, so you likely need to JObject.Parse(step.result)
            await stepContext.Context.SendActivityAsync($"INPUT: {stepContext.Result}");
            //return await stepContext.NextAsync();
            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        private static bool IsAmbiguous(string timex)
        {
            var timexProperty = new TimexProperty(timex);
            return !timexProperty.Types.Contains(Constants.TimexTypes.Definite);
        }

        //private async Task<DialogTurnResult> DisplayCardAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        //{
        //    // Create the Adaptive Card
        //    //var card = Path.Combine(".", "Resources", "ActionableAdaptiveCard.json");
        //    var card = Path.Combine(".", "Resources", "MTCNorthAmericaLocations.json");
        //    var cardAttachment = AdaptiveCardHelper.CreateAdaptiveCardAttachmentFromFile(card);

        //    //var aCard = AdaptiveCard.FromJson(json).Card;
        //    //aCard.

        //    // Create the text prompt
        //    var opts = new PromptOptions
        //    {
        //        Prompt = new Activity
        //        {
        //            Attachments = new List<Attachment>() { cardAttachment },
        //            Type = ActivityTypes.Message,
        //            //Text = "waiting for user input...", // You can comment this out if you don't want to display any text. Still works.
        //        }
        //    };

        //    // Display a Text Prompt and wait for input
        //    return await stepContext.PromptAsync(nameof(TextPrompt), opts);
        //}

        //private async Task<DialogTurnResult> HandleResponseAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        //{
        //    // Do something with step.result
        //    // Adaptive Card submissions are objects, so you likely need to JObject.Parse(step.result)
        //    if (stepContext.Result.ToString().Contains("MTCLocation")) //{"MTCLocation":"Boston"} in case of adaptive card
        //    {
        //        var json = JObject.Parse(stepContext.Result.ToString());
        //        var location = json["MTCLocation"].ToString();
        //        await stepContext.Context.SendActivityAsync(location);
        //    }
        //    // if listcard was used in previos step just go to the next step
        //    return await stepContext.NextAsync();
        //}

        //private async Task<DialogTurnResult> PromptWithAdaptiveCardAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        //{
        //    // Define choices
        //    var choices = new[] { "One", "Two", "Three" };

        //    // Create card
        //    var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
        //    {
        //        // Use LINQ to turn the choices into submit actions
        //        Actions = choices.Select(choice => new AdaptiveSubmitAction
        //        {
        //            Title = choice,
        //            Data = choice,  // This will be a string
        //        }).ToList<AdaptiveAction>(),
        //    };

        //    // Prompt
        //    return await stepContext.PromptAsync(
        //        "CHOICEPROMPT",
        //        new PromptOptions
        //        {
        //            Prompt = (Activity)MessageFactory.Attachment(new Attachment
        //            {
        //                ContentType = AdaptiveCard.ContentType,
        //                // Convert the AdaptiveCard to a JObject
        //                Content = JObject.FromObject(card),
        //            }),
        //            Choices = ChoiceFactory.ToChoices(choices), // Don't render the choices outside the card
        //            Style = ListStyle.None,
        //        },
        //        cancellationToken);
        //}

        //private async Task SendValueToDialogAsync(ITurnContext turnContext,   CancellationToken cancellationToken)
        //{
        //    // Serialize value
        //    var json = JsonConvert.SerializeObject(turnContext.Activity.Value);
        //    // Assign the serialized value to the turn context's activity
        //    turnContext.Activity.Text = json;
        //    // Create a dialog context
        //    var dc = await _dialogSet.CreateContextAsync(turnContext, cancellationToken);
        //    // Continue the dialog with the modified activity
        //    await dc.ContinueDialogAsync(cancellationToken);
        //}

    }
}
