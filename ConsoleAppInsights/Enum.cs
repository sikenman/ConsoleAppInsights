using System;

namespace ConsoleAppInsights
{
    class Enum
    {
        public enum Attachments
        {
            Failed = 253040003,
            ProcessedMovedAttachments = 253040002,
            ProcessesNoAttachments = 253040001,
            WaitingProcess = 253040000
        };

        public enum Loglevel
        {
            Error = 200,
            Warn = 300,
            Info = 400,
            Debug = 500,
            Trace = 600
        };
    }
}
