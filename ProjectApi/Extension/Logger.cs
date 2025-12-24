using Microsoft.Extensions.Logging;
using System;

namespace ProjectApi.Extension
{
    public class Logger : ILogger
    {
        // Các phương thức khác của ILogger (Log, IsEnabled, ...)

        public IDisposable BeginScope<TState>(TState state)
        {
            // Triển khai logic tạo scope của bạn ở đây.
            // Ví dụ đơn giản:
            return new NoOpDisposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            // Implement your logic to determine if logging is enabled for the given log level
            return true; // Or your specific logic
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // Implement your logging logic here
            Console.WriteLine(formatter(state, exception)); // Simple example
        }

        private class NoOpDisposable : IDisposable
        {
            public void Dispose()
            {
                // Nothing to dispose in this simple scope
            }
        }
    }
}