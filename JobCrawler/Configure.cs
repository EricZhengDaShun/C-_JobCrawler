using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCrawler.Configure
{
    public class Url
    {
        public string Path { get; set; }
        public int PageNum { get; set; }
    }

    public class Tool
    {
        public List<string> IncludeTools { get; set; }
        public List<string> ExcludeTools { get; set; }
    }

    public class JobLinkTag
    {
        public string Type { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public string ContentAttribute { get; set; }
    }

    public class ToolHTML
    {
        public string Type { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }

    public class JobTitle
    {
        public string Type { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public string ContentAttribute { get; set; }
    }

    public class Salary
    {
        public string Type { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }

    public class DataSetting
    {
        public bool UseFile { get; set; }
        public bool SaveData { get; set; }
        public string FilePath { get; set; }
    }
}
