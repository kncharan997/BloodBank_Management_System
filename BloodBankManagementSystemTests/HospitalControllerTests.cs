using BloodBankManagementSystem.Controllers;
using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BloodBankManagementSystemTests
{
    [TestFixture]
    public class HospitalControllerTests
    {
        private BloodBankContext _context;
        private HospitalController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<BloodBankContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new BloodBankContext(options);
            _context.Database.EnsureCreated();

            // Seed data
            _context.Hospitals.AddRange(new List<Hospital>
            {
                new Hospital { HospitalID = 1, Name = "City Hospital", City = "Hyderabad", Contact = "1234567890", Address = "Street 1" },
                new Hospital { HospitalID = 2, Name = "Metro Hospital", City = "Mumbai", Contact = "0987654321", Address = "Street 2" }
            });
            _context.SaveChanges();

            _controller = new HospitalController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _controller?.Dispose();
        }

        [Test]
        public void Index_ReturnsViewWithHospitals()
        {
            var result = _controller.Index() as ViewResult;
            var model = result?.Model as List<Hospital>;

            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.That(model?.Count, Is.EqualTo(2));
        }

        [Test]
        public void Details_ValidId_ReturnsHospital()
        {
            var result = _controller.Details(1) as ViewResult;
            var hospital = result?.Model as Hospital;

            Assert.NotNull(result);
            Assert.NotNull(hospital);
            Assert.That(hospital?.HospitalID, Is.EqualTo(1));
        }

        [Test]
        public void Details_InvalidId_ReturnsNotFound()
        {
            var result = _controller.Details(99);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Create_ValidHospital_RedirectsToIndex()
        {
            var newHospital = new Hospital { HospitalID = 3, Name = "Apollo Hospital", City = "Delhi", Contact = "1111111111", Address = "Street 3" };

            var result = _controller.Create(newHospital);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.That(_context.Hospitals.Count(), Is.EqualTo(3)); // Ensure data is added
        }

        [Test]
        public void Create_InvalidHospital_ReturnsView()
        {
            _controller.ModelState.AddModelError("Name", "Required");

            var result = _controller.Create(new Hospital()) as ViewResult;

            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Edit_ValidId_ReturnsViewWithHospital()
        {
            var result = _controller.Edit(1) as ViewResult;
            var hospital = result?.Model as Hospital;

            Assert.NotNull(result);
            Assert.NotNull(hospital);
            Assert.That(hospital?.HospitalID, Is.EqualTo(1));
        }

        [Test]
        public void Edit_InvalidId_ReturnsNotFound()
        {
            var result = _controller.Edit(99);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Delete_ValidId_ReturnsViewWithHospital()
        {
            var result = _controller.Delete(1) as ViewResult;
            var hospital = result?.Model as Hospital;

            Assert.NotNull(result);
            Assert.NotNull(hospital);
            Assert.That(hospital?.HospitalID, Is.EqualTo(1));
        }

        [Test]
        public void Delete_InvalidId_ReturnsNotFound()
        {
            var result = _controller.Delete(99);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
