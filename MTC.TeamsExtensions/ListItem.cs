// <copyright file="ListItem.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>



namespace Microsoft.Teams.Apps.SubmitIdea.Models.Card
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// A class that represents list card model.
    /// </summary>
    public class ListCard
    {
        /// <summary>
        /// Gets or sets title of list card.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets items of list card.
        /// </summary>
        [JsonProperty("items")]
#pragma warning disable CA2227 // Getting error to make collection property as read only but needs to assign values.
        public List<ListItem> Items { get; set; }
    }
}


// <copyright file="ListCard.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SubmitIdea.Models.Card
{
    using Microsoft.Bot.Schema;
    using Newtonsoft.Json;

    /// <summary>
    /// A class that represent the list item model.
    /// </summary>
    public class ListItem
    {
        /// <summary>
        /// Gets or sets type of item on list card.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets unique id of the item on list card.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets title of the item on list card.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets subtitle of the item on list card.
        /// </summary>
        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets or sets icon for item on list card.
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets Itam tap action
        /// </summary>
        [JsonProperty(PropertyName = "tap")]
        public CardAction Tap { get; set; }
    }
}