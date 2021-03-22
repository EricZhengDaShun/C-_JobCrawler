using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCrawler
{
    public class JobInfo
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public List<string> Tools { get; set; }
        public string Salary { get; set; }
    }

    public class JobAnalysis
    {
        public List<JobInfo> JobInfos { get; private set; } = new();
        public Configure.JobTitle JobTitleSetting { get; private set; }
        public Configure.ToolHTML ToolHTMLSetting { get; private set; }
        public Configure.Salary SalarySetting { get; private set; }
        public List<WebInfo> JobInfoRawData { get; private set; }

        private readonly HtmlDocument htmlDocument = new();

        public JobAnalysis(List<WebInfo> jobInfoRawData
            , Configure.JobTitle jobTitle
            , Configure.ToolHTML toolHTML
            , Configure.Salary salary)
        {
            JobInfoRawData = jobInfoRawData;
            JobTitleSetting = jobTitle;
            ToolHTMLSetting = toolHTML;
            SalarySetting = salary;
        }

        public void Analysis()
        {
            JobInfos.Clear();
            for (int count = 0; count < JobInfoRawData.Count; ++count)
            {
                htmlDocument.LoadHtml(JobInfoRawData[count].Html);
                var jobInfo = new JobInfo()
                {
                    Url = JobInfoRawData[count].Url
                };

                jobInfo.Title = (from node in htmlDocument.DocumentNode.Descendants()
                                 where node.Name == JobTitleSetting.Type &&
                                 node.Attributes[JobTitleSetting.AttributeName]?.Value == JobTitleSetting.AttributeValue
                                 select node.Attributes[JobTitleSetting.ContentAttribute].Value).ToList().FirstOrDefault();

                jobInfo.Tools = (from node in htmlDocument.DocumentNode.Descendants()
                                 where node.Name == ToolHTMLSetting.Type &&
                                 node.Attributes[ToolHTMLSetting.AttributeName]?.Value == ToolHTMLSetting.AttributeValue
                                 select node.InnerHtml).ToList();

                jobInfo.Salary = (from node in htmlDocument.DocumentNode.Descendants()
                                  where node.Name == SalarySetting.Type &&
                                  node.Attributes[SalarySetting.AttributeName]?.Value == SalarySetting.AttributeValue
                                  select node.InnerHtml).ToList().FirstOrDefault();

                JobInfos.Add(jobInfo);
            }
        }
    }
}
