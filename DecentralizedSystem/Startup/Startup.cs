using DecentralizedSystem.Extensions;
using DecentralizedSystem.Helpers;
using DecentralizedSystem.Infrastructure;
using DecentralizedSystem.Persistence;
using DecentralizedSystem.Services.Account;
using DecentralizedSystem.Services.IdmProjects;
using DecentralizedSystem.Services.Logger;
using DecentralizedSystem.Services.MasterPackages;
using DecentralizedSystem.Services.SOA;
using DecentralizedSystem.Services.UserInfos;
using DecentralizedSystem.Services.UserManagement;
using DecentralizedSystem.Services.CourseManagement;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DecentralizedSystem.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            _configuration = configuration;
            Helpers.Utility.RegisterTypeMaps();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DecentralizedSystem API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
                //includes xml file to show description on swagger UI
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                }

                c.OperationFilter<RemoveVersionParameterFilter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
            });
            services.AddSwaggerGenNewtonsoftSupport(); // explicit opt-in - needs to be placed after AddSwaggerGen()

            // Other code omitted
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });
            // Lowercase urls 
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            //Restricting Media Types in Content Negotiation
            services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson();

            //services.AddAntiforgery(options =>
            //{
            //    options.HeaderName = "X-XSRF-TOKEN";
            //    options.Cookie.Name = "XSRF-TOKEN";
            //    options.Cookie.HttpOnly = false;
            //});

            AuthConfigurer.Configure(services, _configuration);

            //This config for allow localhost connect. For development stage only, remove when deploy production
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            this._configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                //.Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            services.AddAutoMapper(typeof(Startup));

            services.AddDataProtection();

            // Inject this dict
            services.AddInfrastructure(this._configuration);
            services.AddSingleton<ILoggerManager, LoggerManager>();


            // Register your regular repositories
            services.AddScoped<ModelValidationAttribute>();
            services.AddSingleton<IRSAHelper, RSAHelper>();
            services.AddSingleton<IAESHelper, AESHelper>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISOAService, SOAService>();
            services.AddScoped<IAccountTranFccService, AccountTranFccService>();
            services.AddScoped<IAccountClassFccService, AccountClassFccService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IUserInfoService, UserInfoService>();

            services.AddScoped<IMasterPackageServices, MasterPackageServices>();
            services.AddScoped<IIdmProjectService, IdmProjectService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<ICourseManagementService, CourseManagementService>();
            services.AddScoped<ICourseLikeService, CourseLikeService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "My API V1");
                });
            }

            app.UseMiddleware<ExceptionMiddleware>();
            //app.UseMiddleware<ValidateAntiForgeryTokenMiddleware>();
            app.UseMiddleware<EncryptionMiddleware>();
            app.UseMiddleware<AuthMiddleware>();

            // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(_defaultCorsPolicyName);// Enable CORS!

            app.UseAuthentication();
            app.UseAuthorization();

            // Configure the Localization middleware
            var cultureInfo = new CultureInfo("en-US");
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(cultureInfo),
                SupportedCultures = new List<CultureInfo>
                {
                    cultureInfo,
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    cultureInfo,
                }
            });

            //This config for allow localhost connect. For development stage only, remove when deploy production

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
