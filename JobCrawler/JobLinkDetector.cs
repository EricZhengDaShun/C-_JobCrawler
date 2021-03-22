using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace JobCrawler
{
    public class JobLinkDetector
    {
        public ConcurrentQueue<WebInfo> JobItemHtml { get; private set; }
        public Configure.JobLinkTag JobLinkTag { get; private set; }
        public List<string> JobLinks { get; private set; } = new();

        private readonly HtmlDocument htmlDocument = new();

        public JobLinkDetector(Configure.JobLinkTag jobLinkTag, ConcurrentQueue<WebInfo> jobItemHtml)
        {
            JobLinkTag = jobLinkTag;
            JobItemHtml = new ConcurrentQueue<WebInfo>(jobItemHtml);
        }

        public void Detector()
        {
            JobLinks.Clear();
            while (JobItemHtml.TryDequeue(out WebInfo webInfo))
            {
                htmlDocument.LoadHtml(webInfo.Html);
                JobLinks = (from node in htmlDocument.DocumentNode.Descendants()
                            where node.Name == JobLinkTag.Type &&
                            node.Attributes[JobLinkTag.ContentAttribute] != null &&
                            node.Attributes[JobLinkTag.AttributeName]?.Value == JobLinkTag.AttributeValue
                            select "https:" + node.Attributes[JobLinkTag.ContentAttribute].Value).ToList();
            }
        }
    }
}
