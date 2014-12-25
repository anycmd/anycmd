
namespace Anycmd.Logging
{
    using log4net;
    using log4net.Appender;
    using System;
    using System.Configuration;

    /// <summary>
    /// 
    /// </summary>
    public class AnycmdAdoNetAppender : AdoNetAppender
    {
        private static ILog _log;

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        protected static ILog Log
        {
            get { return _log ?? (_log = LogManager.GetLogger(typeof (AnycmdAdoNetAppender))); }
        }

        /// <summary>
        /// Initialize the appender based on the options set
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is part of the <see cref="T:log4net.Core.IOptionHandler"/> delayed object
        /// activation scheme. The <see cref="M:log4net.Appender.AdoNetAppender.ActivateOptions"/> method must
        /// be called on this object after the configuration properties have
        /// been set. Until <see cref="M:log4net.Appender.AdoNetAppender.ActivateOptions"/> is called this
        /// object is in an undefined state and must not be used.
        /// </para>
        /// <para>
        /// If any of the configuration properties are modified then
        /// <see cref="M:log4net.Appender.AdoNetAppender.ActivateOptions"/> must be called again.
        /// </para>
        /// </remarks>
        public override void ActivateOptions()
        {
            PopulateConnectionString();
            base.ActivateOptions();
        }

        /// <summary>
        /// Populates the connection string.
        /// </summary>
        private void PopulateConnectionString()
        {
            // if connection string already defined, do nothing
            if (!String.IsNullOrEmpty(ConnectionString)) return;

            // retrieve connection string from the name
            ConnectionString = ConfigurationManager.AppSettings["BootDbConnString"];
        }
    }
}
