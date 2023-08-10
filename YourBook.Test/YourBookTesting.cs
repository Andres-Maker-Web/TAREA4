using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading.Tasks;
using YourBook.Controllers;
using YourBook.Data;
using YourBook.Models;

namespace YourBook.Test
{
    [NUnit.Framework.TestFixture]
    public class YourBookTesting
    {
        private IWebDriver _driver;
        private YourBookDbContext _dbContext;
        private ILogger<HomeController> _logger;
        private SignInManager<AccountUser> _signInManager;
        private UserManager<AccountUser> _userManager;




        [SetUp]
        public void SetUp()
        {
            // Configurar el navegador Chrome
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();

            // Configurar la base de datos utilizando la misma cadena de conexión que en tu aplicación
            var optionsBuilder = new DbContextOptionsBuilder<YourBookDbContext>();
            optionsBuilder.UseSqlServer("Data Source=SERVER\\MSSQLSERVERDEV;Initial Catalog=YourBooks;Integrated Security=True;TrustServerCertificate=True");

            _dbContext = new YourBookDbContext(optionsBuilder.Options);
            _dbContext.Database.EnsureCreated();


            // Configurar SignInManager y UserManager
            var userStore = new UserStore<AccountUser>(_dbContext);
            _userManager = new UserManager<AccountUser>(userStore, null, null, null, null, null, null, null, null);
            _signInManager = new SignInManager<AccountUser>(_userManager, _driver, null);

            
        }

        [NUnit.Framework.Test]
        public async Task Index_Should_Return_Books_With_SearchString()
        {
            // Arrange
            var controller = new BooksController(_dbContext);
            var searchString = "Programacion";

            // Agregar un libro de ejemplo a la base de datos
            _dbContext.Books.Add(new Book { Title = searchString, Rating = 90 });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await controller.Index(searchString) as ViewResult;
            var model = result?.Model as List<Book>;

            // Assert
            NUnit.Framework.Assert.IsNotNull(result);
            NUnit.Framework.Assert.AreEqual(searchString, result.ViewData["SearchString"]);
            NUnit.Framework.Assert.IsNotNull(model);
            NUnit.Framework.Assert.AreEqual(1, model.Count);
            NUnit.Framework.Assert.AreEqual(searchString, model[0].Title);
        }

        [NUnit.Framework.Test]
        public async Task Create_Post_Should_Create_Book()
        {
            // Arrange
            var controller = new BooksController(_dbContext);
            var book = new Book { Title = "Test Book", Rating = 90 };

            // Act
            var result = await controller.Create(book) as RedirectToActionResult;

            // Assert
            NUnit.Framework.Assert.IsNotNull(result);
            NUnit.Framework.Assert.AreEqual("Index", result.ActionName);
        }

        [NUnit.Framework.Test]
        public async Task Index_Should_Display_Top_Rated_Books()
        {
            // Arrange
            var controller = new HomeController(_logger, _dbContext);

            // Agregar libros de ejemplo a la base de datos
            _dbContext.Books.Add(new Book { Title = "Book 1", Rating = 90 });
            _dbContext.Books.Add(new Book { Title = "Book 2", Rating = 85 });
            _dbContext.Books.Add(new Book { Title = "Book 3", Rating = 95 });
            _dbContext.SaveChanges();

            // Act
            var result = await controller.Index() as ViewResult;
            var model = result?.Model as IEnumerable<Book>;

            // Assert
            NUnit.Framework.Assert.IsNotNull(result);
            NUnit.Framework.Assert.AreEqual("Home Page", result.ViewData["Title"]);
            NUnit.Framework.Assert.IsNotNull(model);
            NUnit.Framework.Assert.AreEqual(3, model.Count());
            // Add more assertions as needed to validate the displayed books
        }

        [NUnit.Framework.Test]
        public void Register_Should_Display_RegisterView()
        {
            // Arrange
            var controller = new AccountController(_userManager, _signInManager);

            // Act
            var result = controller.Register() as ViewResult;

            // Assert
            NUnit.Framework.Assert.IsNotNull(result);
            NUnit.Framework.Assert.AreEqual("Sign up", result.ViewData["Title"]);
        }

        [TearDown]
        public void TearDown()
        {
            // Limpiar y cerrar recursos
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();

            _driver.Quit();
            _driver.Dispose();
        }



        //[NUnit.Framework.Test]
        //public async Task TestBookRegistrationAndNotification()
        //{
        //    // Arrange
        //    string title = "New Book Title";
        //    string rating = "5"; // Cambiar a la calificación deseada

        //    // Act
        //    driver.Navigate().GoToUrl($"{BaseUrl}/Books/Create"); //driver.Navigate().GoToUrl($"{BaseUrl}/Books/Create");
        //    driver.FindElement(By.LinkText("Create")).Click();
        //    var titleInput = driver.FindElement(By.Id("Title"));
        //    var ratingInput = driver.FindElement(By.Id("Rating"));

        //    titleInput.SendKeys(title);
        //    ratingInput.SendKeys(rating);

        //    driver.FindElement(By.CssSelector(".btn-primary")).Click();
        //    Thread.Sleep(1000); // Esperar un momento para que aparezca la notificación

        //    // Assert
        //    var successMessage = driver.FindElement(By.CssSelector(".alert-success")).Text;
        //    NUnit.Framework.Assert.IsTrue(successMessage.Contains("El libro ha sido creado exitosamente"));
        //}

        //[NUnit.Framework.Test]
        //public async Task TestRecommendation()
        //{
        //    // Arrange & Act
        //    driver.Navigate().GoToUrl($"{BaseUrl}/Home/Index"); //driver.Navigate().GoToUrl($"{BaseUrl}/Books/Create");

        //    // Assert
        //    var recommendationSection = driver.FindElement(By.CssSelector(".container.text-center"));
        //    NUnit.Framework.Assert.IsNotNull(recommendationSection);

        //    var bookCards = driver.FindElements(By.CssSelector(".card.mb-3"));
        //    NUnit.Framework.Assert.IsTrue(bookCards.Count > 0);

        //    foreach (var bookCard in bookCards)
        //    {
        //        var titleElement = bookCard.FindElement(By.CssSelector(".card-title"));
        //        var ratingElement = bookCard.FindElement(By.CssSelector(".card-subtitle.mb-2.text-body-secondary"));

        //        var title = titleElement.Text;
        //        var ratingText = ratingElement.Text;
        //        NUnit.Framework.Assert.IsFalse(string.IsNullOrWhiteSpace(title));
        //        NUnit.Framework.Assert.IsTrue(ratingText.EndsWith("%"));
        //    }
        //}

        //[NUnit.Framework.Test]
        //public async Task TestUserAccountCreation()
        //{
        //    // Arrange
        //    string username = "usuario@ejemplo.com"; // Cambiar al correo electrónico de un usuario válido
        //    string password = "contraseña"; // Cambiar a la contraseña correcta del usuario

        //    // Act
        //    driver.Navigate().GoToUrl($"{BaseUrl}/Home/Index"); //driver.Navigate().GoToUrl($"{BaseUrl}/Books/Create");
        //    driver.FindElement(By.LinkText("Log in")).Click();
        //    var usernameInput = driver.FindElement(By.Id("UserName"));
        //    var passwordInput = driver.FindElement(By.Id("Password"));
        //    var loginButton = driver.FindElement(By.CssSelector(".btn-outline-success"));

        //    usernameInput.SendKeys(username);
        //    passwordInput.SendKeys(password);
        //    loginButton.Click();
        //    Thread.Sleep(1000); // Esperar un momento para la redirección

        //    // Assert
        //    var pageTitle = driver.Title;
        //    NUnit.Framework.Assert.AreEqual("YourBooks - Home Page", pageTitle); // Cambiar al título correcto de la página de inicio

        //    var welcomeMessage = driver.FindElement(By.CssSelector(".display-4")).Text;
        //    NUnit.Framework.Assert.IsTrue(welcomeMessage.Contains("Bienvenido a 'YourBooks'"));

        //    var logoutLink = driver.FindElement(By.LinkText("Cerrar sesión"));
        //    NUnit.Framework.Assert.IsNotNull(logoutLink);
        //}

        //[NUnit.Framework.Test]
        //public void TestUserRegistration()
        //{
        //    // Arrange
        //    string username = "nuevo_usuario@ejemplo.com"; // Cambiar al correo electrónico de un nuevo usuario
        //    string password = "contraseña"; // Cambiar a la contraseña para el nuevo usuario

        //    // Act
        //    driver.Navigate().GoToUrl($"{BaseUrl}/Home/Index"); //driver.Navigate().GoToUrl($"{BaseUrl}/Books/Create");
        //    driver.FindElement(By.LinkText("Sign up")).Click();
        //    var usernameInput = driver.FindElement(By.Id("Username"));
        //    var emailInput = driver.FindElement(By.Id("EmailAddress"));
        //    var passwordInput = driver.FindElement(By.Id("Password"));
        //    var signUpButton = driver.FindElement(By.CssSelector(".btn-outline-success"));

        //    usernameInput.SendKeys(username);
        //    emailInput.SendKeys(username); // Usamos el mismo correo para demostrar duplicación de correo (se debe cambiar para pruebas reales)
        //    passwordInput.SendKeys(password);
        //    signUpButton.Click();
        //    Thread.Sleep(1000); // Esperar un momento para la redirección

        //    // Assert
        //    var notification = driver.FindElement(By.CssSelector(".alert.alert-success"));
        //    NUnit.Framework.Assert.IsNotNull(notification);

        //    var notificationText = notification.Text;
        //    NUnit.Framework.Assert.IsTrue(notificationText.Contains("Bienvenido")); // Verificar mensaje de bienvenida
        //    NUnit.Framework.Assert.IsTrue(notificationText.Contains(username)); // Verificar que el nombre de usuario está en la notificación
        //}
    }
}
