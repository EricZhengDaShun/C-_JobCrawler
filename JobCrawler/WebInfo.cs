using System;
using System.Collections.Generic;
using System.Text;

namespace JobCrawler
{
    public class WebInfo
    {
        public string Url { get; set; }
        public string Html { get; set; }
        public bool Health { get; set; }
    }
}
