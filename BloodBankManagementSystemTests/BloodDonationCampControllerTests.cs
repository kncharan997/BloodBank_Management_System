using BloodBankManagementSystem.Controllers;
using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BloodBankManagementSystem.Tests
{
    public class BloodDonationCampControllerTests
    {
        private BloodBankContext _context;
        private BloodDonationCampController _controller;

        [SetUp]
        public void Setup()
        {
            // Use a unique in-memory database for each test
            var options = new DbContextOptionsBuilder<BloodBankContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB name for each test
                .Options;

            _context = new BloodBankContext(options);
            _controller = new BloodDonationCampController(_context);

            // Add a test BloodDonationCamp record if not already added
            if (!_context.BloodDonationCamps.Any(c => c.BloodDonationCampID == 1)) // Check if record exists before adding
            {
                var camp = new BloodDonationCamp
                {
                    BloodDonationCampID = 1,
                    CampName = "Test Camp",
                    Address = "123 Test Street",
                    City = "Test City",
                    CampStartDate = DateTime.Now.AddDays(1), // Start date in the future
                    CampEndDate = DateTime.Now.AddDays(2)
                };
                _context.BloodDonationCamps.Add(camp);
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
        public void Index_ReturnsAViewResult_WithAListOfBloodDonationCamps()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as System.Collections.Generic.List<BloodDonationCamp>;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);  // There should be one record
        }

        [Test]
        public void Details_ReturnsNotFound_WhenCampIsNotFound()
        {
            // Act
            var result = _controller.Details(999);  // ID that doesn't exist

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Details_ReturnsAViewResult_WithCorrectBloodDonationCamp()
        {
            // Act
            var result = _controller.Details(1);  // Existing ID

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as BloodDonationCamp;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.BloodDonationCampID);
        }

        [Test]
        public void Create_ReturnsRedirectToAction_WhenModelStateIsValid()
        {
            // Arrange
            var camp = new BloodDonationCamp
            {
                CampName = "New Camp",
                Address = "456 New Street",
                City = "New City",
                CampStartDate = DateTime.Now.AddDays(1),
                CampEndDate = DateTime.Now.AddDays(2)
            };

            // Act
            var result = _controller.Create(camp);

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName); // Redirect to Index after creation
        }

        [Test]
        public void Edit_ReturnsNotFound_WhenCampDoesNotExist()
        {
            // Act
            var result = _controller.Edit(999, new BloodDonationCamp());  // ID that doesn't exist

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Edit_ReturnsRedirectToAction_WhenModelStateIsValid()
        {
            // Arrange
            var camp = _context.BloodDonationCamps.First();
            camp.CampName = "Updated Camp";
            camp.Address = "Updated Address";
            camp.CampStartDate = DateTime.Now.AddDays(1);
            camp.CampEndDate = DateTime.Now.AddDays(3);

            // Act
            var result = _controller.Edit(camp.BloodDonationCampID, camp);

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);  // Redirect to Index after edit
        }

        [Test]
        public void DeleteConfirmed_RemovesBloodDonationCamp_WhenIdIsValid()
        {
            // Act
            var result = _controller.DeleteConfirmed(1);  // Existing ID

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var camp = _context.BloodDonationCamps.FirstOrDefault(c => c.BloodDonationCampID == 1);
            Assert.IsNull(camp);  // BloodDonationCamp should be removed
        }
    }
}
