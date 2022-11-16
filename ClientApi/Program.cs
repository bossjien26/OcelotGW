using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("SecretKey"))),
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
//     .AddJsonFile("Ocelot.json", optional: false, reloadOnChange: true)
//     .AddEnvironmentVariables();
builder.Configuration.AddJsonFile("Ocelot.json", optional: true, reloadOnChange: true);
// builder.Services.AddOcelot(builder.Configuration).AddConsul();
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
}));
// prevent from mapping "sub" claim to nameidentifier.
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// var identityUrl = builder.Configuration.GetValue<string>("IdentityUrl");
// var authenticationProviderKey = "";

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

// }).AddJwtBearer(authenticationProviderKey, options =>
// {
//     options.Authority = identityUrl;
//     options.RequireHttpsMetadata = false;
//     options.Audience = "basket";
// });


// builder.Services.AddOcelot().AddSingletonDefinedAggregator<MyAggregator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHealthChecks("/api/healthz");
// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseOcelot().GetAwaiter().GetResult();
// app.UseRouting();
// app.UseAuthorization();
// app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run();
