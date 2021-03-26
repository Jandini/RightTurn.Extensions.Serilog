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
        /// <param name="configuration">IConfiguration</param>
        /// <returns></returns>
        public static ITurn WithSerilog(this ITurn turn, Func<ITurn, IConfiguration> configuration)
        {
            return WithSerilog(turn, (config) => config.ReadFrom.Configuration(configuration(turn)));
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
