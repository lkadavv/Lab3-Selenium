using NUnit.Framework;
using NUnit.Framework.Legacy;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

[TestFixture]
public class Variant3Tests
{
    private IWebDriver driver;
    private const string BaseUrl = "https://the-internet.herokuapp.com";

    [SetUp]
    public void SetUp()
    {
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
    }

    [TearDown]
    public void TearDown()
    {
        driver?.Quit();
        driver?.Dispose();
    }

    [Test]
    public void Test01_Checkboxes()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/checkboxes");
        var checkboxes = driver.FindElements(By.CssSelector("form#checkboxes input[type='checkbox']"));

        // Перший чекбокс за замовчуванням вимкнений
        Assert.That(checkboxes[0].Selected, Is.False, "Перший чекбокс має бути вимкненим.");
        checkboxes[0].Click();
        Assert.That(checkboxes[0].Selected, Is.True, "Перший чекбокс має бути увімкненим після кліку.");

        // Другий чекбокс за замовчуванням увімкнений
        Assert.That(checkboxes[1].Selected, Is.True, "Другий чекбокс має бути увімкненим.");
        checkboxes[1].Click();
        Assert.That(checkboxes[1].Selected, Is.False, "Другий чекбокс має бути вимкненим після кліку.");
    }

    [Test]
    public void Test02_AddRemoveElements()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/add_remove_elements/");
        var addButton = driver.FindElement(By.XPath("//button[text()='Add Element']"));

        // Додаємо елемент
        addButton.Click();
        var deleteButtons = driver.FindElements(By.ClassName("added-manually"));
        Assert.That(deleteButtons.Count, Is.EqualTo(1), "Кнопка Delete має з'явитися.");

        // Видаляємо елемент
        deleteButtons[0].Click();
        deleteButtons = driver.FindElements(By.ClassName("added-manually"));
        Assert.That(deleteButtons.Count, Is.EqualTo(0), "Кнопка Delete має зникнути після кліку.");
    }

    [Test]
    public void Test03_Dropdown()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/dropdown");
        var dropdownElement = driver.FindElement(By.Id("dropdown"));
        var select = new SelectElement(dropdownElement);

        select.SelectByText("Option 1");
        Assert.That(select.SelectedOption.Text, Is.EqualTo("Option 1"));

        select.SelectByValue("2");
        Assert.That(select.SelectedOption.Text, Is.EqualTo("Option 2"));
    }

    [Test]
    public void Test04_Inputs()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/inputs");
        var inputField = driver.FindElement(By.CssSelector("input[type='number']"));

        inputField.SendKeys("12345");
        Assert.That(inputField.GetAttribute("value"), Is.EqualTo("12345"));
    }

    [Test]
    public void Test05_StatusCodes()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/status_codes");
        driver.FindElement(By.LinkText("200")).Click();

        Assert.That(driver.Url, Does.Contain("/status_codes/200"), "URL має містити статус 200.");
        var text = driver.FindElement(By.CssSelector("div.example p")).Text;
        Assert.That(text, Does.Contain("This page returned a 200 status code."));
    }

    [Test]
    public void Test06_NestedFrames()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/nested_frames");

        // Переходимо у верхній фрейм, а потім у лівий
        driver.SwitchTo().Frame("frame-top");
        driver.SwitchTo().Frame("frame-left");

        var bodyText = driver.FindElement(By.TagName("body")).Text;
        StringAssert.Contains("LEFT", bodyText);

        // Повертаємося до основного документа
        driver.SwitchTo().DefaultContent();
    }

    [Test]
    public void Test07_FormAuthentication()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/login");

        driver.FindElement(By.Id("username")).SendKeys("tomsmith");
        driver.FindElement(By.Id("password")).SendKeys("SuperSecretPassword!");
        driver.FindElement(By.CssSelector("button[type='submit']")).Click();

        var flashMessage = driver.FindElement(By.Id("flash")).Text;
        StringAssert.Contains("You logged into a secure area!", flashMessage);
    }

    [Test]
    public void Test08_DynamicContent()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/dynamic_content");
        var firstRowTextBefore = driver.FindElement(By.CssSelector("#content > div.row:nth-child(1) > div:nth-child(2)")).Text;

        driver.Navigate().Refresh();

        var firstRowTextAfter = driver.FindElement(By.CssSelector("#content > div.row:nth-child(1) > div:nth-child(2)")).Text;

        // Динамічний контент часто змінюється при перезавантаженні
        Assert.That(firstRowTextAfter, Is.Not.Null);
    }

    [Test]
    public void Test09_DisappearingElements()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/disappearing_elements");
        var links = driver.FindElements(By.CssSelector("div.example ul li a"));

        Assert.That(links.Count, Is.GreaterThanOrEqualTo(4), "На сторінці мають бути елементи меню.");
    }

    [Test]
    public void Test10_Hover()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/hovers");
        var figures = driver.FindElements(By.ClassName("figure"));

        // Наводимо курсор на першого користувача
        Actions action = new Actions(driver);
        action.MoveToElement(figures[0]).Perform();

        var caption = figures[0].FindElement(By.ClassName("figcaption"));
        Assert.That(caption.Displayed, Is.True, "Підпис має відображатися після наведення.");
        StringAssert.Contains("name: user1", caption.Text);
    }
}