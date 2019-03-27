using System;

namespace LoggingSample_Logs_DAL.Entities
{
    public class LogMessage
    {
        /// <summary>
        /// Gets or sets log message id for database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the machine name that the process is running on
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Gets or sets logger name
        /// </summary>
        public string LoggerName { get; set; }

        /// <summary>
        /// Gets or sets message provided by developer
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets date and time when log message was generated
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets log message level. For example WARN or ERROR
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets component of system where log message was generated
        /// </summary>
        public string MessageSource { get; set; }

        /// <summary>
        /// Gets or sets trace message in case Exception was generated
        /// </summary>
        public string Exception { get; set; }
    }
}