using Microsoft.Azure.Cosmos;
using Tredi.API.DataServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var cosmosClient = new CosmosClient("https://tredi.documents.azure.com:443/", "GtE4gLEtxNzOP2Rajsy4vTcZu2nMVgQUCoP672JpZGOd72cf2dHV3m896qRPg4HNayZN5YBl0tT4ACDbSe1o6g==", new CosmosClientOptions() { ApplicationName = "CosmosDBDotnetQuickstart" });
builder.Services.AddSingleton((s) => { return cosmosClient; });
builder.Services.AddSingleton((s) => { return new DataServiceCollection(cosmosClient); });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(config =>
{
	config.DefaultAuthenticateScheme = "TrediCooking";
	config.DefaultSignInScheme = "TrediCooking";
	config.DefaultChallengeScheme = "TrediServer";

}).AddCookie("TrediCooking", options =>
{
	options.Cookie.Name = "TrediCooking";
});

//.AddOAuth("TrediServer", config =>
//{
//	config.ClientId = "23817346881-igsa29j1f9e1vm128q86kf7ai94ii6tm.apps.googleusercontent.com";
//	config.ClientSecret = "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";
//	config.CallbackPath = "/Authentication/OauthCallback";
//	config.AuthorizationEndpoint = "http://localhost:7171/Authentication/Authorize";
//	config.TokenEndpoint = "http://localhost:7171/Authentication/Token";
//});

//builder.Services.AddAuthentication().AddGoogle(gOptions => { 
//	gOptions.ClientId = "23817346881-igsa29j1f9e1vm128q86kf7ai94ii6tm.apps.googleusercontent.com";
//	gOptions.ClientSecret = "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";
//});

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: "TrediCorsPolicy", builder =>
	{
		builder.WithOrigins("http://localhost:4200").AllowAnyHeader().WithMethods("GET", "POST").AllowCredentials();
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
app.UseCors("TrediCorsPolicy");
app.UseAuthorization();
app.MapControllers();
app.Run();