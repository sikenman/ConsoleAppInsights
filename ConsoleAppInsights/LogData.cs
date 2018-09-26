using System;
using System.Collections.Generic;

namespace ConsoleAppInsights
{
    public class LogData
    {
        public string FeatureCode { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> CustomProperties { get; set; }
    }
}

