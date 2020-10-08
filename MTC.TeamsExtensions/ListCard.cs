using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTCRequestBot.Cards
{
    public class ListCard
    {
        /// <summary>
        /// Content type of List Card
        /// </summary>
        public const string ContentType = "application/vnd.microsoft.teams.card.list";

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCard"/> class.
        /// </summary>
        public ListCard()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCard" /> class
        /// </summary>
        /// <param name="title">Card title</param>
        /// <param name="items">Items of card</param>
        /// <param name="buttons">Buttons of card</param>
        public ListCard(string title = default(string), IList<ListItemBase> items = default(IList<ListItemBase>), IList<CardAction> buttons = default(IList<CardAction>))
        {
            this.Title = title;
            this.ListItems = items;
            this.Buttons = buttons;
        }

        /// <summary>
        /// Gets or sets Card title
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets Array of item
        /// </summary>
        [JsonProperty(PropertyName = "items")]
        public IList<ListItemBase> ListItems { get; set; }

        /// <summary>
        /// Gets or sets Set of actions applicable to the current card
        /// </summary>
        [JsonProperty(PropertyName = "buttons")]
        public IList<CardAction> Buttons { get; set; }
    }
}
