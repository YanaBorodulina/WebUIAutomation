using System.Collections.Generic;

using OpenQA.Selenium;
using Sample.Web.Core.Core.WebDriver;
using Sample.Web.Core.Extensions.Selenium;
using Sample.Web.Core.Logger;

namespace Sample.Web.Core.Core
{
    public class CustomWebElement
    {
        public CustomWebElement(By locator)
        {
            Locator = locator;
        }

        private By Locator { get; }

        public IReadOnlyCollection<IWebElement> Elements
        {
            get
            {
                try
                {
                    return GetElements();
                }
                catch (StaleElementReferenceException)
                {
                    LoggingHelper.LogDebug("Re-initialize web elements since DOM has been refreshed");

                    return GetElements();
                }
            }
        }

        public IWebElement Element
        {
            get
            {
                try
                {
                    return GetElement();
                }
                catch (StaleElementReferenceException)
                {
                    LoggingHelper.LogDebug("Re-initialize web element since DOM has been refreshed");

                    return GetElement();
                }
            }
        }

        private IWebElement GetElement() => Driver.Instance.WaitForElementPresent(Locator, TestConfig.DefaultTimeout);

        private IReadOnlyCollection<IWebElement> GetElements() => Driver.Instance.WaitFor(WebElementConditions.ElementsPresent(Locator));
    }
}