using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using System.Threading;
using NUnit.Allure.Core;
using NUnit.Allure.Attributes;
using CodeJam;

namespace nopCommerceLogIn;

[AllureNUnit]
[AllureSuite("Tests")]
public class Tests
{

    EdgeDriver? driver;
    Actions actions;

    [SetUp]
    public void Setup()
    {
        this.driver = new EdgeDriver();
        this.driver.Url = "http://192.168.56.3:5044/"; //set my url
        this.actions = new Actions(driver);
    }

    [Test]
    public void AddFeedBack()
    {

        this.driver?.Navigate().GoToUrl("http://192.168.56.3:5044/about");
        Thread.Sleep(3000);

        IWebElement? tbEmail = this.driver?.FindElement(By.Id("email"));
        tbEmail?.Clear();
        tbEmail.SendKeys("user@gmail.com");

        IWebElement? tbMessage = this.driver?.FindElement(By.Id("textArea"));
        tbMessage.Clear();
        tbMessage.SendKeys("AAA bbb CCC");


        //IWebElement? submitButton = this.driver?.FindElement(By.XPath("//button[@type='submit']"));
        IWebElement? btnSubmit = this.driver?.FindElement(By.Id("submit"));
        btnSubmit.Click();
        Thread.Sleep(5000);
        try
        {
            IWebElement? textElement = this.driver.FindElement(By.XPath("//h2"));//h1.page_header
            string submitMessageText = textElement.Text.ToLower();
            Code.AssertState(string.Equals(submitMessageText, "Thank you, your message has been sent and will be reviewed shortly".ToLower()), "submitMessage is not valid!");
        }
        catch
        {
            Code.AssertState(false, "Test_1 failed!");
        }
    }


    [Test]
    public void PostRegister()
    {

        this.driver?.Navigate().GoToUrl("http://192.168.56.3:5044/booking");
        Thread.Sleep(1000);
        int i = 1;
        for (; ; )
        {
            if (i > 10)
            {
                break;
            }
            else
            {
                Thread.Sleep(1000);
            }

            try
            {
                IWebElement? fnlnField1 = this.driver?.FindElement(By.Id("fnln"));
                fnlnField1.Clear();
                fnlnField1.SendKeys("Alex Blob");
            }
            catch
            {
                //ignored
            }
            break;
            i++;
        }
        Thread.Sleep(1000);
        IWebElement? uniField = this.driver?.FindElement(By.Id("claim"));
        uniField.Clear();
        uniField.SendKeys("aa bb cc");

        Thread.Sleep(1000);
        IWebElement? fnlnField = this.driver?.FindElement(By.Id("fnln"));
        //fnlnField.Clear();
        fnlnField.SendKeys("Alex Blob");

        Thread.Sleep(1000);
        IWebElement? phoneField = this.driver?.FindElement(By.Id("phoneNumber"));
        phoneField.Clear();
        phoneField.SendKeys("+375336702727");

        Thread.Sleep(1000);
        IWebElement? emailField = this.driver?.FindElement(By.Id("email"));
        emailField.Clear();
        emailField.SendKeys("user@gmail.com");

        Thread.Sleep(1000);
        IWebElement? descriptionField = this.driver?.FindElement(By.Id("claim_details"));
        descriptionField.Clear();
        descriptionField.SendKeys("aaa bbb ccc ddd");
        
        Thread.Sleep(1000);
        IWebElement? yearsField = this.driver?.FindElement(By.Id("age"));
        yearsField.Clear();
        yearsField.SendKeys(Keys.Backspace);
        yearsField.SendKeys("50");
        
        Thread.Sleep(1000);
        IWebElement? invUSDField = this.driver?.FindElement(By.Id("priceusd"));
        invUSDField.SendKeys(Keys.Backspace);
        invUSDField.SendKeys("20");
        invUSDField.SendKeys(Keys.Tab);
        
        Thread.Sleep(3000);
        IWebElement? courseField = this.driver?.FindElement(By.Id("course"));
        IWebElement? invUAHField = this.driver?.FindElement(By.Id("priceuah"));
        float invUAH = float.Parse(invUAHField.GetAttribute("value"));

        Code.AssertState(invUAH > 515 && invUAH < 516, "UAH is wrong!!!"); // 

        IWebElement? submitButton = this.driver?.FindElement(By.XPath("//form/button"));
        submitButton.Click();

        Thread.Sleep(2000);
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);",
                    this.driver?.FindElement(By.XPath("//footer")));
        
        Thread.Sleep(5000);

        IWebElement? listPageHeader = this.driver?.FindElement(By.XPath("//table"));
        int RowCount = this.driver.FindElements(By.XPath("//table/tbody//tr")).Count;
        IWebElement? tableElement = this.driver?.FindElement(By.XPath("//table/tbody/tr[" + RowCount + "]/td[1]"));
        string insertedText = tableElement.Text;

        Code.AssertState(string.Equals(insertedText, "Alex Blob"), "Item hasn't been added to the database"); 
    }


     [Test]
    public void PostRegisterWrongAge() 
     {
        
       this.driver?.Navigate().GoToUrl("http://192.168.56.3:5044/booking");
        Thread.Sleep(1000);
        int i = 1;
        for (; ; )
        {
            if (i > 10)
            {
                break;
            }
            else
            {
                Thread.Sleep(1000);
            }

            try
            {
                IWebElement? fnlnField1 = this.driver?.FindElement(By.Id("fnln"));
                fnlnField1.Clear();
                fnlnField1.SendKeys("Alex Blob");
            }
            catch
            {
                //ignored
            }
            break;
            i++;
        }
        Thread.Sleep(1000);
        IWebElement? uniField = this.driver?.FindElement(By.Id("claim"));
        uniField.Clear();
        uniField.SendKeys("aa bb cc");

        Thread.Sleep(1000);
        IWebElement? fnlnField = this.driver?.FindElement(By.Id("fnln"));
        fnlnField.Clear();
        fnlnField.SendKeys("Alex Blob");

        Thread.Sleep(1000);
        IWebElement? phoneField = this.driver?.FindElement(By.Id("phoneNumber"));
        phoneField.Clear();
        phoneField.SendKeys("+375336702727");

        Thread.Sleep(1000);
        IWebElement? emailField = this.driver?.FindElement(By.Id("email"));
        emailField.Clear();
        emailField.SendKeys("user@gmail.com");

        Thread.Sleep(1000);
        IWebElement? descriptionField = this.driver?.FindElement(By.Id("claim_details"));
        descriptionField.Clear();
        descriptionField.SendKeys("aaa bbb ccc ddd");
        
        Thread.Sleep(1000);
        IWebElement? yearsField = this.driver?.FindElement(By.Id("age"));
        yearsField.Clear();
        yearsField.SendKeys(Keys.Backspace);
        yearsField.SendKeys("11");
        
        Thread.Sleep(1000);
        IWebElement? invUSDField = this.driver?.FindElement(By.Id("priceusd"));
        invUSDField.SendKeys(Keys.Backspace);
        invUSDField.SendKeys("20");
        invUSDField.SendKeys(Keys.Tab);
        
        Thread.Sleep(3000);


        Thread.Sleep(3000);
        IWebElement? courseField = this.driver?.FindElement(By.Id("course"));
        IWebElement? invUAHField = this.driver?.FindElement(By.Id("priceuah"));
        float invUAH = float.Parse(invUAHField.GetAttribute("value"));

        Code.AssertState(invUAH > 515 && invUAH < 516, "UAH is wrong!!!"); // 

        IWebElement? submitButton = this.driver?.FindElement(By.XPath("//form/button"));
        submitButton.Click();

        Thread.Sleep(1000);

        IWebElement? alert = this.driver?.FindElement(By.XPath("//div[@class='alert alert-danger']"));
          
        Code.AssertState(!(alert is null), "Alert is Empty!"); 
        Code.AssertState((alert.Text.ToLower() != "Возраст должен быть более 18 лет!.".ToLower()), "Alert Message is wrong!"); 
    }


    [TearDown]
    public void endTests()
    {
        this.driver.Close();
    }
}