using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace JobCrawler
{
    public class ConfigureLoader
    {
        public Configure.Url Url { get; set; }
        public Configure.Tool Tool { get; set; }
        public Configure.JobLinkTag JobLinkTag { get; set; }
        public Configure.ToolHTML ToolHTML { get; set; }
        public Configure.JobTitle JobTitle { get; set; }
        public Configure.Salary Salary { get; set; }
        public Configure.DataSetting DataSetting { get; set; }
        public Configure.FilterSwitch FilterSwitch { get; set; }
        public Configure.Title Title { get; set; }
        public Configure.JobContent JobContent { get; set; }
        public Configure.Content Content { get; set;}

        public void Load(string fileName)
        {
            var config = new ConfigurationBuilder()
                            .AddJsonFile(
                                path: "appsettings.json",
                                optional: false,
                                reloadOnChange: true)
                            .Build();

            var section = config.GetSection(nameof(Configure.Url));
            Url = section.Get<Configure.Url>();

            section = config.GetSection(nameof(Configure.Tool));
            Tool = section.Get<Configure.Tool>();

            section = config.GetSection("htmlTag:" + nameof(Configure.JobLinkTag));
            JobLinkTag = section.Get<Configure.JobLinkTag>();

            section = config.GetSection("htmlTag:" + nameof(Configure.ToolHTML));
            ToolHTML = section.Get<Configure.ToolHTML>();

            section = config.GetSection("htmlTag:" + nameof(Configure.JobTitle));
            JobTitle = section.Get<Configure.JobTitle>();

            section = config.GetSection("htmlTag:" + nameof(Configure.Salary));
            Salary = section.Get<Configure.Salary>();

            section = config.GetSection(nameof(Configure.DataSetting));
            DataSetting = section.Get<Configure.DataSetting>();

            section = config.GetSection(nameof(Configure.FilterSwitch));
            FilterSwitch = section.Get<Configure.FilterSwitch>();

            section = config.GetSection(nameof(Configure.Title));
            Title = section.Get<Configure.Title>();

            section = config.GetSection("htmlTag:" + nameof(Configure.JobContent));
            JobContent = section.Get<Configure.JobContent>();

            section = config.GetSection(nameof(Configure.Content));
            Content = section.Get<Configure.Content>();
        }
    }
}
