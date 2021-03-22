using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobCrawler
{
    public class JobItemFetcher
    {
        public Configure.Url Url { get; private set; }
        public ConcurrentQueue<string> JobLinks { get; private set; } = new ConcurrentQueue<string>();
        public HtmlFetchers HtmlFetchers { get; private set; }
        public ConcurrentQueue<WebInfo> JobItemHtml { get; private set; } = new  ConcurrentQueue<WebInfo>();
        public int FetchErrorNum;

        public JobItemFetcher(HtmlFetchers htmlFetchers, Configure.Url url)
        {
            HtmlFetchers = htmlFetchers;
            Url = url;
        }

        public void Fetch()
        {
            HtmlFetchers.Urls.Clear();

            List<string> urls = GetUrls(Url);
            urls.ForEach(url => HtmlFetchers.Urls.Enqueue(url));

            while (HtmlFetchers.WebInfos.Count != urls.Count)
            {
                Thread.Sleep(500);
            }

            JobItemHtml.Clear();
            while (HtmlFetchers.WebInfos.TryDequeue(out WebInfo webInfo))
            {
                if (webInfo.Health == false)
                {
                    ++FetchErrorNum;
                    continue;
                }

                JobItemHtml.Enqueue(webInfo);
            }
        }

        private static List<string> GetUrls(Configure.Url url)
        {
            var urls = new List<string>();
            string keyWord = "&page=";
            int pagePosition = url.Path.LastIndexOf(keyWord);
            if (pagePosition == -1) return urls;
            int keyWordTailPosition = pagePosition + keyWord.Length;

            var header = url.Path.Substring(0, keyWordTailPosition);
            var tail = url.Path[(keyWordTailPosition + 1)..];
            for (int count = 0; count < url.PageNum; ++count)
            {
                string jobItemUrl = header + count + tail;
                urls.Add(jobItemUrl);
            }

            return urls;
        }
    }
}
