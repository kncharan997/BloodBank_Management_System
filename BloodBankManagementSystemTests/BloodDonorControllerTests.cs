using BloodBankManagementSystem.Controllers;
using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BloodBankManagementSystem.Tests
{
    public class BloodDonorControllerTests
    {
        private BloodBankContext _context;
        private BloodDonorController _controller;

        [SetUp]
        public void Setup()
        {
            // Use a unique in-memory database for each test
            var options = new DbContextOptionsBuilder<BloodBankContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB name for each test
                .Options;

            _context = new BloodBankContext(options);
            _controller = new BloodDonorController(_context);

            // Add a test BloodDonor record if not already added
            if (!_context.BloodDonors.Any(d => d.BloodDonorID == 1)) // Check if record exists before adding
            {
                var donor = new BloodDonor
                {
                    BloodDonorID = 1,
                    Name = "John Doe",
                    BloodType = "O+",
                    Address = "123 Test Street",
                    ContactNumber = "9876543210",
                    Age = 25, // 25 years old
                    LastDonationDate = DateTime.Now.AddMonths(-3), // Last donation 3 months ago
                    TotalDonations = 5
                };
                _context.BloodDonors.Add(donor);
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
        public void Index_ReturnsAViewResult_WithAListOfBloodDonors()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as System.Collections.Generic.List<BloodDonor>;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);  // There should be one donor record
        }

        [Test]
        public void Details_ReturnsNotFound_WhenDonorIsNotFound()
        {
            // Act
            var result = _controller.Details(999);  // ID that doesn't exist

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Details_ReturnsAViewResult_WithCorrectBloodDonor()
        {
            // Act
            var result = _controller.Details(1);  // Existing ID

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as BloodDonor;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.BloodDonorID);
        }

        [Test]
        public void Create_ReturnsRedirectToAction_WhenModelStateIsValid()
        {
            // Arrange
            var donor = new BloodDonor
            {
                Name = "Jolly",
                BloodType = "A+",
                Address = "456 New Street",
                ContactNumber = "9675876545",
                Age = 30, // 30 years old
                LastDonationDate = DateTime.Now.AddMonths(-6), // Last donation 6 months ago
                Disease = "No",
                TotalDonations = 2,
                HospitalID = 1
            };

            // Act
            var result = _controller.Create(donor);

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName); // Redirect to Index after creation
        }

        [Test]
        public void Edit_ReturnsNotFound_WhenDonorDoesNotExist()
        {
            // Act
            var result = _controller.Edit(999, new BloodDonor());  // ID that doesn't exist

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Edit_ReturnsRedirectToAction_WhenModelStateIsValid()
        {
            // Arrange
            var donor = _context.BloodDonors.First();
            donor.Name = "Updated Name";
            donor.ContactNumber = "1234567890";
            donor.TotalDonations = 6;

            // Act
            var result = _controller.Edit(donor.BloodDonorID, donor);

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);  // Redirect to Index after edit
        }

        [Test]
        public void DeleteConfirmed_RemovesBloodDonor_WhenIdIsValid()
        {
            // Act
            var result = _controller.DeleteConfirmed(1);  // Existing ID

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var donor = _context.BloodDonors.FirstOrDefault(d => d.BloodDonorID == 1);
            Assert.IsNull(donor);  // BloodDonor should be removed
        }

        [Test]
        public void DonateBlood_ReturnsNotFound_WhenDonorIsNotFound()
        {
            // Act
            var result = _controller.DonateBlood(999);  // ID that doesn't exist

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }


    }
}
