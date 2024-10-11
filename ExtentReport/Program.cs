using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

public class ExtentReport
{
    public static void Main()
    {
        // Initialize WebDriver
        IWebDriver driver = new ChromeDriver();

        // Initialize ExtentReports and attach the reporter
        ExtentReports extent = new ExtentReports();
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

            driver.FindElement(By.Id("password")).SendKeys("Password123");
            test.Log(Status.Info, "Entered password");

            driver.FindElement(By.Id("submit")).Click();
            test.Log(Status.Info, "Clicked submit button");

            // Wait for the login result
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".post-title")));

            // Log the success
            test.Log(Status.Pass, "Login Test Passed");
        }
        catch (Exception ex)
        {
            // Log the failure with exception
            test.Log(Status.Fail, "Login Test Failed: " + ex.Message);
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
