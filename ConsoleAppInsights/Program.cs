using System;
using System.Configuration;
using System.Collections.Generic;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using TraceSeveretyLevel = Microsoft.ApplicationInsights.DataContracts.SeverityLevel;

namespace ConsoleAppInsights
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start New 2018");
            Exception InnerEx3 = new Exception(Environment.NewLine + "This is Inner-Inner-Inner Exception");
            Exception InnerEx2 = new Exception(Environment.NewLine + "This is Inner-Inner Exception", InnerEx3);
            Exception InnerEx1 = new Exception(Environment.NewLine + "This is Inner Exception", InnerEx2);

            Exception ExInfo = new Exception("This is Info Exception 2018" + Environment.NewLine + "Info", InnerEx1);
            Exception ExWarn = new Exception("This is Warn Exception 2018\r\nWarn", InnerEx1);      // use [\r\n or Environment.NewLine] for new line separator
            Exception ExError = new Exception("This is Error Exception 2018\r\nError", InnerEx1);

            String orgName = "AmitOrg";
            Guid orgID = new Guid("74230d1c-ec57-49b7-a0fb-b2e8d5849703");

            WriteAppInsightsTrace(ExInfo.ToString().Replace("--->", "--->\n"), orgID, orgName, Enum.Loglevel.Info.ToString(), "doc", "38529-28722-289251");
            WriteAppInsightsTrace(ExWarn.ToString().Replace("--->", "--->\n"), orgID, orgName, Enum.Loglevel.Warn.ToString(), "docx", "38539-38293-171945");
            WriteAppInsightsTrace(ExError.ToString().Replace("--->", "--->\n"), orgID, orgName, Enum.Loglevel.Error.ToString(), "xls", "38544-38329-171329");
            Console.WriteLine("Ending New 2018");

            try
            {
                try
                {
                    int y = 0;
                    int x = 555 / y;
                }
                catch
                {
                    Exception ErrorExp = new Exception("This is Error Exception 2018\r\nError", InnerEx1);
                    throw ErrorExp;
                }
            }
            catch (Exception ex)
            {
                WriteAppInsightsException(ex, orgID, orgName, Enum.Loglevel.Info.ToString(), "xlsx", "38529-28722-289251");
            }
        }

        private static void WriteAppInsightsTrace(string Message, Guid OrgIDValue, string OrgNameValue, string LogLevel, string AttachmentType, string AttachmentId)
        {
            TelemetryClient logTrace = new TelemetryClient();
            TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["POInstrumentationKey"].ToString();

            if (!string.IsNullOrWhiteSpace(Message))
            {
                var LogData = new LogData
                {
                    FeatureCode = "PAT",
                    Message = Message,
                };

                if (LogData.CustomProperties == null)
                {
                    Dictionary<string, string> dictionary = new Dictionary<string, string>
                    {
                        { "LogLevel", LogLevel},
                        { "Source", "PAT" },
                        { "OrgId", OrgIDValue.ToString() },
                        { "OrgName", OrgNameValue },
                        { "AttachmentType", AttachmentType },
                        { "AttachmentId", AttachmentId }
                    };
                    LogData.CustomProperties = dictionary;
                }

                TraceSeveretyLevel TSLevel = TraceSeveretyLevel.Information;
                switch (LogLevel)
                {
                    case "Info":
                        TSLevel = TraceSeveretyLevel.Information;
                        break;

                    case "Warn":
                        TSLevel = TraceSeveretyLevel.Warning;
                        break;

                    case "Error":
                        TSLevel = TraceSeveretyLevel.Error;
                        break;
                }

                logTrace.TrackTrace(LogData.Message, TSLevel, LogData.CustomProperties);
                logTrace.Flush();
            }
        }

        private static void WriteAppInsightsException(Exception Ex, Guid OrgIDValue, string OrgNameValue, string LogLevel, string AttachmentType, string AttachmentId)
        {
            TelemetryClient logTrace = new TelemetryClient();
            TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["POInstrumentationKey"].ToString();

            if (!string.IsNullOrWhiteSpace(Ex.Message))
            {
                var LogData = new LogData
                {
                    FeatureCode = "PAT",
                    Message = Ex.Message,
                };

                if (LogData.CustomProperties == null)
                {
                    Dictionary<string, string> dictionary = new Dictionary<string, string>
                    {
                        { "LogLevel", LogLevel},
                        { "Source", "PAT" },
                        { "OrgId", OrgIDValue.ToString() },
                        { "OrgName", OrgNameValue },
                        { "AttachmentType", AttachmentType },
                        { "AttachmentId", AttachmentId }
                    };
                    LogData.CustomProperties = dictionary;
                }

                logTrace.TrackException(Ex, LogData.CustomProperties);
                logTrace.Flush();
            }
        }
    }
}
