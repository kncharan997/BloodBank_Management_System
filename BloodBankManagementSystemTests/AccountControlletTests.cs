using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using BloodBankManagementSystem.Models;
using Moq;
namespace BloodBankManagementSystem.Tests
{
    [TestFixture]
    public class AccountControllerTests
    {
        private AccountController _controller;
        private BloodBankContext _context;

        [SetUp]
        public void Setup()
        {
            // Create a fresh in-memory database for each test to ensure isolation.
            var options = new DbContextOptionsBuilder<BloodBankContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())  // Use a unique name to avoid conflicts
                .Options;

            _context = new BloodBankContext(options);

            // Adding initial test data for each test (if necessary)
            _context.Users.Add(new User { UserID = 1, UserName = "testuser", Password = "password123" });
            _context.SaveChanges();

            // Initialize the controller
            _controller = new AccountController(_context);

            // Create mocks for HttpContext and Session
            var mockHttpContext = new Mock<HttpContext>();
            var mockSession = new Mock<ISession>();

            // Setup the session mock behavior
            mockHttpContext.Setup(s => s.Session).Returns(mockSession.Object);

            // Set the HttpContext in the controller's ControllerContext
            _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        }


        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
            _context?.Dispose();
            _context = null;
        }


        [Test]
        public async Task Login_ValidUser_ReturnsRedirectToHome()
        {

            var result = await _controller.Login("testuser", "password123");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
        }

        [Test]
        public async Task Login_InvalidUser_ReturnsViewWithErrorMessage()
        {


            // Act
            var result = await _controller.Login("wronguser", "wrongpassword");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsTrue(viewResult.ViewData.ContainsKey("ErrorMessage"));
            Assert.AreEqual("Invalid username or password.", viewResult.ViewData["ErrorMessage"]);
        }

        [Test]
        public async Task Register_NewUser_ReturnsRedirectToLogin()
        {

            // Arrange
            var newUser = new User { UserID = 2, UserName = "newuser", Password = "newpassword" };

            //// Act
            var result = _controller.Register(newUser);

            //// Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Login", redirectResult.ActionName);
        }

        [Test]
        public async Task Register_ExistingUser_ReturnsViewWithErrorMessage()
        {
            // Arrange

            var existingUser = new User { UserID = 3, UserName = "testuser", Password = "password123" };

            // Act
            var result = _controller.Register(existingUser);

            //Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.IsTrue(viewResult.ViewData.ModelState.ContainsKey("UserName"));
            Assert.AreEqual("UserName already exists", viewResult.ViewData.ModelState["UserName"].Errors.First().ErrorMessage);
        }

        [Test]
        public void Logout_ClearsSessionAndRedirectsToLogin()
        {

            // Act
            var result = _controller.Logout();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Login", redirectResult.ActionName);
        }
    }
}
