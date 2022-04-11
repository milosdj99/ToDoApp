using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;
using System.Text.Json.Serialization;
using ToDoApi.Authentication;
using ToDoApi.ToDoServices;
using ToDoInfrastructure;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddDbContext<ToDoDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ToDo")));

builder.Services.AddScoped<ToDoService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();


builder.Services.AddControllers().AddJsonOptions(x =>
               x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://dev-ly15t0g7.us.auth0.com/";
        options.Audience = "https://localhost:7117/api";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });


string domain = "https://dev-ly15t0g7.us.auth0.com/";

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("get:lists", policy => policy.Requirements.Add(new HasScopeRequirement("get:lists", domain)));
    options.AddPolicy("get:list", policy => policy.Requirements.Add(new HasScopeRequirement("get:list", domain)));
    options.AddPolicy("search:lists", policy => policy.Requirements.Add(new HasScopeRequirement("search:lists", domain)));
    options.AddPolicy("add:list", policy => policy.Requirements.Add(new HasScopeRequirement("add:list", domain)));
    options.AddPolicy("modify:list", policy => policy.Requirements.Add(new HasScopeRequirement("modify:list", domain)));
    options.AddPolicy("get:item", policy => policy.Requirements.Add(new HasScopeRequirement("get:item", domain)));
    options.AddPolicy("add:item", policy => policy.Requirements.Add(new HasScopeRequirement("add:item", domain)));
    options.AddPolicy("modify:item", policy => policy.Requirements.Add(new HasScopeRequirement("modify:item", domain)));
    options.AddPolicy("delete:list", policy => policy.Requirements.Add(new HasScopeRequirement("delete:list", domain)));
    options.AddPolicy("delete:item", policy => policy.Requirements.Add(new HasScopeRequirement("delete:item", domain)));
    options.AddPolicy("modify:list-position", policy => policy.Requirements.Add(new HasScopeRequirement("modify:list-position", domain)));
    options.AddPolicy("modify:item-position", policy => policy.Requirements.Add(new HasScopeRequirement("modify:item-position", domain)));
});


builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();


//builder.Services.AddHostedService<RemainderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthentication();

app.UseCors(MyAllowSpecificOrigins);

app.UseMvc();

app.UseHttpsRedirection();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "My API V1");
});


using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
    dataContext.Database.Migrate();
}

Log.Debug("Application started!");

app.Run();
