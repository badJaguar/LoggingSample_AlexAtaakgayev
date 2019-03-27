using System;
using System.Threading;
using System.Threading.Tasks;
using LoggingSample_Logs_DAL.Context;
using LoggingSample_Logs_DAL.Entities;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace LoggingSample_BLL.LogTargets
{
    [Target("DbTarget")]
    public sealed class DbTarget : AsyncTaskTarget
    {
        public DbTarget()
        {
            this.Host = Environment.MachineName;
        }

        [RequiredParameter]
        public string Host { get; set; }

        protected override async Task WriteAsyncTask(LogEventInfo logEvent, CancellationToken cancellationToken)
        {
            using (var logContext = new LogContext()) {
                logContext.LogMessages.Add(new LogMessage
                {
                    MachineName = this.Host,
                    Exception = logEvent.Exception?.ToString(),
                    LoggerName = logEvent.LoggerName,
                    Level = logEvent.Level.ToString(),
                    Message = logEvent.Message,
                    MessageSource = logEvent.CallerFilePath,
                    TimeStamp = logEvent.TimeStamp
                });

                await logContext.SaveChangesAsync(cancellationToken);
            }

        }
    }
}