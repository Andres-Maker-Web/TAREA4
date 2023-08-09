using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading.Tasks;
using YourBook.Models;

namespace YourBook.Test
{
    [TestClass]
    public class YourBookTesting
    {
        private IWebDriver driver;
        private const string BaseUrl = "http://localhost:47250";

        [TestInitialize]
            public void TestInitialize()
            {
                driver = new ChromeDriver();
            }

            [TestCleanup]
            public void TestCleanup()
            {
                driver.Quit();
            }

        [TestMethod]
        public async Task TestBookSearch()
        {
            // Arrange
            string searchString = "PartialTitle"; // Cambiar a la cadena de búsqueda parcial deseada

            // Act
            driver.Navigate().GoToUrl(BaseUrl); //driver.Navigate().GoToUrl($"{BaseUrl}/Books/Create");
            var searchBox = driver.FindElement(By.Name("SearchString")); 
            searchBox.SendKeys(searchString);
            searchBox.Submit();

            // Assert
            var bookResults = driver.FindElements(By.CssSelector(".book-result"));
            foreach (var bookResult in bookResults)
            {
                var title = bookResult.FindElement(By.CssSelector(".book-title")).Text;
                Assert.IsTrue(title.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }
        }

        [TestMethod]
        public async Task TestBookRegistrationAndNotification()
        {
            // Arrange
            string title = "New Book Title";
            string rating = "5"; // Cambiar a la calificación deseada

            // Act
            driver.Navigate().GoToUrl(BaseUrl); //driver.Navigate().GoToUrl($"{BaseUrl}/Books/Create");
            driver.FindElement(By.LinkText("Create")).Click();
            var titleInput = driver.FindElement(By.Id("Title"));
            var ratingInput = driver.FindElement(By.Id("Rating"));

            titleInput.SendKeys(title);
            ratingInput.SendKeys(rating);

            driver.FindElement(By.CssSelector(".btn-primary")).Click();
            Thread.Sleep(1000); // Esperar un momento para que aparezca la notificación

            // Assert
            var successMessage = driver.FindElement(By.CssSelector(".alert-success")).Text;
            Assert.IsTrue(successMessage.Contains("El libro ha sido creado exitosamente"));
        }

        [TestMethod]
        public async Task TestUserLoginAndRecommendation()
        {
            // Arrange & Act
            driver.Navigate().GoToUrl(BaseUrl); //driver.Navigate().GoToUrl($"{BaseUrl}/Books/Create");

            // Assert
            var recommendationSection = driver.FindElement(By.CssSelector(".container.text-center"));
            Assert.IsNotNull(recommendationSection);

            var bookCards = driver.FindElements(By.CssSelector(".card.mb-3"));
            Assert.IsTrue(bookCards.Count > 0);

            foreach (var bookCard in bookCards)
            {
                var titleElement = bookCard.FindElement(By.CssSelector(".card-title"));
                var ratingElement = bookCard.FindElement(By.CssSelector(".card-subtitle.mb-2.text-body-secondary"));

                var title = titleElement.Text;
                var ratingText = ratingElement.Text;
                Assert.IsFalse(string.IsNullOrWhiteSpace(title));
                Assert.IsTrue(ratingText.EndsWith("%"));
            }
        }

        [TestMethod]
        public async Task TestUserAccountCreation()
        {
            // Arrange
            string username = "usuario@ejemplo.com"; // Cambiar al correo electrónico de un usuario válido
            string password = "contraseña"; // Cambiar a la contraseña correcta del usuario

            // Act
            driver.Navigate().GoToUrl(BaseUrl); //driver.Navigate().GoToUrl($"{BaseUrl}/Books/Create");
            driver.FindElement(By.LinkText("Log in")).Click();
            var usernameInput = driver.FindElement(By.Id("UserName"));
            var passwordInput = driver.FindElement(By.Id("Password"));
            var loginButton = driver.FindElement(By.CssSelector(".btn-outline-success"));

            usernameInput.SendKeys(username);
            passwordInput.SendKeys(password);
            loginButton.Click();
            Thread.Sleep(1000); // Esperar un momento para la redirección

            // Assert
            var pageTitle = driver.Title;
            Assert.AreEqual("YourBooks - Home Page", pageTitle); // Cambiar al título correcto de la página de inicio

            var welcomeMessage = driver.FindElement(By.CssSelector(".display-4")).Text;
            Assert.IsTrue(welcomeMessage.Contains("Bienvenido a 'YourBooks'"));

            var logoutLink = driver.FindElement(By.LinkText("Cerrar sesión"));
            Assert.IsNotNull(logoutLink);
        }

        [TestMethod]
        public void UserRegistration_Success()
        {
            // Arrange
            string username = "nuevo_usuario@ejemplo.com"; // Cambiar al correo electrónico de un nuevo usuario
            string password = "contraseña"; // Cambiar a la contraseña para el nuevo usuario

            // Act
            driver.Navigate().GoToUrl(BaseUrl); //driver.Navigate().GoToUrl($"{BaseUrl}/Books/Create");
            driver.FindElement(By.LinkText("Sign up")).Click();
            var usernameInput = driver.FindElement(By.Id("Username"));
            var emailInput = driver.FindElement(By.Id("EmailAddress"));
            var passwordInput = driver.FindElement(By.Id("Password"));
            var signUpButton = driver.FindElement(By.CssSelector(".btn-outline-success"));

            usernameInput.SendKeys(username);
            emailInput.SendKeys(username); // Usamos el mismo correo para demostrar duplicación de correo (se debe cambiar para pruebas reales)
            passwordInput.SendKeys(password);
            signUpButton.Click();
            Thread.Sleep(1000); // Esperar un momento para la redirección

            // Assert
            var notification = driver.FindElement(By.CssSelector(".alert.alert-success"));
            Assert.IsNotNull(notification);

            var notificationText = notification.Text;
            Assert.IsTrue(notificationText.Contains("Bienvenido")); // Verificar mensaje de bienvenida
            Assert.IsTrue(notificationText.Contains(username)); // Verificar que el nombre de usuario está en la notificación
        }
    }
}
