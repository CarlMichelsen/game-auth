using GameAuth.Database.DataContexts;
using GameAuth.Database.Repository.Interface;
using GameAuth.Database.Repository;

using GameAuth.Email.Configuration;
using GameAuth.Email.Service.Interface;
using GameAuth.Email.Service;

using GameAuth.Api.Services;
using GameAuth.Api.Services.Interface;
using GameAuth.Api.Validators;
using GameAuth.Api.Configuration;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuration
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("GameAuth.Api")));

builder.Services.AddConfigurationSingleton<IJwtConfiguration, AppConfiguration>(builder.Configuration);
builder.Services.AddConfigurationSingleton<IEmailConfiguration, AppConfiguration>(builder.Configuration);

// Repositories
builder.Services
    .AddTransient<IBanRepository, BanRepository>()
    .AddTransient<IEmailRepository, EmailRepository>()
    .AddTransient<IAccountRepository, AccountRepository>();

// Services
builder.Services
    .AddTransient<IRegisterService, RegisterService>()
    .AddTransient<ILoginService, LoginService>()
    .AddTransient<IRefreshService, RefreshService>()
    .AddTransient<IHashingService, HashingService>()
    .AddTransient<IAccessControlService, AccessControlService>()
    .AddTransient<IEmailService, EmailService>()
    .AddTransient<IJwtService, JwtService>();

// Validators
builder.Services.AddTransient<IAccountValidator, AccountValidator>();

// HttpClientFactories
builder.Services.AddHttpClient<IEmailService, EmailService>(client =>
{
    client.BaseAddress = new Uri("https://api.mailgun.net");
    var authBytes = Encoding.UTF8.GetBytes($"api:{builder.Configuration["MailGun:ApiKey"]}");
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));
});

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var key = builder.Configuration["Jwt:RefreshSecret"] ?? throw new NullReferenceException("Jwt:RefreshSecret");
    var byteKey = Encoding.UTF8.GetBytes(key);
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(byteKey),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
