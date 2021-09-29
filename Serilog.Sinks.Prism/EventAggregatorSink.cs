using Prism.Events;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using System.IO;

namespace Serilog.Sinks.Prism
{
    public class EventAggregatorSink : ILogEventSink
    {
        public IEventAggregator EventAggregator { get; }
        public ITextFormatter TextFormatter { get; }

        public EventAggregatorSink(IEventAggregator eventAggregator, ITextFormatter textFormatter)
        {
            EventAggregator = eventAggregator;
            TextFormatter = textFormatter;
        }

        public void Emit(LogEvent logEvent)
        {
            var stringWritter = new StringWriter();
            TextFormatter.Format(logEvent, stringWritter);
            string message = stringWritter.GetStringBuilder().ToString();
            var messageEvent = EventAggregator.GetEvent<EventAggregatorSinkEvent>();
            messageEvent.Publish(message);
        }
    }
}
