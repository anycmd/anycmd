
namespace Anycmd.Logging
{
    using Extensions;
    using System.Data;

    public class ExceptionLog : ExceptionLogBase
    {
        public static ExceptionLog Create(IDataRecord reader)
        {
            return new ExceptionLog
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                BaseDirectory = reader.GetNullableString("BaseDirectory"),
                Process = reader.GetNullableString("Process"),
                Machine = reader.GetNullableString("Machine"),
                Level = reader.GetNullableString("Level"),
                Logger = reader.GetNullableString("Logger"),
                LogOn = reader.GetDateTime(reader.GetOrdinal("LogOn")),
                Message = reader.GetNullableString("Message"),
                Thread = reader.GetNullableString("Thread"),
                Exception = reader.GetNullableString("Exception")
            };
        }
    }
}
