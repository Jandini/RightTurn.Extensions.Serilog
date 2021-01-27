using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RightTurn.Extensions.Logging;
using Serilog;
using System;

namespace RightTurn.Extensions.Serilog
{
    public static class SerilogTurnExtensions
    {
        public static ITurn WithSerilog(this ITurn turn) => WithSerilog(turn, (configuration) => configuration.ReadFrom.Configuration(turn.Directions.Get<IConfiguration>()));

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
