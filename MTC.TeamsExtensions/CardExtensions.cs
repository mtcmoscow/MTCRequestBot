using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTCRequestBot.Cards
{
    public static class CardExtensions
    {
        /// <summary>
        ///  Creates a new attachment from <see cref="PersonCard" />.
        /// </summary>
        /// <param name="card"> The instance of <see cref="PersonCard" />.</param>
        /// <returns> The generated attachment.</returns>
        public static Attachment ToAttachment(this PersonCard card)
        {
            return new Attachment
            {
                Content = card,
                ContentType = PersonCard.ContentType
            };
        }
    }
}
