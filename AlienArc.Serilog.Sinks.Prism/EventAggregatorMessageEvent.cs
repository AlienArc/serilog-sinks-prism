using Prism.Events;

namespace AlienArc.Serilog.Sinks.Prism
{
    public class EventAggregatorMessageEvent : PubSubEvent<string>
    {
        public EventAggregatorMessageEvent()
        {
        }
    }
}
