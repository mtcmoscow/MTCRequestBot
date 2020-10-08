using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTCRequestBot.CognitiveModels
{
    /// <summary>
    /// Extends the partial MTCRequest class with methods and props that simplify accessing entities in the luis results.
    /// </summary>
    public partial class MTCRequest
    {
        public string Location => Entities?.MTCLocation?.FirstOrDefault()?.FirstOrDefault();
        public string EngagementType => Entities?.MTCEngagementType?.FirstOrDefault()?.FirstOrDefault();
        
        // This value will be a TIMEX. And we are only interested in a Date so grab the first result and drop the Time part.
        // TIMEX is a format that represents DateTime expressions that include some ambiguity. e.g. missing a Year.
        public string Date
            => Entities.datetime?.FirstOrDefault()?.Expressions.FirstOrDefault()?.Split('T')[0];
    }
}
