
namespace MTCRequestBot.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Schema.Teams;
    using MTCRequestBot.Cards;
    using MTCRequestBot.Model;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;


    public class AdaptiveCardHelper
    {
        /// <summary>
        /// Helps to get the expert submit card.
        /// </summary>
        /// <param name="message">A message in a conversation.</param>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        public static async Task<Attachment> ShareFeedbackSubmitText(
            IMessageActivity message,
            ITurnContext<IMessageActivity> turnContext,
            CancellationToken cancellationToken)
        {
            var shareFeedbackSubmitTextPayload = ((JObject)message.Value).ToObject<ShareFeedbackCardPayload>();

            // Validate required fields.
            if (!Enum.TryParse(shareFeedbackSubmitTextPayload?.Rating, out FeedbackRating rating))
            {
                var updateCardActivity = new Activity(ActivityTypes.Message)
                {
                    Id = turnContext.Activity.ReplyToId,
                    Conversation = turnContext.Activity.Conversation,
                    Attachments = new List<Attachment> { ShareFeedbackCard.GetCard(shareFeedbackSubmitTextPayload) },
                };
                await turnContext.UpdateActivityAsync(updateCardActivity, cancellationToken).ConfigureAwait(false);
                return null;
            }
            return null;
            //var teamsUserDetails = await GetUserDetailsInPersonalChatAsync(turnContext, cancellationToken).ConfigureAwait(false);
            //return SmeFeedbackCard.GetCard(shareFeedbackSubmitTextPayload, teamsUserDetails);
        }
    }
}
