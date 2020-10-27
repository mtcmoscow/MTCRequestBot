// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;

namespace BotWithSentimentAnalysisAndKeyVault
{
    public class EmptyBot : ActivityHandler
    {
       //private static readonly AzureKeyCredential credentials = new AzureKeyCredential("877ceee917f74c98bac1a6f2955c1fa9");
       //private static readonly Uri endpoint = new Uri("https://sentimentcognitiveservices.cognitiveservices.azure.com/");

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello world!"), cancellationToken);
                }
            }
        }

        private readonly IConfiguration _configuration;
        public EmptyBot(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            string userInput = turnContext.Activity.Text;
            var client = new TextAnalyticsClient(
                new Uri(_configuration["CognitiveServicesEndpoint"]),
                new AzureKeyCredential(_configuration["AzureKeyCredential"]));
            var sentiment = client.AnalyzeSentiment(userInput).Value.Sentiment;

            static string GetReplyText(TextSentiment sentiment) => sentiment switch
            {
                TextSentiment.Positive => "I am Gruut.",
                TextSentiment.Negative => "I AM GRUUUUUTTT!!",
                TextSentiment.Neutral => "I am Gruut?",
                _ => "I. AM. GRUUUUUT"
            };

            var replyText = GetReplyText(sentiment);
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);            
        }

        
    }
}
