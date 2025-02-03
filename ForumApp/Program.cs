using ForumApp.Data;
using ForumApp.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ForumApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddCors();
            builder.Services.AddAuthentication();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ForumApp API", Version = "v1" });
            });
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseOracle(connectionString,x => x.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19)));
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "ForumApp.AuthCookie";
                options.Cookie.HttpOnly = true;  
                options.ExpireTimeSpan = TimeSpan.FromHours(4);
                options.SlidingExpiration = true;
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                dbContext.Database.EnsureCreated();
            }
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            } else if (app.Environment.IsProduction())
            {
                app.UseMiddleware<ApiKeyMiddleware>();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseCors(options => options.WithOrigins("*").AllowAnyMethod());
            app.MapControllers();
            app.Run();
        }
    }
}
