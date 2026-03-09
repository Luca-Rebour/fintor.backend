
using Application.Interfaces.Services;
using AutoMapper;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Application.MappingProfiles;
using Microsoft.Extensions.Logging;
using Application.Interfaces.Repositories;
using Infrastructure.Repositories;
using Application.Interfaces.UseCases.Users;
using Application.UseCases.Users;
using Api.Middlewares;
using Application.Interfaces.UseCases.Auth;
using Application.UseCases.Auth;
using Application.Interfaces.UseCases.Accounts;
using Application.UseCases.Accounts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Application.Interfaces.UseCases.Transactions;
using Application.UseCases.Transactions;
using Application.Interfaces.UseCases.Categories;
using Application.UseCases.Categories;
using Application.Interfaces.Common;
using Application.Interfaces.UseCases.RecurringTransactions;
using Application.UseCases.RecurringTransactions;
using Application.Interfaces.UseCases.Reports;
using Application.UseCases.Reports;
using Application.Interfaces.UseCases.PendingApproveTransactions;
using Application.UseCases.PendingApprovalTransactions;


namespace Fintor.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ESTO PERMITE USAR JWT EN SWAGGER, NO NECESARIO EN PRODUCCION
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fintor", Version = "v1" });

                // ?? Configurar soporte para JWT
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Ingrese el token JWT en este formato: Bearer {token}"
                });

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
            Array.Empty<string>()
        }
    });
            });

            // Configuracion de CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!)),
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddDbContext<FintorDbContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IPasswordService, PasswordService>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AccountProfile>();
                cfg.AddProfile<CategoryProfile>();
                cfg.AddProfile<TransactionProfile>();
                cfg.AddProfile<RecurringTransactionProfile>();
                cfg.AddProfile<UserProfile>();
            });

            IMapper mapper = mapperConfig.CreateMapper();

            builder.Services.AddSingleton(mapper);

            // Inyeccion de dependencias de Repositories

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IRecurringTransactionRepository, RecurringTransactionRepository>();
            builder.Services.AddScoped<IPendingApprovalTransactionRepository, PendingApprovalTransactionRepository>();


			// Inyeccion de dependencias UseCases de User
			builder.Services.AddScoped<ICreateUser, CreateUser>();
            builder.Services.AddScoped<IMe, Me>();


            // Inyeccion de dependencias UseCases de Auth
            builder.Services.AddScoped<ISignIn, SignIn>();

            // Inyeccion de dependencias UseCases de Account
            builder.Services.AddScoped<ICreateAccount, CreateAccount>();
            builder.Services.AddScoped<IDeleteAccount, DeleteAccount>();
            builder.Services.AddScoped<IGetAllAccounts, GetAllAccounts>();

            // Inyeccion de dependencias UseCases de Transaction
            builder.Services.AddScoped<ICreateTransaction, CreateTransaction>();
            builder.Services.AddScoped<IGetAccountTransactions, GetAccountTransactions>();
            builder.Services.AddScoped<IGetAllTransactions, GetAllTransactions>();
            builder.Services.AddScoped<IDeleteTransaction, DeleteTransaction>();

			// Inyeccion de dependencias UseCases de PendingApproveTransaction
			builder.Services.AddScoped<IApprovePendingApprovalTransaction, ApprovePendingApprovalTransaction>();
			builder.Services.AddScoped<ICancelPendingApprovalTransaction, CancelPendingApprovalTransaction>();
			builder.Services.AddScoped<IGetPendingApprovalTransactions, GetPendingApprovalTransactions>();



			// Inyeccion de dependencias UseCases de Category
			builder.Services.AddScoped<ICreateCategory, CreateCategory>();
            builder.Services.AddScoped<IGetAllCategories, GetAllCategories>();


            // Inyeccion de dependencias UseCases de Report
            builder.Services.AddScoped<IGetOverviewResponse, GetOverviewResponse>();

            // Inyeccion de dependencias UseCases de RecurringTransaction
            builder.Services.AddScoped<IGenerateRecurringTransactions, GenerateRecurringTransaction>();
            builder.Services.AddScoped<ICreateRecurringTransaction, CreateRecurringTransaction>();
            builder.Services.AddScoped<IGetAccountRecurringTransactions, GetAccountRecurringTransactions>();
			builder.Services.AddScoped<IGetRecurringTransactions, GetRecurringTransactions>();


			//Inyeccion de dependencias Services
			builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddHostedService<RecurringTransactionHostedService>();
            builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.WebHost.ConfigureKestrel(opt =>
            {
                opt.ListenAnyIP(7267, lo => lo.UseHttps()); // HTTPS en LAN
                opt.ListenAnyIP(5000);                      // HTTP en LAN
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fintor v1");
                    c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
                });
            }

            app.MapControllers();
            app.Run();
        }
    }
}
