using AutoMapper;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OData.Edm;
using Microsoft.OpenApi.Models;
using NetCoreAPI_Template_v2.Data;
using NetCoreAPI_Template_v2.Helpers;
using NetCoreAPI_Template_v2.Models;
using NetCoreAPI_Template_v2.Services;
using NetCoreAPI_Template_v2.Services.Charecter;
using NetCoreAPI_Template_v2.Services.Company;
using NetCoreAPI_Template_v2.Services.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreAPI_Template_v2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddControllers();

            services.AddHttpContextAccessor();
            services.AddResponseCaching();

            //------Allow Origins------
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
                // builder.WithExposedHeaders("totalAmountRecords");
                // builder.WithExposedHeaders("totalAmountPages");
                // builder.WithExposedHeaders("currentPage");
                // builder.WithExposedHeaders("recordsPerPage");
            }));
            //------End: Allow Origins------

            //------HealthChecks------
            services.AddHealthChecks().AddDbContextCheck<AppDBContext>(tags: new[] { "ready" });
            //------End: HealthChecks------

            //------AutoMapper------
            services.AddAutoMapper(typeof(Startup));
            //------End: AutoMapper------

            //------DBContext------
            services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //------End: DBContext------


            services.AddOData();


            //------Swagger------
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "CoreAPI" });

                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                    });
            });
            //------End: Swagger------

            //------Authentication------
            //services.AddAuthentication(config =>
            //{
            //    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(options =>

            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = false,
            //            ValidateAudience = false,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(
            //                Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
            //            ClockSkew = TimeSpan.Zero
            //        }
            //    );
            //------End: Authentication------

            //------Service------
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IcaracterService, CharacterService>();
            services.AddScoped<IProductService,ProductService>();
            services.AddScoped<ICompanyService,CompanyService>();
            services.AddScoped<IBulkService, BuikService>();
            //------End: Service------

            AddFormatters(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //------Swagger------
            app.UseSwagger();

            app.UseSwaggerUI(config => config.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCoreAPI_Template_v2"));
            //------End: Swagger------

            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseResponseCaching();

            //app.UseAuthentication();

            app.UseAuthorization();

            //------Allow Origins------
            app.UseCors("MyPolicy");
            //------End: Allow Origins------

            //------HealthChecks------
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
                {
                    ResponseWriter = HealthCheckResponseWriter.WriteResponseReadiness,
                    Predicate = (check) => check.Tags.Contains("ready")
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    ResponseWriter = HealthCheckResponseWriter.WriteResponseLiveness,
                    Predicate = (check) => !check.Tags.Contains("ready")
                });
            });
            //------End: HealthChecks------

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.EnableDependencyInjection();
                endpoints.Select().Filter().Count().Expand().OrderBy().MaxTop(100);
                endpoints.MapODataRoute(routeName: "api", routePrefix: "api", model: GetEdmModel());
            });
        }

        private IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Bulk>("Bulk");
            return builder.GetEdmModel();
        }

        private void AddFormatters(IServiceCollection services)
        {
            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<OutputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }

                foreach (var inputFormatter in options.InputFormatters.OfType<InputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            }
            );
        }
    }
}