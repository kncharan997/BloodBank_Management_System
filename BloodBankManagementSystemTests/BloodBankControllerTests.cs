using BloodBankManagementSystem.Controllers;
using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloodBankManagementSystem.Tests
{
    public class BloodBankControllerTests
    {
        private BloodBankContext _context;
        private BloodBankController _controller;

        [SetUp]
        public void Setup()
        {
            // Use a unique in-memory database for each test to avoid conflicts between tests
            var options = new DbContextOptionsBuilder<BloodBankContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB name for each test
                .Options;

            _context = new BloodBankContext(options);
            _controller = new BloodBankController(_context);

            // Add a test BloodBank record if not already added
            if (!_context.BloodBanks.Any(b => b.BloodBankID == 1)) // Check if record exists before adding
            {
                var bloodBank = new BloodBank
                {
                    BloodBankID = 1,
                    Name = "Test BloodBank",
                    City = "Test City",
                    Contact = "1234567890"
                };
                _context.BloodBanks.Add(bloodBank);
                _context.SaveChanges();
            }
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
            _context?.Dispose();
            _context = null;
        }

        [Test]
        public void Index_ReturnsAViewResult_WithAListOfBloodBanks()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as List<BloodBank>;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);
        }

        [Test]
        public void Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = _controller.Details(null);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Details_ReturnsAViewResult_WithCorrectBloodBank()
        {
            // Act
            var result = _controller.Details(1);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as BloodBank;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.BloodBankID);
        }

        [Test]
        public void Create_ReturnsRedirectToAction_WhenModelStateIsValid()
        {
            // Arrange
            var bloodBank = new BloodBank
            {
                Name = "New BloodBank",
                City = "New City",
                Contact = "0987654321"
            };

            // Act
            var result = _controller.Create(bloodBank);

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public void Edit_ReturnsNotFound_WhenBloodBankDoesNotExist()
        {
            // Act
            var result = _controller.Edit(999);  // ID that doesn't exist

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Edit_ReturnsViewResult_WhenModelStateIsValid()
        {
            // Arrange
            var bloodBank = _context.BloodBanks.First();
            bloodBank.Name = "Updated BloodBank";
            bloodBank.Contact = "0000000000";

            // Act
            var result = _controller.Edit(bloodBank.BloodBankID, bloodBank);

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public void DeleteConfirmed_RemovesBloodBank_WhenIdIsValid()
        {
            // Act
            var result = _controller.DeleteConfirmed(1);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var bloodBank = _context.BloodBanks.FirstOrDefault(b => b.BloodBankID == 1);
            Assert.IsNull(bloodBank);  // Blood bank should be removed
        }
    }
}
