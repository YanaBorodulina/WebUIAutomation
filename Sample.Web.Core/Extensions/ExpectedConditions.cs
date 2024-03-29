﻿using System;
using System.Collections.ObjectModel;
using System.Linq;

using OpenQA.Selenium;

namespace Sample.Web.Core.Extensions
{
    public sealed class ExpectedConditions
    {
        /// <summary>
        ///     Prevents a default instance of the <see cref="ExpectedConditions"/> class from being created.
        /// </summary>
        private ExpectedConditions()
        {
        }

        /// <summary>
        ///     An expectation for checking the title of a page.
        /// </summary>
        /// <param name="title">The expected title, which must be an exact match.</param>
        /// <returns><see langword="true"/> when the title matches; otherwise, <see langword="false"/>.</returns>
        public static Func<IWebDriver, bool> TitleIs(string title)
        {
            return driver => title == driver.Title;
        }

        /// <summary>
        ///     An expectation for checking that the title of a page contains a case-sensitive substring.
        /// </summary>
        /// <param name="title">The fragment of title expected.</param>
        /// <returns><see langword="true"/> when the title matches; otherwise, <see langword="false"/>.</returns>
        public static Func<IWebDriver, bool> TitleContains(string title)
        {
            return driver => driver.Title.Contains(title);
        }

        /// <summary>
        ///     An expectation for the URL of the current page to be a specific URL.
        /// </summary>
        /// <param name="url">The URL that the page should be on</param>
        /// <returns><see langword="true"/> when the URL is what it should be; otherwise, <see langword="false"/>.</returns>
        public static Func<IWebDriver, bool> UrlToBe(string url)
        {
            return driver => driver.Url.ToLowerInvariant().Equals(url.ToLowerInvariant());
        }

        /// <summary>
        ///     An expectation for the URL of the current page to be a specific URL.
        /// </summary>
        /// <param name="fraction">The fraction of the url that the page should be on</param>
        /// <returns><see langword="true"/> when the URL contains the text; otherwise, <see langword="false"/>.</returns>
        public static Func<IWebDriver, bool> UrlContains(string fraction)
        {
            return driver => driver.Url.ToLowerInvariant().Contains(fraction.ToLowerInvariant());
        }

        /// <summary>
        ///     An expectation for checking that an element is present on the DOM of a
        ///     page. This does not necessarily mean that the element is visible.
        /// </summary>
        /// <param name="locator">The locator used to find the element.</param>
        /// <returns>The <see cref="IWebElement"/> once it is located.</returns>
        public static Func<IWebDriver, IWebElement> ElementExists(By locator)
        {
            return driver => driver.FindElement(locator);
        }

        /// <summary>
        ///     An expectation for checking that an element is present on the DOM of a page
        ///     and visible. Visibility means that the element is not only displayed but
        ///     also has a height and width that is greater than 0.
        /// </summary>
        /// <param name="locator">The locator used to find the element.</param>
        /// <returns>The <see cref="IWebElement"/> once it is located and visible.</returns>
        public static Func<IWebDriver, IWebElement> ElementIsVisible(By locator)
        {
            return driver =>
            {
                try
                {
                    return ElementIfVisible(driver.FindElement(locator));
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            };
        }

        /// <summary>
        ///     An expectation for checking that all elements present on the web page that
        ///     match the locator are visible. Visibility means that the elements are not
        ///     only displayed but also have a height and width that is greater than 0.
        /// </summary>
        /// <param name="locator">The locator used to find the element.</param>
        /// <param name="elementsCondition">The elements condition used to specify state element on the page.</param>
        /// <returns>The list of <see cref="IWebElement"/> once it is located and visible.</returns>
        public static Func<IWebDriver, ReadOnlyCollection<IWebElement>> ExpectAllElementsLocatedBy(By locator, Func<IWebElement, bool> elementsCondition = null)
        {
            return driver =>
            {
                try
                {
                    var elements = driver.FindElements(locator);

                    if (elementsCondition != null)
                    {
                        elements = elements.Where(elementsCondition).ToList().AsReadOnly();
                    }

                    return elements.Any() ? elements : null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            };
        }

        /// <summary>
        ///     An expectation for checking that all elements present on the web page that
        ///     match the locator are visible. Visibility means that the elements are not
        ///     only displayed but also have a height and width that is greater than 0.
        /// </summary>
        /// <param name="elements">list of WebElements</param>
        /// <returns>The list of <see cref="IWebElement"/> once it is located and visible.</returns>
        public static Func<IWebDriver, ReadOnlyCollection<IWebElement>> VisibilityOfAllElementsLocatedBy(ReadOnlyCollection<IWebElement> elements)
        {
            return driver =>
            {
                try
                {
                    return elements.Any(element => !element.Displayed) ? elements : null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            };
        }

        /// <summary>
        ///     An expectation for checking if the given text is present in the specified element.
        /// </summary>
        /// <param name="element">The WebElement</param>
        /// <param name="text">Text to be present in the element</param>
        /// <returns><see langword="true"/> once the element contains the given text; otherwise, <see langword="false"/>.</returns>
        public static Func<IWebDriver, bool> TextToBePresentInElement(IWebElement element, string text)
        {
            return driver =>
            {
                try
                {
                    var elementText = element.Text;

                    return elementText.Contains(text);
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            };
        }

        /// <summary>
        ///     An expectation for checking if the given text is present in the element that matches the given locator.
        /// </summary>
        /// <param name="locator">The locator used to find the element.</param>
        /// <param name="text">Text to be present in the element</param>
        /// <returns><see langword="true"/> once the element contains the given text; otherwise, <see langword="false"/>.</returns>
        public static Func<IWebDriver, bool> TextToBePresentInElementLocated(By locator, string text)
        {
            return driver =>
            {
                try
                {
                    var element = driver.FindElement(locator);
                    var elementText = element.Text;

                    return elementText.Contains(text);
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            };
        }

        /// <summary>
        ///     An expectation for checking if the given text is present in the specified elements value attribute.
        /// </summary>
        /// <param name="element">The WebElement</param>
        /// <param name="text">Text to be present in the element</param>
        /// <returns><see langword="true"/> once the element contains the given text; otherwise, <see langword="false"/>.</returns>
        public static Func<IWebDriver, bool> TextToBePresentInElementValue(IWebElement element, string text)
        {
            return driver =>
            {
                try
                {
                    var elementValue = element.GetAttribute("value") ?? "";

                    return elementValue.Contains(text);
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            };
        }

        /// <summary>
        ///     An expectation for checking if the given text is present in the specified elements value attribute.
        /// </summary>
        /// <param name="locator">The locator used to find the element.</param>
        /// <param name="text">Text to be present in the element</param>
        /// <returns><see langword="true"/> once the element contains the given text; otherwise, <see langword="false"/>.</returns>
        public static Func<IWebDriver, bool> TextToBePresentInElementValue(By locator, string text)
        {
            return driver =>
            {
                try
                {
                    var element = driver.FindElement(locator);
                    var elementValue = element.GetAttribute("value") ?? "";

                    return elementValue.Contains(text);
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            };
        }

        /// <summary>
        ///     An expectation for checking whether the given frame is available to switch
        ///     to. If the frame is available it switches the given driver to the
        ///     specified frame.
        /// </summary>
        /// <param name="frameLocator">Used to find the frame (id or name)</param>
        /// <returns>
        ///     <see cref="IWebDriver"/>
        /// </returns>
        public static Func<IWebDriver, IWebDriver> FrameToBeAvailableAndSwitchToIt(string frameLocator)
        {
            return driver =>
            {
                try
                {
                    return driver.SwitchTo().Frame(frameLocator);
                }
                catch (NoSuchFrameException)
                {
                    return null;
                }
            };
        }

        /// <summary>
        ///     An expectation for checking whether the given frame is available to switch
        ///     to. If the frame is available it switches the given driver to the
        ///     specified frame.
        /// </summary>
        /// <param name="locator">Locator for the Frame</param>
        /// <returns>
        ///     <see cref="IWebDriver"/>
        /// </returns>
        public static Func<IWebDriver, IWebDriver> FrameToBeAvailableAndSwitchToIt(By locator)
        {
            return driver =>
            {
                try
                {
                    var frameElement = driver.FindElement(locator);

                    return driver.SwitchTo().Frame(frameElement);
                }
                catch (NoSuchFrameException)
                {
                    return null;
                }
            };
        }

        /// <summary>
        ///     An expectation for checking that an element is either invisible or not present on the DOM.
        /// </summary>
        /// <param name="locator">The locator used to find the element.</param>
        /// <returns><see langword="true"/> if the element is not displayed; otherwise, <see langword="false"/>.</returns>
        public static Func<IWebDriver, bool> InvisibilityOfElementLocated(By locator)
        {
            return driver =>
            {
                try
                {
                    var element = driver.FindElement(locator);

                    return !element.Displayed;
                }
                catch (NoSuchElementException)
                {
                    // Returns true because the element is not present in DOM. The
                    // try block checks if the element is present but is invisible.
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    // Returns true because stale element reference implies that element
                    // is no longer visible.
                    return true;
                }
            };
        }

        /// <summary>
        ///     An expectation for checking that an element with text is either invisible or not present on the DOM.
        /// </summary>
        /// <param name="locator">The locator used to find the element.</param>
        /// <param name="text">Text of the element</param>
        /// <returns><see langword="true"/> if the element is not displayed; otherwise, <see langword="false"/>.</returns>
        public static Func<IWebDriver, bool> InvisibilityOfElementWithText(By locator, string text)
        {
            return driver =>
            {
                try
                {
                    var element = driver.FindElement(locator);
                    var elementText = element.Text;

                    if (string.IsNullOrEmpty(elementText))
                    {
                        return true;
                    }

                    return !elementText.Equals(text);
                }
                catch (NoSuchElementException)
                {
                    // Returns true because the element with text is not present in DOM. The
                    // try block checks if the element is present but is invisible.
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    // Returns true because stale element reference implies that element
                    // is no longer visible.
                    return true;
                }
            };
        }

        /// <summary>
        ///     An expectation for checking an element is visible and enabled such that you
        ///     can click it.
        /// </summary>
        /// <param name="locator">The locator used to find the element.</param>
        /// <returns>The <see cref="IWebElement"/> once it is located and clickable (visible and enabled).</returns>
        public static Func<IWebDriver, IWebElement> ElementToBeClickable(By locator)
        {
            //TODO check if works currently
            return driver =>
            {
                var element = ElementIfVisible(driver.FindElement(locator));

                try
                {
                    if (element != null && element.Enabled)
                    {
                        return element;
                    }

                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            };
        }

        /// <summary>
        ///     An expectation for checking an element is visible and enabled such that you
        ///     can click it.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The <see cref="IWebElement"/> once it is clickable (visible and enabled).</returns>
        public static Func<IWebDriver, IWebElement> ElementToBeClickable(IWebElement element)
        {
            return driver =>
            {
                try
                {
                    if (element != null && element.Displayed && element.Enabled)
                    {
                        return element;
                    }

                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            };
        }

        /// <summary>
        ///     Wait until an element is no longer attached to the DOM.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns><see langword="false"/> is the element is still attached to the DOM; otherwise, <see langword="true"/>.</returns>
        public static Func<IWebDriver, bool> StalenessOf(IWebElement element)
        {
            return driver =>
            {
                try
                {
                    // Calling any method forces a staleness check
                    return element == null || !element.Enabled;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            };
        }

        /// <summary>
        ///     An expectation for checking if the given element is selected.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns><see langword="true"/> given element is selected.; otherwise, <see langword="false"/>.</returns>
        public static Func<IWebDriver, bool> ElementToBeSelected(IWebElement element) => ElementSelectionStateToBe(element, true);

        /// <summary>
        ///     An expectation for checking if the given element is in correct state.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="selected">selected or not selected</param>
        /// <returns><see langword="true"/> given element is in correct state.; otherwise, <see langword="false"/>.</returns>
        public static Func<IWebDriver, bool> ElementToBeSelected(IWebElement element, bool selected)
        {
            return driver => { return element.Selected == selected; };
        }

        /// <summary>
        ///     An expectation for checking if the given element is in correct state.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="selected">selected or not selected</param>
        /// <returns><see langword="true"/> given element is in correct state.; otherwise, <see langword="false"/>.</returns>
        public static Func<IWebDriver, bool> ElementSelectionStateToBe(IWebElement element, bool selected)
        {
            return driver => { return element.Selected == selected; };
        }

        /// <summary>
        ///     An expectation for checking if the given element is selected.
        /// </summary>
        /// <param name="locator">The locator used to find the element.</param>
        /// <returns><see langword="true"/> given element is selected.; otherwise, <see langword="false"/>.</returns>
        public static Func<IWebDriver, bool> ElementToBeSelected(By locator) => ElementSelectionStateToBe(locator, true);

        /// <summary>
        ///     An expectation for checking if the given element is in correct state.
        /// </summary>
        /// <param name="locator">The locator used to find the element.</param>
        /// <param name="selected">selected or not selected</param>
        /// <returns><see langword="true"/> given element is in correct state.; otherwise, <see langword="false"/>.</returns>
        public static Func<IWebDriver, bool> ElementSelectionStateToBe(By locator, bool selected)
        {
            return driver =>
            {
                try
                {
                    var element = driver.FindElement(locator);

                    return element.Selected == selected;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            };
        }

        /// <summary>
        ///     An expectation for checking the AlterIsPresent
        /// </summary>
        /// <returns>Alert </returns>
        public static Func<IWebDriver, IAlert> AlertIsPresent()
        {
            return driver =>
            {
                try
                {
                    return driver.SwitchTo().Alert();
                }
                catch (NoAlertPresentException)
                {
                    return null;
                }
            };
        }

        /// <summary>
        ///     An expectation for checking the Alert State
        /// </summary>
        /// <param name="state">A value indicating whether or not an alert should be displayed in order to meet this condition.</param>
        /// <returns><see langword="true"/> alert is in correct state present or not present; otherwise, <see langword="false"/>.</returns>
        public static Func<IWebDriver, bool> AlertState(bool state)
        {
            return driver =>
            {
                bool alertState;

                try
                {
                    driver.SwitchTo().Alert();
                    alertState = true;

                    return alertState == state;
                }
                catch (NoAlertPresentException)
                {
                    alertState = false;

                    return alertState == state;
                }
            };
        }

        private static IWebElement ElementIfVisible(IWebElement element) => element.Displayed ? element : null;
    }
}