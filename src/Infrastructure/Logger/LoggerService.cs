namespace CleanArchitecture.Infrastructure.Logger
{
    using CleanArchitecture.Application.Interfaces.Infrastructure;
    using Microsoft.Extensions.Logging;

    public class LoggerService<T> : ILoggerService<T>
    {
        private readonly ILogger<T> logger;

        public LoggerService(ILogger<T> logger) => this.logger = logger;

        public void LogTrace(string message, params object[] arguments) => this.logger.LogTrace(message, arguments);

        public void LogDebug(string message, params object[] arguments) => this.logger.LogDebug(message, arguments);

        public void LogInformation(string message, params object[] arguments) => this.logger.LogInformation(message, arguments);

        public void LogWarning(string message, params object[] arguments) => this.logger.LogWarning(message, arguments);

        public void LogError(string message, params object[] arguments) => this.logger.LogError(message, arguments);

        public void LogCritical(string message, params object[] arguments) => this.logger.LogCritical(message, arguments);
    }
}
