using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace JobCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var configureLoader = new ConfigureLoader();
            configureLoader.Load("appsettings.json");
            var dataSetting = configureLoader.DataSetting;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var jobInfoRawData = new List<WebInfo>();

            if (dataSetting.UseFile && File.Exists(dataSetting.FilePath))
            {
                string jsonData = File.ReadAllText(dataSetting.FilePath);
                jobInfoRawData = JsonConvert.DeserializeObject<List<WebInfo>>(jsonData);
            }
            else
            {
                using var htmlFetchers = new HtmlFetchers(Environment.ProcessorCount);

                var jobItemFetcher = new JobItemFetcher(htmlFetchers, configureLoader.Url);
                jobItemFetcher.Fetch();
                var jobLinkDetector = new JobLinkDetector(configureLoader.JobLinkTag, jobItemFetcher.JobItemHtml);
                jobLinkDetector.Detector();

                var jobInfoFetcher = new JobInfoFetcher(htmlFetchers, jobLinkDetector.JobLinks);
                jobInfoFetcher.Fetch();
                while (jobInfoFetcher.JobInfoHtml.TryDequeue(out WebInfo webInfo))
                {
                    jobInfoRawData.Add(webInfo);
                }

                if (dataSetting.SaveData)
                {
                    string jsonData = JsonConvert.SerializeObject(jobInfoRawData);
                    File.WriteAllText(dataSetting.FilePath, jsonData);
                }

                Console.WriteLine("Job Item Fetch ErrorNum: {0}", jobItemFetcher.FetchErrorNum);
                Console.WriteLine("Job Info Fetch ErrorNum: {0}", jobInfoFetcher.FetchErrorNum);
            }


            var jobAnalysis = new JobAnalysis(jobInfoRawData
                , configureLoader.JobTitle
                , configureLoader.ToolHTML
                , configureLoader.Salary);
            jobAnalysis.Analysis();

            var jobFilter = new JobFilter(jobAnalysis.JobInfos
                , configureLoader.FilterSwitch
                , configureLoader.Tool
                , configureLoader.Title);
            jobFilter.Filter();


            var stringBuilder = new StringBuilder();
            jobFilter.PassJobInfos.ForEach(info =>
            {
                stringBuilder.AppendLine(info.Title);
                info.Tools.ForEach(tool => stringBuilder.AppendLine(tool));
                stringBuilder.AppendLine(info.Salary);
                stringBuilder.AppendLine(info.Url);
                stringBuilder.AppendLine();
            });

            File.WriteAllText("result.txt", stringBuilder.ToString());

            stopwatch.Stop();
            Console.WriteLine("Done !");
            Console.WriteLine("Spend {0} s", stopwatch.Elapsed.TotalSeconds);

            Console.Read();
        }
    }
}
