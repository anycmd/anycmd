
namespace Anycmd.Events.Storage
{
    using Events;
    using Serialization;
    using System;
    using System.IO;
    using Util;

    public static class AcDomainExtension
    {
        /// <summary>
        /// Creates and initializes the domain event data object from the given domain event.
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="entity">The domain event instance from which the domain event data object
        /// is created and initialized.</param>
        /// <returns>The initialized data object instance.</returns>
        public static DomainEventDataObject FromDomainEvent(this IAcDomain acDomain, IDomainEvent entity)
        {
            var serializer = acDomain.RetrieveRequiredService<IDomainEventSerializer>();
            var obj = new DomainEventDataObject
            {
                Branch = entity.Branch,
                Data = serializer.Serialize(entity),
                Id = entity.Id,
                AssemblyQualifiedEventType =
                    string.IsNullOrEmpty(entity.AssemblyQualifiedEventType)
                        ? entity.GetType().AssemblyQualifiedName
                        : entity.AssemblyQualifiedEventType,
                Timestamp = entity.Timestamp,
                Version = entity.Version,
                SourceId = entity.Source.Id,
                AssemblyQualifiedSourceType = entity.Source.GetType().AssemblyQualifiedName
            };

            return obj;
        }

        /// <summary>
        /// Converts the domain event data object to its corresponding domain event entity instance.
        /// </summary>
        /// <returns>The domain event entity instance that is converted from the current domain event data object.</returns>
        public static IDomainEvent ToDomainEvent(this IAcDomain acDomain, DomainEventDataObject from)
        {
            if (string.IsNullOrEmpty(from.AssemblyQualifiedEventType))
                throw new InvalidDataException("form.AssemblyQualifiedTypeName");
            if (from.Data == null || from.Data.Length == 0)
                throw new InvalidDataException("Data");

            var serializer = acDomain.RetrieveRequiredService<IDomainEventSerializer>();
            var type = Type.GetType(from.AssemblyQualifiedEventType);
            var ret = (IDomainEvent)serializer.Deserialize(type, from.Data);

            return ret;
        }
    }
}
