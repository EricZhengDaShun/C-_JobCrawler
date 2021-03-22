using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCrawler
{
    public class JobFilter
    {
        public List<JobInfo> JobInfos { get; private set; }
        public Configure.Tool Tool { get; private set; }
        public List<JobInfo> PassJobInfos { get; private set; } = new();
        public Configure.FilterSwitch FilterSwitch { get; set; }
        public Configure.Title Title { get; private set; }

        public JobFilter(List<JobInfo> jobInfos
            , Configure.FilterSwitch filterSwitch
            , Configure.Tool tool
            , Configure.Title title)
        {
            JobInfos = jobInfos;
            FilterSwitch = filterSwitch;
            Tool = tool;
            Title = title;
        }

        public void Filter()
        {
            PassJobInfos = new List<JobInfo>(JobInfos);
            if (FilterSwitch.ToolEnable) ToolFilter();
            if (FilterSwitch.TitleEnable) TitleFilter();
        }

        private void ToolFilter()
        {
            var fliterResult = new List<JobInfo>();
            PassJobInfos.ForEach(info =>
            {
                if (!Tool.Include.All(x => (info.Tools.Contains(x)))) return;
                if (!Tool.Exclude.All(x =>
                {
                    if (x == string.Empty) return true;
                    return (!info.Tools.Contains(x));
                })) return;
                fliterResult.Add(info);
            });
            PassJobInfos = fliterResult;
        }

        private void TitleFilter()
        {
            var fliterResult = new List<JobInfo>();
            PassJobInfos.ForEach(info =>
            {
                if (!Title.Include.All(x => info.Title.Contains(x, StringComparison.InvariantCultureIgnoreCase))) return;
                if (!Title.Exclude.All(x =>
                {
                    if (x == string.Empty) return true;
                    return (!info.Title.Contains(x, StringComparison.InvariantCultureIgnoreCase));
                })) return;
                fliterResult.Add(info);
            });
            PassJobInfos = fliterResult;
        }
    }
}
