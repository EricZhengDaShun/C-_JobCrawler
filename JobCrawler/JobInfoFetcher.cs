using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobCrawler
{
    public class JobInfoFetcher
    {
        public HtmlFetchers HtmlFetchers { get; private set; }
        public List<string> JobLinks { get; private set; }
        public ConcurrentQueue<WebInfo> JobInfoHtml { get; private set; } = new ConcurrentQueue<WebInfo>();
        public int FetchErrorNum;

        public JobInfoFetcher(HtmlFetchers htmlFetchers, List<string> jobLinks)
        {
            HtmlFetchers = htmlFetchers;
            JobLinks = jobLinks;
        }

        public void Fetch()
        {
            HtmlFetchers.Urls.Clear();

            JobLinks.ForEach(url => HtmlFetchers.Urls.Enqueue(url));

            while (HtmlFetchers.WebInfos.Count != JobLinks.Count)
            {
                Thread.Sleep(500);
            }

            JobInfoHtml.Clear();
            while (HtmlFetchers.WebInfos.TryDequeue(out WebInfo webInfo))
            {
                if (webInfo.Health == false)
                {
                    ++FetchErrorNum;
                    continue;
                }

                JobInfoHtml.Enqueue(webInfo);
            }
        }
    }
}
