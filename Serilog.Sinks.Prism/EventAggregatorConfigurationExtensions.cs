using Prism.Events;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using System;
using System.Text;

namespace Serilog.Sinks.Prism
{
    public static class EventAggregatorConfigurationExtensions
    {
        const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static LoggerConfiguration EventAggregator(
            this LoggerSinkConfiguration sinkConfiguration,
            IEventAggregator eventAggregator,
            string outputTemplate = DefaultOutputTemplate,
            IFormatProvider formatProvider = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));
            if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));

            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);

            return ConfigureEventAggregator(sinkConfiguration.Sink, eventAggregator, formatter, restrictedToMinimumLevel, levelSwitch, false);
        }

        public static LoggerConfiguration EventAggregator(
            this LoggerSinkConfiguration sinkConfiguration,
            IEventAggregator eventAggregator,
            ITextFormatter formatter,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));
            if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));

            return ConfigureEventAggregator(sinkConfiguration.Sink, eventAggregator, formatter, restrictedToMinimumLevel, levelSwitch, false);
        }

        public static LoggerConfiguration EventAggregator(
            this LoggerAuditSinkConfiguration sinkConfiguration,
            IEventAggregator eventAggregator,
            ITextFormatter formatter,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));
            if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));

            return ConfigureEventAggregator(sinkConfiguration.Sink, eventAggregator, formatter, restrictedToMinimumLevel, levelSwitch, true);
        }

        static LoggerConfiguration ConfigureEventAggregator(
            this Func<ILogEventSink, LogEventLevel, LoggingLevelSwitch, LoggerConfiguration> addSink,
            IEventAggregator eventAggregator,
            ITextFormatter formatter,
            LogEventLevel restrictedToMinimumLevel,
            LoggingLevelSwitch levelSwitch,
            bool propagateExceptions)
        {
            if (addSink == null) throw new ArgumentNullException(nameof(addSink));
            if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));

            ILogEventSink sink;

            try
            {
                sink = new EventAggregatorSink(eventAggregator, formatter);
            }
            catch (Exception ex)
            {
                SelfLog.WriteLine("Unable to create sink: {0}", ex);

                if (propagateExceptions) throw;

                return addSink(new NullSink(), LevelAlias.Maximum, null);
            }

            return addSink(sink, restrictedToMinimumLevel, levelSwitch);
        }
    }
}
