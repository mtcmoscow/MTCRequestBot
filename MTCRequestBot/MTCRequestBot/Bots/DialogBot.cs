// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.10.2

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using MTCRequestBot.Cards;
using MTCRequestBot.Helpers;
using MTCRequestBot.Model;
using MTCRequestBot.Resources;
using Newtonsoft.Json.Linq;

namespace MTCRequestBot.Bots
{
    // This IBot implementation can run any type of Dialog. The use of type parameterization is to allows multiple different bots
    // to be run at different endpoints within the same project. This can be achieved by defining distinct Controller types
    // each with dependency on distinct IBot types, this way ASP Dependency Injection can glue everything together without ambiguity.
    // The ConversationState is used by the Dialog system. The UserState isn't, however, it might have been used in a Dialog implementation,
    // and the requirement is that all BotState objects are saved at the end of a turn.
    public class DialogBot<T> : Microsoft.Bot.Builder.Teams.TeamsActivityHandler
        where T : Dialog
    {
        protected readonly Dialog Dialog;
        protected readonly BotState ConversationState;
        protected readonly BotState UserState;
        protected readonly ILogger Logger;

        /// <summary>
        /// Represents the conversation type as personal.
        /// </summary>
        private const string ConversationTypePersonal = "personal";

        public DialogBot(ConversationState conversationState, UserState userState, T dialog, ILogger<DialogBot<T>> logger)
        {
            ConversationState = conversationState;
            UserState = userState;
            Dialog = dialog;
            Logger = logger;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have occured during the turn.
            await ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await UserState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        /// <summary>
        /// Invoked when a message activity is received from the user.
        /// </summary>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        /// <remarks>
        /// Reference link: https://docs.microsoft.com/en-us/dotnet/api/microsoft.bot.builder.activityhandler.onmessageactivityasync?view=botbuilder-dotnet-stable.
        /// </remarks>
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, 
            CancellationToken cancellationToken)
        {
            Logger.LogInformation("Running dialog with Message Activity.");

            var message = turnContext?.Activity;
            Logger.LogInformation("$from: {message.From?.Id}, conversation: {message.Conversation.Id}, replyToId: {message.ReplyToId}");

            //switch (message.Conversation.ConversationType.ToLower())
            //{
            //    case ConversationTypePersonal:
            //        await this.OnMessageActivityInPersonalChatAsync(message, turnContext, cancellationToken).ConfigureAwait(false);
            //        break;
            //}

            // Run the Dialog with the new message Activity.
            await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>("DialogState"), cancellationToken);
        }

        /// <summary>
        /// Handle message activity in 1:1 chat.
        /// </summary>
        /// <param name="message">A message in a conversation.</param>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        private async Task OnMessageActivityInPersonalChatAsync(
            IMessageActivity message,
            ITurnContext<IMessageActivity> turnContext,
            CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(message.ReplyToId) && (message.Value != null) && ((JObject)message.Value).HasValues)
            {
                Logger.LogInformation("Card submit in 1:1 chat");
                await this.OnAdaptiveCardSubmitInPersonalChatAsync(message, turnContext, cancellationToken).ConfigureAwait(false);
                return;
            }

            string text = message.Text?.ToLower()?.Trim() ?? string.Empty;

            switch (text)
            {
                //case Constants.AskAnExpert:
                //    this.logger.LogInformation("Sending user ask an expert card");
                //    await turnContext.SendActivityAsync(MessageFactory.Attachment(AskAnExpertCard.GetCard())).ConfigureAwait(false);
                //    break;

                case "share feedback": // Constants.ShareFeedback:
                    Logger.LogInformation("Sending user feedback card");
                    await turnContext.SendActivityAsync(MessageFactory.Attachment(ShareFeedbackCard.GetCard())).ConfigureAwait(false);
                    break;

                //case Constants.TakeATour:
                //    this.logger.LogInformation("Sending user tour card");
                //    var userTourCards = TourCarousel.GetUserTourCards(this.appBaseUri);
                //    await turnContext.SendActivityAsync(MessageFactory.Carousel(userTourCards)).ConfigureAwait(false);
                //    break;

                default:
                    Logger.LogInformation("Sending input to QnAMaker");
                    //await this.GetQuestionAnswerReplyAsync(turnContext, text).ConfigureAwait(false);
                    break;
            }
        }


        /// <summary>
        /// Handle adaptive card submit in 1:1 chat.
        /// Submits the question or feedback to the SME team.
        /// </summary>
        /// <param name="message">A message in a conversation.</param>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        private async Task OnAdaptiveCardSubmitInPersonalChatAsync(
            IMessageActivity message,
            ITurnContext<IMessageActivity> turnContext,
            CancellationToken cancellationToken)
        {
            Attachment smeTeamCard = null;      // Notification to SME team
            Attachment userCard = null;         // Acknowledgement to the user
            //TicketEntity newTicket = null;      // New ticket

            switch (message?.Text)
            {
                //    case Constants.AskAnExpert:
                //        this.logger.LogInformation("Sending user ask an expert card (from answer)");
                //        var askAnExpertPayload = ((JObject)message.Value).ToObject<ResponseCardPayload>();
                //        await turnContext.SendActivityAsync(MessageFactory.Attachment(AskAnExpertCard.GetCard(askAnExpertPayload))).ConfigureAwait(false);
                //        break;

                case "share feedback": // Constants.ShareFeedback:
                    //this.logger.LogInformation("Sending user share feedback card (from answer)");
                    var shareFeedbackPayload = ((JObject)message.Value).ToObject<ResponseCardPayload>();
                    await turnContext.SendActivityAsync(MessageFactory.Attachment(ShareFeedbackCard.GetCard(shareFeedbackPayload))).ConfigureAwait(false);
                    break;              

                case ShareFeedbackCard.ShareFeedbackSubmitText:
                    //this.logger.LogInformation("Received app feedback");
                    smeTeamCard = await AdaptiveCardHelper.ShareFeedbackSubmitText(message, turnContext, cancellationToken).ConfigureAwait(false);
                    if (smeTeamCard != null)
                    {
                        await turnContext.SendActivityAsync(MessageFactory.Text(Strings.ThankYouTextContent)).ConfigureAwait(false);
                    }

                    break;

                default:
                    //this.logger.LogInformation($"Unexpected text in submit payload: {message.Text}", SeverityLevel.Warning);
                    break;
            }
            

            // Send acknowledgment to the user
            if (userCard != null)
            {
                await turnContext.SendActivityAsync(MessageFactory.Attachment(userCard), cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
