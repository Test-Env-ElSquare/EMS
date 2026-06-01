using BLL.Services.Abstractions;
using BLL.Services.Implementations;
using BLL.ExternalServices.EmailManagements;
using DAL.Context;
using DAL.Models.Identity;
using DAL.Repositories.Implementation;
using DAL.Repositories.Implementation.Definitions;
using DAL.Repositories.Interface;
using DAL.Repositories.Interface.Definitions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Controllers + Swagger

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

#region DbContext

builder.Services.AddDbContext<EmsContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Eipico")));

#endregion

#region Identity

builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<EmsContext>()
    .AddDefaultTokenProviders();

#endregion

#region Repositories & UnitOfWork

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IFactoryRepository, FactoryRepository>();
builder.Services.AddScoped<ILineRepository, LineRepository>();
builder.Services.AddScoped<ILineTransformerRepository, LineTransformerRepository>();
builder.Services.AddScoped<ITransformerRepository, TransformerRepository>();

#endregion

#region Services

builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IDashboradRepository, DashboardRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();

#endregion

#region Email

builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

#endregion

var app = builder.Build();

#region Middleware Pipeline

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();