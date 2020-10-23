using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTCWhoBotPrototype.Translation
{
    /// <summary>
    /// Translation result from Translator API v3.
    /// </summary>
    internal class TranslatorResult
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }
    }
}
