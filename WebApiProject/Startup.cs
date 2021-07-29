using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Linq;
using System.Text;
using WebApiProject.Data;
using WebApiProject.Data.Interfaces;
using WebApiProject.Extensions;
using WebApiProject.Filters;
using WebApiProject.Helpers;

namespace WebApiProject
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
            //var builder = new SqlConnectionStringBuilder(
            //    Configuration.GetConnectionString("Default"));
            //builder.Password = Configuration.GetSection("DBPassword").Value;

            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("LibraryManager")));
            services.AddControllers();
            services.AddCors();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter))).AddFluentValidation();
            services.AddScoped<SeedData>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiProject", Version = "v1" });
            });

            var secretKey = Configuration.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(secretKey));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = key
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SeedData seedData, DataContext dataContext )
        {
            RunMigrations(dataContext);
            app.ConfigureExceptionHandler(env);

            app.UseRouting();
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseCors(m => m.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                seedData.Seed();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LibrayAPI v1"));
            }

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private void RunMigrations(DataContext dataContext)
        {
            var pendingMirgration = dataContext.Database.GetPendingMigrations();
            if (pendingMirgration.Any())
            {
                dataContext.Database.Migrate();
            }
        }
    }
}
