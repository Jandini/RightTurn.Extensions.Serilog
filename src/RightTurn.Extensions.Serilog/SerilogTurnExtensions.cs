using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RightTurn.Extensions.Configuration;
using RightTurn.Extensions.Logging;
using Serilog;
using System;

namespace RightTurn.Extensions.Serilog
{
    public static class SerilogTurnExtensions
    {
        /// <summary>
        /// Add Serilog with configuration direction.
        /// If configuration is not present then optional appsettings.json configuration file is used.
        /// </summary>
        /// <param name="turn"></param>
        /// <returns></returns>
        public static ITurn WithSerilog(this ITurn turn) 
        {
            if (!turn.Directions.Have<IConfiguration>())
                turn.WithConfigurationFile();

            return WithSerilog(turn, (configuration) => configuration.ReadFrom.Configuration(turn.Directions.Get<IConfiguration>()));
        }

        /// <summary>
        /// Add Serilog with configuration action.
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ITurn WithSerilog(this ITurn turn, Func<LoggerConfiguration, LoggerConfiguration> configuration)
        {
            turn.Directions.Add<Action<ILoggingBuilder>>((ILoggingBuilder builder) =>
            {
                builder.AddSerilog(
                    configuration
                        .Invoke(new LoggerConfiguration())
                        .CreateLogger(),
                    dispose: true);
            });

            if (!turn.Directions.Have<ITurnLogging>())
                turn.AddLogging();

            return turn;
        }
    }
}
