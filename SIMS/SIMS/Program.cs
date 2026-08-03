using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Repositories.Implementations;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Implementations;
using SIMS.Services.Interfaces;

namespace SIMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Config DbContext
            builder.Services.AddDbContext<SimsDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. Register Repositories & Services
            // Repository
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IAdmissionApplicationRepository, AdmissionApplicationRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IClassRepository, ClassRepository>();
            builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            builder.Services.AddScoped<IRepository<Assignment>, Repository<Assignment>>();
            
            builder.Services.AddScoped<IAllCourseDetailsRepository, AllCourseDetailsRepository>();
            builder.Services.AddScoped<IRepository<Grade>, Repository<Grade>>();

            // Service
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAdmissionService, AdmissionService>();
            builder.Services.AddScoped<ISendAdmissionService, SendAdmissionService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IClassService, ClassService>();
            builder.Services.AddScoped<ISubjectService, SubjectService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IStudentManagerService, StudentManagerService>();
            builder.Services.AddScoped<IFacultyManagerService, FacultyManagerService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IAssignmentService, AssignmentService>();
            builder.Services.AddScoped<IGradeService, GradeService>();

            builder.Services.AddScoped<IApplicantService, ApplicantService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IFacultyService, FacultyService>();
            builder.Services.AddScoped<IStudentService, StudentService>();


            // 3. Comfig Authentication (Cookies & Google)
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/api/auth/logout";
            })
            .AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? throw new InvalidOperationException("Google ClientId is missing.");
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? throw new InvalidOperationException("Google ClientSecret is missing.");
            });


            //4. Config Cloudinary
            var cloudinarySettings = builder.Configuration.GetSection("CloudinarySettings");
            string cloudName = cloudinarySettings["CloudName"];
            string apiKey = cloudinarySettings["ApiKey"];
            string apiSecret = cloudinarySettings["ApiSecret"];

            var cloudinaryAccount = new Account(cloudName, apiKey, apiSecret);
            var cloudinary = new Cloudinary(cloudinaryAccount);
            builder.Services.AddSingleton(cloudinary);




            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Auth/Login");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
