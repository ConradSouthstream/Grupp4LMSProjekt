using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace LMS.Grupp4.Test
{
    //Test that Entityframework 5 is correctly connecting to our SQL server database by using some Nunit tests.
    //These are commonly called integration tests as we are testing directly against our SQL server database 
    [TestFixture]
    public class SelectTests
    {
        private ApplicationDbContext _context;
        [SetUp]
        public void SetUp()
        {
            _context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-LMS.Grupp4.Web-436A730E-1472-4495-9A0F-482552BBED9C;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options);

        }
        [Test]

        public void GetAllCourses()
        {
            IEnumerable<Kurs> courses =  _context.Kurser.ToList();
            Assert.AreEqual(10, courses.Count());
        }
        [Test]
        public void CoursesHaveModules()
        {
            List<Kurs> courses = _context.Kurser.Include(a=>a.Moduler).ToList();
            Assert.AreEqual(5, courses[0].Moduler.Count);
            Assert.AreEqual(5, courses[1].Moduler.Count);

        }

    }
}
