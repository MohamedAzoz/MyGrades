using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyGrades.API.Handlers;
using MyGrades.API.Hubs;
using MyGrades.Application.AutoMapper;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Application.Contracts.Services;
using MyGrades.Application.Helper;
using MyGrades.Application.Services;
using MyGrades.Domain.Entities;
using MyGrades.Infrastructure;
using MyGrades.Infrastructure.Repositories;
using System.Reflection;
using System.Text;

namespace MyGrades.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddSignalR();
            // ===== ( User Handelers Folder ) ======

            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JwtSettings"));

            //************************************************
            //====== ( Add Identity & DbContext ) ========
            builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();


            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //************************************************

            #region Repositories & UnitOfWork 
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IGradeRepository, GradeRepository>();
            builder.Services.AddScoped<IAssistantRepository, AssistantRepository>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            //builder.Services.AddScoped<IAcademicLevelRepository, AcademicLevelRepository>();

            // ==============================================================
            builder.Services.AddScoped<IFlashNotificationService, FlashNotificationService>();
            builder.Services.AddScoped<IGradeService, GradeService>();
            builder.Services.AddScoped<ISubjectService, SubjectService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IAcademicLevelService, AcademicLevelService>();
            builder.Services.AddScoped<IAssistantService, AssistantService>();
            builder.Services.AddScoped<IAuthService, AuthService>();    
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IStudentSubjectService, StudentSubjectService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            #endregion

            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddScoped<ExcelReader>(); 
            builder.Services.AddScoped<ExcelWriter>();

            #region AutoMapper
            builder.Services.AddAutoMapper(typeof(Mapping).Assembly);

            builder.Services.AddAutoMapper(
                typeof(Program).Assembly, // API Assembly
                Assembly.Load("MyGrades.Application") // Core Assembly
            );
            #endregion

            #region Global Handler Exception 

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            #endregion

            #region Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("policy", policy =>
                {
                    policy.WithOrigins(
                        "https://your-angular-production-domain.com",
                        "http://localhost:4200"
                    )
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });
            #endregion

            #region JwtBearer & Authentication

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true, // if false will be public
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            #endregion


            //============================================================================
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            #region Swagger Setting

            builder.Services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation    
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET 8 Web API",
                    Description = " MyGrades Project"
                });
                // To Enable authorization using Swagger (JWT)    
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    }
                    },
                    new string[] {}
                    }
                    });
            });

            #endregion


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}


            //======== ( Global Handler Exception  ) ==============

            app.UseExceptionHandler();
            //*********************************************************
            //======== ( SignalR ) ==============
            app.MapHub<NotificationHub>("/notification");
            //*********************************************************

            //========= ( Policy ) =============

            app.UseCors("policy"); // ==> (2)

            app.UseStaticFiles();//==> (1)

            //*********************************************************

            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllers();

            app.Run();
        }
    }
}
