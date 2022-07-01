using OpenQA.Selenium;

namespace Sample.Web.Core.Core.WebDriver.Factory
{
    public interface IDriverCreator
    {
        IWebDriver GetLocalDriver();
        IWebDriver GetRemoteDriver(string remoteUri);
    }
}