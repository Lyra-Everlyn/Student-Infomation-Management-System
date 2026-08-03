using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SIMS.Controllers;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Repositories.Implementations;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Implementations;
using SIMS.Services.Interfaces;
using SIMS_System.Controllers;
using System.Security.Claims;
using Xunit;

namespace SIMS.Tests
{
    public class SIMSTestSuite
    {
        #region TC_UT_01: Unit Test - Admin Auto Assign Students to Course
        [Fact]
        public async Task TC_UT_01_Admin_AutoAssignStudents_ShouldGenerateCorrectEnrollments()
        {
            int targetClassId = 1;
            var newCourse = new Course { CourseName = "C# Programming", SubjectId = 1 };

            var options = new DbContextOptionsBuilder<SimsDbContext>()
                .UseInMemoryDatabase(databaseName: "TC_UT_01_DB")
                .Options;
            using var context = new SimsDbContext(options);

            context.Students.AddRange(
                new Student { StudentId = 1, StudentCode = "BH001", ClassId = targetClassId },
                new Student { StudentId = 2, StudentCode = "BH002", ClassId = targetClassId },
                new Student { StudentId = 3, StudentCode = "BH003", ClassId = targetClassId }
            );
            await context.SaveChangesAsync();

            var mockCourseRepo = new Mock<ICourseRepository>();
            var mockSubjectRepo = new Mock<ISubjectRepository>();
            var mockClassRepo = new Mock<IClassRepository>();
            var mockFacultyRepo = new Mock<IFacultyRepository>();

            var courseService = new CourseService(
                mockCourseRepo.Object,
                mockSubjectRepo.Object,
                mockClassRepo.Object,
                mockFacultyRepo.Object,
                context
            );

            // Act
            var result = await courseService.CreateCourseAsync(newCourse, targetClassId);

            // Assert
            Assert.True(result.Success);
            var enrollmentsCreated = await context.Enrollments.Where(e => e.CourseId == newCourse.CourseId).ToListAsync();
            Assert.Equal(3, enrollmentsCreated.Count);
            mockCourseRepo.Verify(r => r.AddAsync(It.IsAny<Course>()), Times.Once);
        }
        #endregion

        #region TC_UT_02: Unit Test - Login Routing by Role
        [Fact]
        public async Task TC_UT_02_Login_FacultyRole_ShouldRouteToFacultyDashboard()
        {
            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(s => s.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync(new User
                           {
                               Email = "faculty@fpt.edu.vn",
                               FullName = "Teacher Test",
                               Role = "Faculty"
                           });

            var controller = new AuthController(mockAuthService.Object);

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddControllersWithViews();

            var mockAuthSchemeProvider = new Mock<IAuthenticationService>();
            mockAuthSchemeProvider
                .Setup(x => x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);
            services.AddSingleton<IAuthenticationService>(mockAuthSchemeProvider.Object);

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("https://localhost/Faculty/FacultyDashboard");

            var mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
            mockUrlHelperFactory
                .Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>()))
                .Returns(mockUrlHelper.Object);
            services.AddSingleton<IUrlHelperFactory>(mockUrlHelperFactory.Object);

            var serviceProvider = services.BuildServiceProvider();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = serviceProvider
                }
            };

            var result = await controller.Login("faculty@fpt.edu.vn", "123456") as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("FacultyDashboard", result.ActionName);
            Assert.Equal("Faculty", result.ControllerName);
        }
        #endregion

        #region TC_IT_01: Integration Test - Create Course and Save to DB
        [Fact]
        public async Task TC_IT_01_Admin_CreateCourse_ShouldSaveToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SimsDbContext>()
                .UseInMemoryDatabase(databaseName: "TC_IT_01_DB")
                .Options;

            using var context = new SimsDbContext(options);
            context.Faculties.Add(new Faculty { FacultyId = 10, DepartmentId = 1, UserId = 1 });
            await context.SaveChangesAsync();

            var courseRepo = new CourseRepository(context);
            var mockSubjectRepo = new Mock<ISubjectRepository>();
            var mockClassRepo = new Mock<IClassRepository>();
            var mockFacultyRepo = new Mock<IFacultyRepository>();

            var courseService = new CourseService(
                courseRepo,
                mockSubjectRepo.Object,
                mockClassRepo.Object,
                mockFacultyRepo.Object,
                context
            );

            var newCourse = new Course
            {
                CourseName = "Advanced Mathematics",
                FacultyId = 10,
                SubjectId = 1
            };

            // Act
            var result = await courseService.CreateCourseAsync(newCourse, 0);

            // Assert
            Assert.True(result.Success);
            var savedCourse = await context.Courses.FirstOrDefaultAsync(c => c.CourseName == "Advanced Mathematics");
            Assert.NotNull(savedCourse);
            Assert.Equal(10, savedCourse.FacultyId);
            Assert.StartsWith("CRS-", savedCourse.CourseCode);
        }
        #endregion

        #region TC_IT_02: Integration Test - Instructor Grading Update
        [Fact]
        public async Task TC_IT_02_Faculty_GradeSubmission_ShouldUpdateDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SimsDbContext>()
                .UseInMemoryDatabase(databaseName: "TC_IT_02_DB")
                .Options;

            using var context = new SimsDbContext(options);

            var initialGrade = new Grade
            {
                GradeId = 1,
                AssignmentId = 1,
                StudentId = 1,
                Score = null,
                Feedback = null
            };
            context.Grades.Add(initialGrade);
            await context.SaveChangesAsync();

            var gradeRepo = new Repository<Grade>(context);

            var gradeService = new GradeService(gradeRepo);

            decimal newScore = 8.5m;
            string newFeedback = "Very good!";

            // Act
            await gradeService.GradeSubmissionAsync(initialGrade.GradeId, newScore, newFeedback);

            // Assert
            using var assertContext = new SimsDbContext(options);
            var updatedGrade = await assertContext.Grades.FindAsync(initialGrade.GradeId);

            Assert.NotNull(updatedGrade);
            Assert.Equal(newScore, updatedGrade.Score);
            Assert.Equal(newFeedback, updatedGrade.Feedback);
            Assert.NotNull(updatedGrade.GradedAt);
        }
        #endregion
    }
}