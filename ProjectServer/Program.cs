using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
// var authenticationProviderKey = "IdentityApiKey";
// var identityUrl = builder.Configuration.GetValue<string>("IdentityUrl");

// builder.Services.AddAuthentication()
// .AddJwtBearer(authenticationProviderKey, x =>
// {
//     x.Authority = identityUrl;
//     x.RequireHttpsMetadata = false;
//     x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
//     {
//         ValidAudiences = new[] { "orders", "basket", "locations", "marketing", "mobileshoppingagg", "webshoppingagg" }
//     };
// });
//...
// var identityUrl = builder.Configuration.GetValue<string>("IdentityUrl");
// var authenticationProviderKey = "IdentityApiKey";
// //…
// builder.Services.AddAuthentication()
//     .AddJwtBearer(authenticationProviderKey, x =>
//     {
//         x.Authority = identityUrl;
//         x.RequireHttpsMetadata = false;
//         x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
//         {
//             ValidAudiences = new[] { "orders", "basket", "locations", "marketing", "mobileshoppingagg", "webshoppingagg" }
//         };
//     });
// var identityUrl = builder.Configuration.GetValue<string>("IdentityUrl");
// var authenticationProviderKey = "IdentityApiKey";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("SecretKey"))), // Key
        ValidateIssuer = true,
        ValidIssuer = "CashUser", // 簽發者
        ValidateAudience = true,
        ValidAudience = "CashAudience", // 接收者
        ValidateLifetime = true, // 驗證時間
        RequireExpirationTime = true, // 是不是有過期時間
        ClockSkew = TimeSpan.Zero // 時間偏移
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }
app.MapHealthChecks("/api/healthz");

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
// app.UseAuthorization();
// app.UseHttpsRedirection();
app.MapControllers();

app.Run();
