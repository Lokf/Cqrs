using Newtonsoft.Json;
using System.Collections.Generic;

namespace SoderbergPartners.Kalle.Cqrs.Domain
{
    /// <summary>
    /// Key-value stored metadata for events.
    /// </summary>
    public class Metadata : Dictionary<string, string>
    {
        /// <summary>
        /// Gets the event name.
        /// </summary>
        [JsonIgnore]
        public string EventName => this[MetadataKeys.EventName];
    }
}
