using BLL.Services.Abstractions;
using BLL.Services.Implementations;
using DAL.Context;
using DAL.Repositories.Implementation;
using DAL.Repositories.Implementation.Definitions;
using DAL.Repositories.Interface;
using DAL.Repositories.Interface.Definitions;
using EMS;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Controllers

builder.Services.AddControllers();

#endregion

#region DbContext

builder.Services.AddDbContext<EmsContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Eipico")));

#endregion

#region Extensions

builder.Services.AddAuth(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerDocumentation();

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

var app = builder.Build();

#region Middleware

app.UseSwaggerUIWithDocs();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();