# RightTurn.Extensions.Serilog

Provides Serilog extensions for [RightTurn](https://github.com/Jandini/RightTurn)


## Practical Scenarios

This section will demonstrate practical or interesing ways how to use RightTurn.Extension.Serilog along with other RightTurn extensions.

### Use logging like console write line

In this example time and log level is moved to the right side of the screen leaving the left side acting like console write line. 
Using serilog adds colors and structured data. 

![image](https://user-images.githubusercontent.com/19593367/112734454-c40e9880-8f3d-11eb-93ed-593b909e2dca.png)


###### NuGet Packages

* `RightTurn.Extensions.CommandLine`
* `RightTurn.Extensions.Logging`
* `RightTurn.Extensions.Serilog`



###### IQuickOptions.cs
```C#
namespace QuickStart
{
    interface IQuickOptions
    {
        string Name { get; }
    }
}
```

###### QuickOptions.cs
```C#
using CommandLine;

namespace QuickStart
{
    class QuickOptions : IQuickOptions
    {
        [Option(HelpText = "Your name.")]
        public string Name { get; set; }
    }
}
```

###### IQuickService.cs
```C#
namespace QuickStart
{
    internal interface IQuickService
    {
        void Run();
    }
}
```

###### QuickService.cs
```C#
using Microsoft.Extensions.Logging;
using System;

namespace QuickStart
{
    class QuickService : IQuickService
    {
        readonly ILogger<QuickService> _logger;
        readonly IQuickOptions _options;

        public QuickService(ILogger<QuickService> logger, IQuickOptions options)
        {
            _logger = logger;
            _options = options;
        }

        public void Run()
        {            
            _logger.LogInformation("Service {name} started with {@arguments}", nameof(QuickService), _options);
            
            for (int i = 10; i > 0; i--)
                _logger.LogWarning("Counting down {i}", i);

            _logger.LogError("Where is {i}?", 0);

            _logger.LogInformation("Visiting {uri}", new Uri("https://github.com/Jandini/RightTurn"));
            _logger.LogInformation("Service {name} finished", nameof(QuickService));

        }
    }
}
```




###### Program.cs
```C#
using RightTurn;
using RightTurn.Extensions.CommandLine;
using RightTurn.Extensions.Logging;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog;
using System;

namespace QuickStart
{
    class Program
    {
        static void Main(string[] args) => new Turn()
            .ParseOptions<QuickOptions>(args)
            .WithOptionsAsSingleton<IQuickOptions, QuickOptions>()
            .WithUnhandledExceptionLogging()
            .WithLogging((logging, turn) =>
            {
                var loggerConfiguration = new LoggerConfiguration();
                    
                loggerConfiguration.WriteTo.Console(
                    theme: AnsiConsoleTheme.Literate,
                    outputTemplate: $"{{Message,-{Console.WindowWidth - 19}:lj}} {{Timestamp:HH:mm:ss}} [ {{Level:u4}} ]{{NewLine}}{{Exception}}");

                logging.AddSerilog(
                    loggerConfiguration.CreateLogger(),
                    dispose: true);
            })
            .Take<IQuickService, QuickService>((quick) => quick.Run());
    }
}

```
