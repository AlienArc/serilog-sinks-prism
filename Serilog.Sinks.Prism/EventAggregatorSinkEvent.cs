using Prism.Events;

namespace Serilog.Sinks.Prism
{
    public class EventAggregatorSinkEvent : PubSubEvent<string>
    {
        public EventAggregatorSinkEvent()
        {
        }
    }
}
