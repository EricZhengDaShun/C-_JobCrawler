using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace JobCrawler
{
    public class HtmlFetcher : IDisposable
    {
        private readonly IWebDriver driver;

        public HtmlFetcher()
        {
            try
            {
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments("headless");
                driver = new ChromeDriver(chromeOptions);
            }
            catch (WebDriverException e)
            {
                Console.WriteLine("WebDriverException: " + e.Message);
            }
        }

        public void Dispose()
        {
            if (driver is not null)
            {
                driver.Close();
                driver.Quit();
            }
            GC.SuppressFinalize(this);
        }

        public string LoadUrl(string url)
        {
            int currentRetry = 0;
            string result = "";
            while (currentRetry < 2)
            {
                try
                {
                    driver.Navigate().GoToUrl(url);
                    result = driver.PageSource;
                    break;
                }
                catch (WebDriverException e)
                {
                    Console.WriteLine("WebDriverException: " + e.Message);
                    ++currentRetry;
                    Thread.Sleep(500);
                    continue;
                }
            }
            return result;
        }
    }
}
