﻿using System;
using System.ComponentModel.Composition;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace APLPromoter.Core
{

    namespace Reactive
    {
        [Export]
        [PartCreationPolicy(CreationPolicy.Shared)]
        public class EventAggregator : IEventAggregator
        {
            readonly ISubject<object> subject = new Subject<object>();

            public IObservable<TEvent> GetEvent<TEvent>()
            {
                return subject.OfType<TEvent>().AsObservable();
            }

            public void Publish<TEvent>(TEvent sampleEvent)
            {
                subject.OnNext(sampleEvent);
            }
        }

        public interface IEventAggregator
        {
            IObservable<TEvent> GetEvent<TEvent>();
            void Publish<TEvent>(TEvent sampleEvent);
        }
    }

    namespace Reactive.Events
    {
        public class ErrorExistsEvent
        {

        }

        public class WindowResizedEvent
        {

        }

    }

}
