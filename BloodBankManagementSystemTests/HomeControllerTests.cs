using NUnit.Framework;

using Moq;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Http;
using BloodBankManagementSystem.Controllers;

namespace BloodBankManagementSystem.Tests
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController _controller;
        private Mock<ISession> _mockSession;
        private Mock<HttpContext> _mockHttpContext;

        [SetUp]
        public void Setup()
        {
            // Mock session
            _mockSession = new Mock<ISession>();

            // Mock HttpContext
            var defaultHttpContext = new DefaultHttpContext();
            defaultHttpContext.Session = _mockSession.Object;

            var mockqHttpContextAccessor = new Mock<HttpContextAccessor>();
            //var mockHttpcontext = new DefaultHttpContext();

            //mockqHttpContextAccessor.Setup(a => a.HttpContext).Returns(defaultHttpContext);
            var httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = defaultHttpContext
            };
            // Initialize Controller
            _controller = new HomeController(mockqHttpContextAccessor.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = defaultHttpContext
            };
        }

        [Test]
        public void Index_UserNotLoggedIn_RedirectsToLogin()
        {
            // Arrange: No UserID in session (returns null)
            _mockSession.Setup(s => s.TryGetValue("UserID", out It.Ref<byte[]>.IsAny)).Returns(false);

            // Act
            var result = _controller.Index() as RedirectToActionResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo("Login"));
            Assert.That(result.ControllerName, Is.EqualTo("Account"));
        }

        [Test]
        public void Index_UserLoggedIn_ReturnsViewResult()
        {
            // Arrange: UserID is set in session
            var userIdBytes = BitConverter.GetBytes(1);

            _mockSession.Setup(s => s.TryGetValue("UserID", out It.Ref<byte[]>.IsAny)).Returns((string key, out byte[] value) =>
            {
                value = userIdBytes;
                return true;
            });

            // Act
            var result = _controller.Index() as ViewResult;
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }
    }
}
