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
var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }
app.MapHealthChecks("/api/healthz");

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();
