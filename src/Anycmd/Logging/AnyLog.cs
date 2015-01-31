
namespace Anycmd.Logging
{
    using System;
    using System.Data;
    using Util;

    /// <summary>
    /// 任何日志<see cref="IAnyLog"/>
    /// </summary>
    public class AnyLog : IAnyLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public AnyLog(Guid id)
        {
            this.Id = id;
            System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
            AppDomain domain = AppDomain.CurrentDomain;
            this.Machine = Environment.MachineName;
            this.Process = process.ProcessName;
            this.BaseDirectory = domain.BaseDirectory;
            this.DynamicDirectory = domain.DynamicDirectory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static AnyLog Create(IDataRecord reader)
        {
            return new AnyLog(reader.GetGuid(reader.GetOrdinal("Id")))
            {
                Machine = reader.GetNullableString(reader.GetOrdinal("Machine")),
                Process = reader.GetNullableString(reader.GetOrdinal("Process")),
                BaseDirectory = reader.GetNullableString(reader.GetOrdinal("BaseDirectory")),
                DynamicDirectory = reader.GetNullableString(reader.GetOrdinal("DynamicDirectory")),
                Req_Ontology = reader.GetNullableString(reader.GetOrdinal("Req_Ontology")),
                Req_Verb = reader.GetNullableString(reader.GetOrdinal("Req_Verb")),
                Req_ClientId = reader.GetNullableString(reader.GetOrdinal("Req_ClientId")),
                Req_ClientType = reader.GetNullableString(reader.GetOrdinal("Req_ClientType")),
                CreateOn = reader.GetDateTime(reader.GetOrdinal("CreateOn"), SystemTime.MinDate),
                Req_Description = reader.GetNullableString(reader.GetOrdinal("Req_Description")),
                Req_EventSourceType = reader.GetNullableString(reader.GetOrdinal("Req_EventSourceType")),
                Req_EventSubjectCode = reader.GetNullableString(reader.GetOrdinal("Req_EventSubjectCode")),
                InfoFormat = reader.GetNullableString(reader.GetOrdinal("InfoFormat")),
                Req_InfoId = reader.GetNullableString(reader.GetOrdinal("Req_InfoId")),
                Req_InfoValue = reader.GetNullableString(reader.GetOrdinal("Req_InfoValue")),
                Req_UserName = reader.GetNullableString(reader.GetOrdinal("Req_UserName")),
                Req_IsDumb = reader.GetBoolean(reader.GetOrdinal("Req_IsDumb")),
                LocalEntityId = reader.GetNullableString(reader.GetOrdinal("LocalEntityId")),
                CatalogCode = reader.GetNullableString(reader.GetOrdinal("CatalogCode")),
                Req_ReasonPhrase = reader.GetNullableString(reader.GetOrdinal("Req_ReasonPhrase")),
                ReceivedOn = reader.GetDateTime(reader.GetOrdinal("ReceivedOn"), SystemTime.MinDate),
                Req_MessageId = reader.GetNullableString(reader.GetOrdinal("Req_MessageId")),
                Req_MessageType = reader.GetNullableString(reader.GetOrdinal("Req_MessageType")),
                Req_QueryList = reader.GetNullableString(reader.GetOrdinal("Req_QueryList")),
                Req_Status = reader.GetInt32(reader.GetOrdinal("Req_Status"), 0),
                Req_TimeStamp = reader.GetDateTime(reader.GetOrdinal("Req_TimeStamp"), SystemTime.MinDate),
                Req_Version = reader.GetNullableString(reader.GetOrdinal("Req_Version")),
                Res_InfoValue = reader.GetNullableString(reader.GetOrdinal("Res_InfoValue")),
                Res_Description = reader.GetNullableString(reader.GetOrdinal("Res_Description")),
                Res_ReasonPhrase = reader.GetNullableString(reader.GetOrdinal("Res_ReasonPhrase")),
                Res_StateCode = reader.GetInt32(reader.GetOrdinal("Res_StateCode"), 0)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataRow ToDataRow(DataTable dt)
        {
            var row = dt.NewRow();
            row["Id"] = this.Id == Guid.Empty ? Guid.NewGuid() : this.Id;
            row["Machine"] = this.Machine;
            row["Process"] = this.Process;
            row["BaseDirectory"] = this.BaseDirectory;
            row["DynamicDirectory"] = this.DynamicDirectory;
            row["ReceivedOn"] = this.ReceivedOn;
            row["CreateOn"] = this.CreateOn;
            row["LocalEntityId"] = this.LocalEntityId;
            row["CatalogCode"] = this.CatalogCode;
            row["Req_Verb"] = this.Req_Verb;
            row["InfoFormat"] = this.InfoFormat;
            row["Req_InfoValue"] = this.Req_InfoValue;
            row["Req_Ontology"] = this.Req_Ontology;
            row["Req_InfoId"] = this.Req_InfoId;
            row["Req_ClientId"] = this.Req_ClientId;
            row["Req_TimeStamp"] = this.Req_TimeStamp;
            row["Req_ClientType"] = this.Req_ClientType;
            row["Req_MessageType"] = this.Req_MessageType;
            row["Req_MessageId"] = this.Req_MessageId;
            row["Req_Status"] = this.Req_Status;
            row["Req_ReasonPhrase"] = this.Req_ReasonPhrase;
            row["Req_Description"] = this.Req_Description;
            row["Req_EventSubjectCode"] = this.Req_EventSubjectCode;
            row["Req_EventSourceType"] = this.Req_EventSourceType;
            row["Req_UserName"] = this.Req_UserName;
            row["Req_QueryList"] = this.Req_QueryList;
            row["Req_Version"] = this.Req_Version;
            row["Req_IsDumb"] = this.Req_IsDumb;
            row["Res_StateCode"] = this.Res_StateCode;
            row["Res_ReasonPhrase"] = this.Res_ReasonPhrase;
            row["Res_Description"] = this.Res_Description;
            row["Res_InfoValue"] = this.Res_InfoValue;
            row["RowGuid"] = Guid.NewGuid();
            // 检测每一列对应的值是否超出了数据库定义的长度
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var dbCol = dt.Columns[i];
                if (dbCol.MaxLength != -1)
                {
                    if (row[i].ToString().Length > dbCol.MaxLength)
                    {
                        row[i] = row[i].ToString().Substring(0, dbCol.MaxLength);
                    }
                }
            }

            return row;
        }

        public Guid Id { get; set; }

        public string Machine { get; private set; }

        public string Process { get; private set; }

        public string BaseDirectory { get; private set; }

        public string DynamicDirectory { get; private set; }

        public string LocalEntityId { get; set; }

        public string CatalogCode { get; set; }

        public DateTime ReceivedOn { get; set; }

        public DateTime CreateOn { get; set; }

        public string InfoFormat { get; set; }

        public string Req_Version { get; set; }

        public bool Req_IsDumb { get; set; }

        public string Req_MessageType { get; set; }

        public string Req_MessageId { get; set; }

        public string Req_ClientType { get; set; }

        public string Req_ClientId { get; set; }

        public int Req_Status { get; set; }

        public string Req_ReasonPhrase { get; set; }

        public string Req_Description { get; set; }

        public string Req_EventSubjectCode { get; set; }

        public string Req_EventSourceType { get; set; }

        public string Req_UserName { get; set; }

        public string Req_Verb { get; set; }

        public string Req_Ontology { get; set; }

        public DateTime Req_TimeStamp { get; set; }

        public string Req_QueryList { get; set; }

        public string Req_InfoId { get; set; }

        public string Req_InfoValue { get; set; }

        public int Res_StateCode { get; set; }

        public string Res_ReasonPhrase { get; set; }

        public string Res_Description { get; set; }

        public string Res_InfoValue { get; set; }
    }
}
