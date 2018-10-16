using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using SoderbergPartners.Kalle.Cqrs.Domain;
using SoderbergPartners.Kalle.Cqrs.EventStores;
using SoderbergPartners.Kalle.Cqrs.Scenarios;
using SoderbergPartners.Kalle.Cqrs.SomeDomain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SoderbergPartners.Kalle.Cqrs.Tests.EventStores
{
    [TestClass]
    public class EventStoreTests
    {
        [TestMethod]
        public void Given_AnEvent_WhenItIsPersisted_Then_TheEventIsSerializedToJson()
        {
            IEnumerable<SerializedDomainEvent> serializedDomainEvents = null;
            var eventPersistence = new Mock<IEventPersistence>();
            eventPersistence
                .Setup(mock => mock.PersistEvents(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<List<SerializedDomainEvent>>()))
                .Callback<Guid, int, IEnumerable<SerializedDomainEvent>>((aggregateId, aggregateVersion, events) => serializedDomainEvents = events);

            var typeMapping = new TypeMapping();
            var domainEventSerializer = new DomainEventSerializer(typeMapping);

            var eventStore = new EventStore(eventPersistence.Object, domainEventSerializer);

            var domainEvents = new List<IDomainEvent>
            {
                new SomeOtherEvent(Guid.Empty, 10),
            };

            eventStore.SaveDomainEvents(Guid.Empty, 0, domainEvents);

            var serializedDomainEvent = serializedDomainEvents.Single();

            var expectedPayload = JToken.Parse(File.ReadAllText(@"EventStores\payload.json"));
            var actualPayload = JToken.Parse(serializedDomainEvent.Payload);
            Assert.IsTrue(JToken.DeepEquals(expectedPayload, actualPayload));
        }

        [TestMethod]
        public void Given_AnEvent_WhenItIsPersisted_Then_TheMetadataIsSerializedToJson()
        {
            IEnumerable<SerializedDomainEvent> serializedDomainEvents = null;
            var eventPersistence = new Mock<IEventPersistence>();
            eventPersistence
                .Setup(mock => mock.PersistEvents(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<List<SerializedDomainEvent>>()))
                .Callback<Guid, int, IEnumerable<SerializedDomainEvent>>((aggregateId, aggregateVersion, events) => serializedDomainEvents = events);

            var typeMapping = new TypeMapping();
            var domainEventSerializer = new DomainEventSerializer(typeMapping);

            var eventStore = new EventStore(eventPersistence.Object, domainEventSerializer);

            var domainEvents = new List<IDomainEvent>
            {
                new SomeOtherEvent(Guid.Empty, 10),
            };

            eventStore.SaveDomainEvents(Guid.Empty, 0, domainEvents);

            var serializedDomainEvent = serializedDomainEvents.Single();

            var expectedMetadata = JToken.Parse(File.ReadAllText(@"EventStores\metadata.json"));
            var actualMetadata = JToken.Parse(serializedDomainEvent.Metadata);
            Assert.IsTrue(JToken.DeepEquals(expectedMetadata, actualMetadata));
        }

        [TestMethod]
        public void Given_ASerializedEvent_When_TheDomainEventsAreLoaded_Then_TheEventsAreDeserialized()
        {
            var aggregateId = Guid.NewGuid();
            var metadata = File.ReadAllText(@"EventStores\metadata.json");
            var payload = File.ReadAllText(@"EventStores\payload.json");

            var serializedDomainEvents = new List<SerializedDomainEvent>
            {
                new SerializedDomainEvent
                {
                    AggregateId = aggregateId,
                    AggregateVersion = 1,
                    EventId = Guid.Empty,
                    Metadata = metadata,
                    Payload = payload,
                },
            };

            var eventPersistence = new Mock<IEventPersistence>();
            eventPersistence
                .Setup(mock => mock.GetEventsByAggregateId(aggregateId))
                .Returns(serializedDomainEvents);

            var typeMapping = new TypeMapping();
            typeMapping.RegisterType(typeof(SomeDomain.SomeOtherEvent));
            var domainEventSerializer = new DomainEventSerializer(typeMapping);

            var eventStore = new EventStore(eventPersistence.Object, domainEventSerializer);

            var actualEvents = eventStore
                .GetDomainEvents(aggregateId)
                .ToList();

            var expectedEvents = new List<IDomainEvent>
            {
                new SomeOtherEvent(Guid.Empty, 10),
            };

            var result = DomainEventsVerifier.VerifyEvents(expectedEvents, actualEvents);
            Assert.IsTrue(result.IsCorrect, result.Reason);
        }
    }
}
