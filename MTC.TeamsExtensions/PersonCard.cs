using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTCRequestBot.Cards
{
    /// <summary>
    /// Content type for <see cref="PersonCard" />
    /// </summary>
    public class PersonCard
    {
        /// <summary>
        /// Content type to be used in the type property.
        /// </summary>
        public const string ContentType = "application/vnd.microsoft.teams.card.profile";



        /// <summary>
        /// Initializes a new instance of the PersonCard class.
        /// </summary>
        /// <param name="upn"></param>
        /// <param name="buttons">
        /// Set of actions applicable to the current
        /// card
        /// </param>
        public PersonCard(string upn = default(string),
            string jobTitle = default(string),
            IList<CardAction> buttons = default(IList<CardAction>))
        {
            Upn = upn;
            Buttons = buttons;
            JobTitle = jobTitle;
        }



        /// <summary>
        /// Gets or sets UPN of the person
        /// </summary>
        [JsonProperty(PropertyName = "upn")]
        public string Upn { get; set; }

        [JsonProperty(PropertyName = "jobTitle")]
        public string JobTitle { get; set; }


        /// <summary>
        /// Gets or sets set of actions applicable to the current card
        /// </summary>
        [JsonProperty(PropertyName = "buttons")]
        public IList<CardAction> Buttons { get; set; }

    }
}
