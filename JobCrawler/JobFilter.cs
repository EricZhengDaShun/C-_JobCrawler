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

        public JobFilter(List<JobInfo> jobInfos, Configure.Tool tool)
        {
            JobInfos = jobInfos;
            Tool = tool;
        }

        public void Filter()
        {
            ToolFilter();
        }

        private void ToolFilter()
        {
            PassJobInfos.Clear();

            JobInfos.ForEach(info =>
            {
                if (!Tool.IncludeTools.All(x => (info.Tools.Contains(x)))) return;
                if (!Tool.ExcludeTools.All(x => (!info.Tools.Contains(x)))) return;
                PassJobInfos.Add(info);
            });            
        }
    }
}
