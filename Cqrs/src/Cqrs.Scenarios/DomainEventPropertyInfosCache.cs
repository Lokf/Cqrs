using SoderbergPartners.Kalle.Cqrs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SoderbergPartners.Kalle.Cqrs.Scenarios
{
    /// <summary>
    /// Cache for property infos of domain events.
    /// </summary>
    internal class DomainEventPropertyInfosCache
    {
        private Dictionary<Type, PropertyInfo[]> cache = new Dictionary<Type, PropertyInfo[]>();
        private object cacheLock = new object();

        /// <summary>
        /// Gets the property infos of the domain event.
        /// </summary>
        /// <param name="domainEventType">The domain event type.</param>
        /// <returns>The </returns>
        public PropertyInfo[] GetPropertyInfos(Type domainEventType)
        {
            if (cache.TryGetValue(domainEventType, out var propertyInfos))
            {
                return propertyInfos;
            }

            lock (cacheLock)
            {
                if (cache.TryGetValue(domainEventType, out propertyInfos))
                {
                    return propertyInfos;
                }

                var bindingFlags = BindingFlags.Public | BindingFlags.Instance;
                propertyInfos = domainEventType
                    .GetProperties(bindingFlags)
                    .Where(propertyInfo => propertyInfo.Name != nameof(IDomainEvent.EventId))
                    .ToArray();
                cache[domainEventType] = propertyInfos;

                return propertyInfos;
            }
        }
    }
}
