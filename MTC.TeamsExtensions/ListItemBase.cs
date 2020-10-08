using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MTCRequestBot.Cards
{
    public abstract class ListItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListItemBase" /> class
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <param name="title">Item title</param>
        /// <param name="subtitle">Item subtitle</param>
        /// <param name="tap">Item tap action</param>
        /// <param name="icon">Item icon</param>
        public ListItemBase(string id = default(string), string title = default(string), string subtitle = default(string), CardAction tap = default(CardAction), string icon = default(string))
        {
            this.ID = id;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Tap = tap;
            this.Icon = icon;
        }

        /// <summary>
        /// Gets or sets Item type
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public abstract string Type { get; }

        /// <summary>
        /// Gets or sets Item ID
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets Item title
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets Item subtitle
        /// </summary>
        [JsonProperty(PropertyName = "subtitle")]
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets or sets Itam tap action
        /// </summary>
        [JsonProperty(PropertyName = "tap")]
        public CardAction Tap { get; set; }

        /// <summary>
        /// Gets or sets Itam icon
        /// </summary>
        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }
    }

    public class SectionListItem : ListItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SectionListItem" /> class
        /// </summary>
        /// <param name="title">Section title</param>
        public SectionListItem(string title = default(string))
            : base(title)
        {
        }

        /// <summary>
        /// Gets or sets Item type - Section
        /// </summary>
        public override string Type
        {
            get
            {
                return "section";
            }

            //set
            //{
            //    throw new NotImplementedException();
            //}
        }
    }

    /// <summary>
    /// List item - ResultItem
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ResultListItem : ListItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultListItem" /> class
        /// </summary>
        /// <param name="title">ResultItem Title</param>
        /// <param name="subtitle">ResultTitle Subtitle</param>
        /// <param name="icon">ResultItem icon url</param>
        /// <param name="tap">Tap action</param>
        public ResultListItem(string title = default(string), 
            string subtitle = default(string),
            string icon = default(string), 
            CardAction tap = default(CardAction))
            : base(title, subtitle, icon, tap)
        {
        }

        /// <summary>
        /// Gets or sets Item type - ResultItem
        /// </summary>
        public override string Type
        {
            get
            {
                return "resultItem";
            }

            //set
            //{
            //    Type = value;
            //}
        }
    }
}
