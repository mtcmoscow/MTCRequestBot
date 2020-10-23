using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTCWhoBotPrototype.Translation
{
    /// <summary>
    /// Array of translated results from Translator API v3.
    /// </summary>
    internal class TranslatorResponse
    {
        [JsonProperty("translations")]
        public IEnumerable<TranslatorResult> Translations { get; set; }
    }
}
