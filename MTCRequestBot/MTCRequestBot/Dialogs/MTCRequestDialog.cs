using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MTCRequestBot.Dialogs
{
    public class MTCRequestDialog: CancelAndHelpDialog
    {
        public MTCRequestDialog() : base(nameof(MTCRequestDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new DateResolverDialog());
            AddDialog(new EngagementTypeDialog());

            AddDialog(
                new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
                {  
                    LocationStepAsync,
                    EngagementTypeStepAsync,
                    DateStepAsync,

                    ConfirmStepAsync,
                    FinalStepAsync
                }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> LocationStepAsync(WaterfallStepContext stepCtx, CancellationToken cancelToken)
        {
            var requestDetails = (MTCRequestDetail)stepCtx.Options;
            if (requestDetails.Location == null)
            {
                var promptMessage = MessageFactory.Text("В каком MTC?");
                return await stepCtx.PromptAsync(nameof(TextPrompt), 
                    new PromptOptions { Prompt = promptMessage }, cancelToken);
            }

            return await stepCtx.NextAsync(requestDetails.Location, cancelToken);
        }

        private async Task<DialogTurnResult> EngagementTypeStepAsync(WaterfallStepContext stepCtx, CancellationToken cancelToken)
        {
            var requestDetails = (MTCRequestDetail)stepCtx.Options;
            requestDetails.Location = (string)stepCtx.Result;

            if (requestDetails.EngementType == null)
            {
                //var promptMessage = MessageFactory.Text("Тип мероприятия:");
                //return await stepCtx.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancelToken);

                var choices = new[] { "sb", "ads", "hackathon" };

                var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
                {
                    Actions = choices.Select(choice => new AdaptiveSubmitAction
                    {
                        Title = choice,
                        Data = choice, //This will be a string
                    }).ToList<AdaptiveAction>(),
                };

                return await stepCtx.PromptAsync("CHOICEPROMPT", new PromptOptions
                {
                    Prompt = (Activity)MessageFactory.Attachment(new Attachment
                    {
                        ContentType = AdaptiveCard.ContentType,
                        Content = JObject.FromObject(card) // Convert the AdaptiveCard to a JObject
                    }),
                    Choices = ChoiceFactory.ToChoices(choices), 
                    Style = ListStyle.None // don't render the choices outside the card.
                }, cancelToken);

                //return await stepCtx.BeginDialogAsync(nameof(EngagementTypeDialog), requestDetails.EngementType, cancelToken);
            }

            return await stepCtx.NextAsync(requestDetails.EngementType, cancelToken);
        }

        private async Task<DialogTurnResult> DateStepAsync(WaterfallStepContext stepCtx, CancellationToken cancelToken)
        {
            var requestDetails = (MTCRequestDetail)stepCtx.Options;
            requestDetails.EngementType = (string)stepCtx.Result;

            if (requestDetails.Date == null || IsAmbiguous(requestDetails.Date))
            {
                return await stepCtx.BeginDialogAsync(nameof(DateResolverDialog), requestDetails.Date, cancelToken);
            }

            return await stepCtx.NextAsync(requestDetails.Date, cancelToken);
        }

        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepCtx, CancellationToken cancelToken)
        {
            var requestDetails = (MTCRequestDetail)stepCtx.Options;
            requestDetails.Date = (string)stepCtx.Result;

            var messageText = $"Please confirm: Engagement type: {requestDetails.EngementType}, Location: {requestDetails.Location} Date: {requestDetails.Date}. Is this correct?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepCtx.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions { Prompt = promptMessage }, cancelToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepCtx, CancellationToken cancelToken)
        {
            if ((bool)stepCtx.Result)
            {
                var requestDetails = (MTCRequestDetail)stepCtx.Options;
                return await stepCtx.EndDialogAsync(requestDetails, cancelToken);
            }

            return await stepCtx.EndDialogAsync(null, cancelToken);
        }

        private static bool IsAmbiguous(string timex)
        {
            var timexProperty = new TimexProperty(timex);
            return !timexProperty.Types.Contains(Constants.TimexTypes.Definite);
        }
    }
}
