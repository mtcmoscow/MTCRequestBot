using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTCRequestBot
{
    /// <summary>
    /// Represents the user rating given for feedback.
    /// </summary>
    public enum FeedbackRating
    {
        /// <summary>
        /// Not helpful.
        /// </summary>
        NotHelpful,

        /// <summary>
        /// Needs improvement.
        /// </summary>
        NeedsImprovement,

        /// <summary>
        /// Helpful.
        /// </summary>
        Helpful,
    }
}
