using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Model;

public class ExtentReport
{
    public static void Main()
    {
        // Initialize WebDriver
        IWebDriver driver = new ChromeDriver();

        // Initialize ExtentReports and attach the reporter
        ExtentReports extent = new ExtentReports();

        try
        {
            if (!Directory.Exists(@"D:\ReportResults\Report"))
            {
                Directory.CreateDirectory(@"D:\ReportResults\Report");
            }

            if (!Directory.Exists(@"D:\ReportResults\Screenshots"))
            {
                Directory.CreateDirectory(@"D:\ReportResults\Screenshots");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"The process failed: {e.StackTrace}");
        }

        ExtentSparkReporter htmlreporter = new ExtentSparkReporter(@"D:\ReportResults\Report" + DateTime.Now.ToString("_MMddyyyy_hhmmtt") + ".html");
        extent.AttachReporter(htmlreporter);

        // Create a test in the report
        ExtentTest test = extent.CreateTest("Login Test", "Testing the login functionality");

        try
        {
            // Navigate to the test site
            driver.Navigate().GoToUrl("https://practicetestautomation.com/practice-test-login/");
            driver.Manage().Window.Maximize();
            test.Log(Status.Info, "Navigated to the login page");

            // Enter credentials and submit
            driver.FindElement(By.Id("username")).SendKeys("student");
            test.Log(Status.Info, "Entered username");

            driver.FindElement(By.Id("password")).SendKeys("Password1234");
            test.Log(Status.Info, "Entered password");

            driver.FindElement(By.Id("submit")).Click();
            test.Log(Status.Info, "Clicked submit button");

            // Wait for the login result
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.FindElement(By.CssSelector(".post-title")).Displayed);

            // Log the success
            test.Log(Status.Pass, "Login Test Passed");
        }
        catch (Exception ex)
        {
            // Log the failure with exception
            test.Log(Status.Fail, "Login Test Failed: " + ex.Message);


            // Capture a screenshot on failure
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            string screenshotPath = @"D:\ReportResults\Screenshots\Screenshot" + DateTime.Now.ToString("_MMddyyyy_hhmmtt") + ".png";
            screenshot.SaveAsFile(screenshotPath);
            test.AddScreenCaptureFromPath(screenshotPath);

        }
        finally
        {
            // Quit the driver
            driver.Quit();

            // Flush the report
            extent.Flush();
        }
    }
}
