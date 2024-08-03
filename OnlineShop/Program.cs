using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineShop.Configurations;
using OnlineShop.Contracts;
using OnlineShop.DataModels;
using OnlineShop.Repository;
using System.Security.Claims;
using System.Text;


public class Program
{

    public static async Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        // For Identity  
        builder.Services.AddIdentity<User, IdentityRole>()
                    .AddRoles<IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationDbContext>()
                        .AddDefaultTokenProviders();

        // Adding Authentication  
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
             .AddJwtBearer(options =>
             {
                 options.SaveToken = true;
                 options.RequireHttpsMetadata = false;
                 options.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidAudience = builder.Configuration["JWT:ValidAudience"],
                     ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                     ClockSkew = TimeSpan.Zero,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                 };
             });

        // Add services to the container.
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();

        builder.Services.AddAutoMapper(typeof(MapperConfig));


        builder.Services.AddControllers();
        //Register for swagger Controller
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            //Add Security Defination
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter into field the word 'Bearer' followed by a space of your token",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            //Add Security Requirment
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                Array.Empty<String>()
            }
                });
        });
        builder.Services.AddAuthorization(option =>
        {
            option.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin").RequireRole("Hr"));
            option.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));
            option.AddPolicy("Over21", policy => policy.RequireAssertion(context =>
            context.User.HasClaim(c =>
            c.Type == ClaimTypes.DateOfBirth && DateTime.Parse(c.Value) <= DateTime.Today.AddYears(-21))));
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { "Admin", "Manager", "Member" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        app.Run();
    }

}