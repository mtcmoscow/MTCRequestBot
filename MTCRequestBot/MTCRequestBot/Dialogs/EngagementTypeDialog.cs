using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using MTCRequestBot.Cards;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MTCRequestBot.Dialogs
{
    /// <summary>
    /// Shows list card for engagement types if a user is on desktop/web.
    /// </summary>
    public class EngagementTypeDialog : CancelAndHelpDialog
    {
        public EngagementTypeDialog(string id = null)
            : base(id ?? nameof(EngagementTypeDialog))
        {
            //AddDialog(new DateTimePrompt(nameof(DateTimePrompt), DateTimePromptValidator));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                InitialStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var timex = (string)stepContext.Options;

            // list with icons
            var listItemsWithIcons = new Attachment()
            {
                ContentType = "application/vnd.microsoft.teams.card.list",
                Content = new ListCard()
                {
                    Title = "Select an engagement type:",
                    ListItems = new List<ListItemBase>(){
                        new ResultListItem()
                        {
                            Title = "Envisioning workshop",
                            Subtitle = "Help your customer comprehensively identify and prioritize their business problems, solutions and opportunities for innovation.",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/EnvisioningWorkshop.png",
                            Tap = new CardAction() { Type = ActionTypes.ImBack, Value = "Envisioning workshop"}
                        },
                        new ResultListItem()
                        {
                            Title = "Strategy briefing",
                            Subtitle = "Dive into your customer's goals and challenges through strategic and technical discussion.",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/StrategyBriefing.png",
                            Tap = new CardAction() { Type = ActionTypes.ImBack, Value = "Strategy briefing"}
                        },
                        new ResultListItem()
                        {
                            Title = "Architecture design session",
                            Subtitle = "Show your customer how their ambitions can be achieved with a customized Microsoft solutions architecture.",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/ads.png"
                        },
                        new ResultListItem()
                        {
                            Title = "Rapid prototype",
                            Subtitle = "Build out aspects of your client's proposed solution in an in-depth, hands-on engagement.",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/rapidprototype.png"
                        },
                        new ResultListItem()
                        {
                            Title = "Hackathon",
                            Subtitle = "Enable your customer to collaborate in a rapid, iterative fashion with the experts in applying creative technology solutions to business challenges.",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/hackathon.png"
                        },
                        new ResultListItem()
                        {
                            Title = "Hands-on lab",
                            Subtitle = "Get your customer hands-on with the technology in immersive exploration sessions including step-by-step  walkthroughs.",
                            Icon = $"https://mtcrequestbot20200907221020.azurewebsites.net/hol.png"
                        }
                    }
                }
            };

            var responseActivityList2 = stepContext.Context.Activity.CreateReply();
            responseActivityList2.TextFormat = TextFormatTypes.Markdown;
            responseActivityList2.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            responseActivityList2.Attachments.Add(listItemsWithIcons);
            await stepContext.Context.SendActivityAsync(responseActivityList2);


            var attachmentList = new Attachment()
            {
                ContentType = ListCard.ContentType,
                Content = new ListCard
                {
                    Title = "Select MTC-Lead TA:",
                    ListItems = new List<ListItemBase>()
                        {
                            //new SectionListItem() { Title = "MTC Director" },
                            //new PersonListItem() {ID = "olegka@microsoft.com", Title = "Oleg Karacharov", Subtitle = "MTC Director - Russia, Moscow", Tap = new CardAction() { Type = ActionTypes.ImBack, Value = "whois gsheldon@microsoft.com"} },
                            //new SectionListItem() { Title = "MTC Moscow TAs" },
                            new PersonListItem() {ID = "mibon@microsoft.com", Title = "Mikhail Bondarevsky", Subtitle = "Azure App Dev, GitHub, Teams Platform, Power Platform", Tap = new CardAction() { Type = ActionTypes.ImBack, Value = "whois mibon@microsoft.com"} },
                            new PersonListItem() {ID = "ivabu@microsoft.com", Title = "Ivan Budylin", Subtitle = "Modern Workplace, Security", Tap = new CardAction() { Type = ActionTypes.ImBack, Value = "whois ivabu@microsoft.com"} },
                            new PersonListItem() {ID = "Anna.Sviridova@microsoft.com", Title = "Anna Sviridova", Subtitle = "Power BI, AI, SQL Server", Tap = new CardAction() { Type = ActionTypes.ImBack, Value = "whois Anna.Sviridova@microsoft.com"} },
                            //new PersonListItem() {ID = "v-seziuz@microsoft.com", Title = "Sergei Ziuzev", Subtitle = "Briefing Coordinator, IoT", Tap = new CardAction() { Type = ActionTypes.ImBack, Value = "whois vchawla@microsoft.com"} }
                        },
                    //Buttons = new List<CardAction>()
                    //    {
                    //        new CardAction() { Title = "Select", Type = ActionTypes.ImBack, Value = "whois" },
                    //        new CardAction() { Title = "MessageBack", Type = ActionTypes.MessageBack, DisplayText = "display text", Value = "{\"invokeKey\":\"invokeValue\"}" }
                    //    },
                }
            };
            var responseActivityList = stepContext.Context.Activity.CreateReply();
            responseActivityList.TextFormat = TextFormatTypes.Markdown;
            responseActivityList.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            responseActivityList.Attachments.Add(attachmentList);
            await stepContext.Context.SendActivityAsync(responseActivityList);

            var confirmationCard = CreateAdaptiveCardAttachment();
            var response = MessageFactory.Attachment(confirmationCard, ssml: "MTC Request Confirmation Screen");
            await stepContext.Context.SendActivityAsync(response, cancellationToken);

            //var promptMessage = MessageFactory.Text(PromptMsgText, PromptMsgText, InputHints.ExpectingInput);
            //var repromptMessage = MessageFactory.Text(RepromptMsgText, RepromptMsgText, InputHints.ExpectingInput);

            //if (timex == null)
            //{
            //    // We were not given any date at all so prompt the user.
            //    return await stepContext.PromptAsync(nameof(DateTimePrompt),
            //        new PromptOptions
            //        {
            //            Prompt = promptMessage,
            //            RetryPrompt = repromptMessage,
            //        }, cancellationToken);
            //}

            // We have a Date we just need to check it is unambiguous.
            //var timexProperty = new TimexProperty(timex);
            //if (!timexProperty.Types.Contains(Constants.TimexTypes.Definite))
            //{
            //    // This is essentially a "reprompt" of the data we were given up front.
            //    return await stepContext.PromptAsync(nameof(DateTimePrompt),
            //        new PromptOptions
            //        {
            //            Prompt = repromptMessage,
            //        }, cancellationToken);
            //}

            return await stepContext.NextAsync(new List<DateTimeResolution> { new DateTimeResolution { Timex = timex } }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var timex = ((List<DateTimeResolution>)stepContext.Result)[0].Timex;
            return await stepContext.EndDialogAsync(timex, cancellationToken);
        }


        // Load attachment from embedded resource.
        private Attachment CreateAdaptiveCardAttachment()
        {
            var cardResourcePath = GetType().Assembly.GetManifestResourceNames().First(name => name.EndsWith("MTCRequestConfirmationScreen.json"));

            using (var stream = GetType().Assembly.GetManifestResourceStream(cardResourcePath))
            {
                using (var reader = new StreamReader(stream))
                {
                    var adaptiveCard = reader.ReadToEnd();
                    return new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(adaptiveCard),
                    };
                }
            }
        }
    }
}
