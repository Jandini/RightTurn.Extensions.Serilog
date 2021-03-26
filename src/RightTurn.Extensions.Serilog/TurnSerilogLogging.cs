using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace RightTurn.Extensions.Serilog
{
    class TurnSerilogLogging : ITurnLogging
    {
        readonly Action<ILoggingBuilder> _logging;

        public TurnSerilogLogging(Action<ILoggingBuilder> logging)
        {
            _logging = logging;
        }

        /// <summary>
        /// This method is called from Turn.Take to add logging with logging builder. 
        /// You must add Action<ILoggingBuilder> to direction container using WithLogging extensions.
        /// </summary>
        /// <param name="turn">Turn instance</param>
        public void AddLogging(ITurn turn)
        {
            turn.Directions.ServiceCollection().AddLogging(_logging);
        }
    }
}
