using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace ConsoleAppTemplate
{
    partial class Program
    {
        public class GreatingService : IGreatingService
        {
            private readonly Logger logger;
            private readonly IConfiguration configuration;

            public GreatingService(Logger logger, IConfiguration configuration)
            {
                this.logger = logger;
                this.configuration = configuration;
            }

            public void ToGreat()
            {
                for (int i = 0; i < configuration.GetValue<int>("LoopTimes"); i++)
                {
                    logger.Information("Run number {runNumber}", i);
                }
            }
        }
    }
}
