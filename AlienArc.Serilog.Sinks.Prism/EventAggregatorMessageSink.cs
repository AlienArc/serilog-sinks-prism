using Prism.Events;
using Serilog.Core;
using Serilog.Events;
using System;

namespace AlienArc.Serilog.Sinks.Prism
{
    public class EventAggregatorMessageSink : ILogEventSink
    {
        public IEventAggregator EventAggregator { get; }

        public EventAggregatorMessageSink(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        public void Emit(LogEvent logEvent)
        {
            var messageEvent = EventAggregator.GetEvent<EventAggregatorMessageEvent>();
            messageEvent.Publish(logEvent.RenderMessage());
        }
    }
}
