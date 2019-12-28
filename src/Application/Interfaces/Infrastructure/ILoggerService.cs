namespace CleanArchitecture.Application.Interfaces.Infrastructure
{
    public interface ILoggerService<T>
    {
        /// <summary>
        /// Logs that contain the most detailed messages. These messages may contain sensitive application data. These messages are disabled by default and should never be enabled in a production environment.
        /// </summary>
        void LogTrace(string message, params object[] arguments);

        /// <summary>
        /// Logs that are used for interactive investigation during development. These logs should primarily contain information useful for debugging and have no long-term value.
        /// </summary>
        void LogDebug(string message, params object[] arguments);

        /// <summary>
        /// Logs that track the general flow of the application. These logs should have long-term value.
        /// </summary>
        void LogInformation(string message, params object[] arguments);

        /// <summary>
        /// Logs that highlight an abnormal or unexpected event in the application flow, but do not otherwise cause the application execution to stop.
        /// </summary>
        void LogWarning(string message, params object[] arguments);

        /// <summary>
        /// Logs that highlight when the current flow of execution is stopped due to a failure. These should indicate a failure in the current activity, not an application-wide failure.
        /// </summary>
        void LogError(string message, params object[] arguments);

        void LogCritical(string message, params object[] arguments);

        // void Log(LogLevel logLevel, string message, params object[] arguments);
    }
}