
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodosAPI.Data;
using TodosAPI.Interfaces;
using TodosAPI.Models;
using TodosAPI.Repositories;
using static System.Net.Mime.MediaTypeNames;

namespace TodosAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string msg = "hi";

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddDbContext<TodosDbContext>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<TodosDbContext>();
            
            builder.Services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
            {
                #region SecretKey
                
                byte[] encodedKey = Encoding.ASCII.GetBytes(builder.Configuration["JWT:Key"]);
                SecurityKey securityKey = new SymmetricSecurityKey(encodedKey);
                #endregion

                option.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            builder.Services.AddScoped<ITodoRepository, TodoRepository>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(msg,
                builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors(msg);

            app.MapControllers();

            app.Run();
        }
    }
}