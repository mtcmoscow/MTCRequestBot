using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MTCWhoBotPrototype.Helpers
{
    public static class TeamsHelper
    {
        /// <summary>
        /// Based on deep link URL received find team id and set it.
        /// </summary>
        /// <param name="teamIdDeepLink">Deep link to get the team id.</param>
        /// <returns>A team id from the deep link URL.</returns>
        private static string ParseTeamIdFromDeepLink(string teamIdDeepLink)
        {
            // team id regex match
            // for a pattern like https://teams.microsoft.com/l/team/19%3a64c719819fb1412db8a28fd4a30b581a%40thread.tacv2/conversations?groupId=53b4782c-7c98-4449-993a-441870d10af9&tenantId=72f988bf-86f1-41af-91ab-2d7cd011db47
            // regex checks for 19%3a64c719819fb1412db8a28fd4a30b581a%40thread.tacv2
            var match = System.Text.RegularExpressions.Regex.Match(teamIdDeepLink, @"teams.microsoft.com/l/team/(\S+)/");
            if (!match.Success)
            {
                throw new ArgumentException($"Invalid team found.");
            }

            return System.Web.HttpUtility.UrlDecode(match.Groups[1].Value);
        }


        /// <summary>
        /// Send the given attachment to the specified team.
        /// </summary>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="cardToSend">The card to send.</param>
        /// <param name="teamId">Team id to which the message is being sent.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns><see cref="Task"/>That resolves to a <see cref="ConversationResourceResponse"/>Send a attachemnt.</returns>
        public static async Task<ConversationResourceResponse> SendCardToTeamAsync(
            ITurnContext turnContext,
            Attachment cardToSend,
            string teamId,
            CancellationToken cancellationToken)
        {
            var conversationParameters = new ConversationParameters
            {
                Activity = (Activity)MessageFactory.Attachment(cardToSend),
                ChannelData = new TeamsChannelData { Channel = new ChannelInfo(teamId) },
            };

            MicrosoftAppCredentials.TrustServiceUrl(turnContext.Activity.ServiceUrl);

            var microsoftAppCredentials = new MicrosoftAppCredentials("dc03a5a1-d2bc-48d9-959d-8a34a3d9685b",
                        "Pj_Iy8Mb_Vnoa0.WHkWW3lil~80G5--bY4");

            var taskCompletionSource = new TaskCompletionSource<ConversationResourceResponse>();
            await ((BotFrameworkAdapter)turnContext.Adapter).CreateConversationAsync(
                null,       // If we set channel = "msteams", there is an error as preinstalled middleware expects ChannelData to be present.
                turnContext.Activity.ServiceUrl,
                microsoftAppCredentials,
                conversationParameters,
                (newTurnContext, newCancellationToken) =>
                {
                    var activity = newTurnContext.Activity;
                    taskCompletionSource.SetResult(new ConversationResourceResponse
                    {
                        Id = activity.Conversation.Id,
                        ActivityId = activity.Id,
                        ServiceUrl = activity.ServiceUrl,
                    });
                    return Task.CompletedTask;
                },
                cancellationToken).ConfigureAwait(false);

            return await taskCompletionSource.Task.ConfigureAwait(false);
        }

        /// <summary>
        /// Send the given attachment to the specified team.
        /// </summary>
        /// <param name="teamPreferenceEntity">Team preference model object.</param>
        /// <param name="cardToSend">The attachment card to send.</param>
        /// <param name="serviceUrl">Service URL for a particular team.</param>
        /// <returns>A task that sends notification card in channel.</returns>
        public static async Task SendCardToTeamAsync(
            string teamId,
            Attachment cardToSend,
            string serviceUrl,
            BotFrameworkAdapter adapter
            )
        {
            MicrosoftAppCredentials.TrustServiceUrl(serviceUrl);
            string teamsChannelId = teamId;

            var conversationReference = new ConversationReference()
            {
                ChannelId = "msteams",// Constants.TeamsBotFrameworkChannelId,
                Bot = new ChannelAccount() { Id = $"28:dc03a5a1-d2bc-48d9-959d-8a34a3d9685b" },
                ServiceUrl = serviceUrl,
                Conversation = new ConversationAccount() { Id = teamsChannelId },
            };

            //this.logger.LogInformation($"sending notification to channelId- {teamsChannelId}");

            // Retry it in addition to the original call.
            //await this.retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    await adapter.ContinueConversationAsync(
                        "dc03a5a1-d2bc-48d9-959d-8a34a3d9685b",
                        conversationReference,
                        async (conversationTurnContext, conversationCancellationToken) =>
                        {
                            await conversationTurnContext.SendActivityAsync(MessageFactory.Attachment(cardToSend));
                        },
                        CancellationToken.None);
                }
                catch (Exception ex)
                {
                    //this.logger.LogError(ex, $"Error while performing retry logic to send digest notification to channel for team: {teamsChannelId}.");
                    throw;
                }
            };
        }
    }
}
