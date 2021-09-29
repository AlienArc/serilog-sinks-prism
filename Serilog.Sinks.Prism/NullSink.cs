using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.Prism
{
    class NullSink : ILogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
        }
    }
}
