using Microsoft.Playwright;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;

namespace MAUI.Hybrid.Integration.Tests;

[TestClass]
public class MAUIAppTests
{
    [TestMethod]
    public async Task Click_Button_Message_Is_Shown()
    {
        // Setup
        AppiumDriver driver = null;
        IPlaywright playwright = null;

        try
        {
            // Start Appium service with environment variables for WebView2
            var builder = new AppiumServiceBuilder()
                .WithEnvironment(new Dictionary<string, string>()
                {
                        { "WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS", "--remote-debugging-port=9222" },
                        { "WEBVIEW2_ENABLE_MONITORING", "1" }
                });

            // Initialize WindowsDriver for the MAUI app
            driver = new WindowsDriver(builder, new AppiumOptions()
            {
                App = "..\\..\\..\\..\\MAUI.Hybrid.Integration.Example\\bin\\Debug\\net10.0-windows10.0.19041.0\\win-x64\\MAUI.Hybrid.Integration.Example.exe",
                PlatformName = "windows",
                DeviceName = "WindowsPC",
                AutomationName = "windows",
            });

            // Initialize Playwright and connect to the WebView2 instance
            playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.ConnectOverCDPAsync("http://localhost:9222");

            var contexts = browser.Contexts;
            var context = contexts.Count > 0 ? contexts[0] : await browser.NewContextAsync();

            var pages = context.Pages;
            var page = pages.Count > 0 ? pages[0] : await context.NewPageAsync();

            // Act
            await page.GetByRole(AriaRole.Button, new() { Name = "Click me" }).ClickAsync();
            await page.Locator("#message").WaitForAsync();

            // Assert
            Assert.AreEqual("Button clicked!", await page.Locator("#message").InnerTextAsync());
        }
        finally
        {
            playwright?.Dispose();
            driver?.Quit();
        }
    }
}
