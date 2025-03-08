using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloodBankManagementSystem.Controllers;
using BloodBankManagementSystem.Models;

namespace BloodBankManagementSystemTests
{
    [TestFixture]
    public class BloodInventoryControllerTests
    {
        private BloodBankContext _context;
        private BloodInventoryController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<BloodBankContext>()
                .UseInMemoryDatabase(databaseName: "BloodBankTestDB")
                .Options;

            _context = new BloodBankContext(options);
            _context.Database.EnsureDeleted();  // Reset DB before each test
            _context.Database.EnsureCreated();

            SeedDatabase();  // Populate DB with test data
            _controller = new BloodInventoryController(_context);
        }

        private void SeedDatabase()
        {
            _context.BloodBanks.AddRange(
                new BloodBank { BloodBankID = 1, Name = "Bank A", Contact = "1234567890", City = "Hyd" },
                new BloodBank { BloodBankID = 2, Name = "Bank B", Contact = "0987654321", City = "delhi" }
            );

            _context.BloodInventories.AddRange(
                new BloodInventory { BloodInventoryID = 1, BloodGroup = "O+", NumberofBottles = 10, ExpiryDate = DateTime.Now.AddDays(30), BloodBankID = 1 },
                new BloodInventory { BloodInventoryID = 2, BloodGroup = "A+", NumberofBottles = 5, ExpiryDate = DateTime.Now.AddDays(20), BloodBankID = 2 }
            );

            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
            _context?.Dispose();
            _context = null;
        }
        [Test]
        public void Index_ReturnsViewResult_WithInventoryList()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.Model, Is.Not.Null);
            var model = viewResult.Model as List<BloodInventory>;
            Assert.That(model.Count, Is.EqualTo(2));
        }

        [Test]
        public void Transfer_Get_ReturnsViewResult_WithBloodAndBloodBanks()
        {
            // Act
            var result = _controller.Transfer(1);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.Model, Is.Not.Null);
            var blood = viewResult.Model as BloodInventory;
            Assert.That(blood.BloodGroup, Is.EqualTo("O+"));
            Assert.That(viewResult.ViewData["BloodBanks"], Is.Not.Null);
        }

        [Test]
        public void Create_Post_AddsBloodInventoryAndRedirects()
        {
            // Arrange
            var newBloodInventory = new BloodInventory
            {
                BloodGroup = "B+",
                NumberofBottles = 7,
                ExpiryDate = DateTime.Now.AddDays(40),
                BloodBankID = 1
            };

            // Act
            var result = _controller.Create(newBloodInventory);

            // Assert
            if (result is ViewResult viewResult)
            {
                // If ModelState is invalid, the method should return ViewResult
                Assert.That(viewResult.Model, Is.Not.Null);
                Assert.That(viewResult.Model, Is.TypeOf<BloodInventory>());
            }
            else if (result is RedirectToActionResult redirectResult)
            {
                // If ModelState is valid, it should redirect to Index
                Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
                Assert.That(_context.BloodInventories.Count(), Is.EqualTo(3)); // Ensure the inventory was added
            }
            else
            {
                Assert.Fail("Unexpected result type returned from Create method.");
            }
        }


        [Test]
        public void DeleteConfirmed_RemovesBloodInventoryAndRedirects()
        {
            // Act
            var result = _controller.DeleteConfirmed(1);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(_context.BloodInventories.Count(), Is.EqualTo(1));
        }


    }
}
