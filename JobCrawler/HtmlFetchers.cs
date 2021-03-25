using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobCrawler
{
    public class HtmlFetchers : IDisposable
    {
        private readonly List<Task> taskList = new();
        private readonly CancellationTokenSource tokenSource = new();
        private readonly CancellationToken token;
        private static int longSleepTotalNum = 300;
        private int longSleepNum;

        public ConcurrentQueue<string> Urls { get; set; } = new ConcurrentQueue<string>();
        public ConcurrentQueue<WebInfo> WebInfos { get; set; } = new ConcurrentQueue<WebInfo>();

        public HtmlFetchers(int num)
        {
            token = tokenSource.Token;
            longSleepNum = longSleepTotalNum / num;

            for (int count = 0; count < num; ++count)
            {
                taskList.Add(Task.Run(() => FetchTask(token)));
            }

        }

        public void Dispose()
        {
            tokenSource.Cancel();
            Task.WhenAll(taskList).Wait();
            GC.SuppressFinalize(this);
        }

        private void FetchTask(CancellationToken token)
        {
            using HtmlFetcher htmlFetcher = new();
            string html;
            bool health;
            int loadUrlCount = 0;
            while (!token.IsCancellationRequested)
            {
                if (!Urls.TryDequeue(out string url))
                {
                    Thread.Sleep(100);
                    continue;
                }

                if (loadUrlCount == longSleepNum)
                {
                    Thread.Sleep(5000);
                    loadUrlCount = 0;
                }

                html = htmlFetcher.LoadUrl(url);

                health = true;
                if (string.IsNullOrEmpty(html))
                {
                    health = false;
                }

                WebInfos.Enqueue(new WebInfo
                {
                    Url = url,
                    Html = html,
                    Health = health
                });

                ++loadUrlCount;
            }
            return;
        }
    }
}
