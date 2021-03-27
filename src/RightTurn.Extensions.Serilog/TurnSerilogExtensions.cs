using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace RightTurn.Extensions.Serilog
{
    public static class TurnSerilogExtensions
    {
        /// <summary>
        /// Add Serilog from configuration.
        /// </summary>
        /// <param name="turn"></param>
        /// <returns></returns>
        public static ITurn WithSerilog(this ITurn turn)
        {
            return WithSerilog(turn, (loggerConfiguration) => {                               
                if (!turn.Directions.Have<IConfiguration>(out var configuration))
                    throw new Exception("Configuration is missing. Use configuration extensions to provide configuration builder.");
                return loggerConfiguration.ReadFrom.Configuration(configuration);
            });
        }

        /// <summary>
        /// Add Serilog with configuration action.
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ITurn WithSerilog(this ITurn turn, Func<LoggerConfiguration, LoggerConfiguration> configuration)
        {
            turn.Directions.Add<ITurnLogging>(new TurnSerilogLogging((ILoggingBuilder builder) =>
            {
                builder.AddSerilog(
                    configuration
                        .Invoke(new LoggerConfiguration())
                        .CreateLogger(),
                    dispose: true);
            }));

            return turn;
        }
    }
}
