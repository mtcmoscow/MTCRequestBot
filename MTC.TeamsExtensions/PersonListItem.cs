using Microsoft.Bot.Schema;
using System;

namespace MTCRequestBot.Cards
{
    public class PersonListItem : ListItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonListItem" /> class
        /// </summary>
        /// <param name="upn">Person UPN</param>
        /// <param name="name">Person name</param>
        /// <param name="subtitle">Person job title and department</param>
        /// <param name="tap">Tap action</param>
        public PersonListItem(string upn = default(string), string name = default(string), string subtitle = default(string), CardAction tap = default(CardAction))
            : base(upn, name, subtitle, tap)
        {
        }

        /// <summary>
        /// Gets or sets Item type - Person
        /// </summary>
        public override string Type
        {
            get
            {
                return "person";
            }

            //set
            //{
            //    throw new NotImplementedException();
            //}
        }
    }
}
