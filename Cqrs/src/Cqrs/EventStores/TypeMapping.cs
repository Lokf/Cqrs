using System;
using System.Collections.Generic;

namespace SoderbergPartners.Kalle.Cqrs.EventStores
{
    /// <summary>
    /// Provides a mapping from the full name of the type to the actual type.
    /// </summary>
    public class TypeMapping
    {
        private readonly Dictionary<string, Type> typeMappings = new Dictionary<string, Type>();

        /// <summary>
        /// Registers a mapping for the given type.
        /// </summary>
        /// <param name="type">The type.</param>
        public void RegisterType(Type type)
        {
            typeMappings.Add(type.FullName, type);
        }

        /// <summary>
        /// Gets the type with the given name.
        /// </summary>
        /// <param name="name">The full name of the type.</param>
        /// <returns>The type.</returns>
        public Type GetType(string name)
        {
            return typeMappings[name];
        }
    }
}
