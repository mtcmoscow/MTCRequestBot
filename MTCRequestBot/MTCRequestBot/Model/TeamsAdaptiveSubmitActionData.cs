﻿// <copyright file="TeamsAdaptiveSubmitActionData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace MTCRequestBot.Model
{
    using Microsoft.Bot.Schema;
    using Newtonsoft.Json;

    /// <summary>
    /// Defines Teams-specific behavior for an adaptive card submit action.
    /// </summary>
    public class TeamsAdaptiveSubmitActionData
    {
        /// <summary>
        /// Gets or sets the Teams-specific action.
        /// </summary>
        [JsonProperty("msteams")]
        public CardAction MsTeams { get; set; }
    }
}
