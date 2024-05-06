using DotNetHotChocolate;
using DotNetHotChocolate.Model;
using DotNetHotChocolate.Query;
using DotNetHotChocolate.Subscription;
using Microsoft.Net.Http.Headers;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//var signingKey = new SymmetricSecurityKey(
//            Encoding.UTF8.GetBytes("MySuperSecretKey"));

// Add services to the container.

builder.Services.AddSingleton(
  sp =>
  {
    var mongoConnectionURL = new MongoUrl("mongodb://localhost:27017");
    var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionURL);
    var client = new MongoClient(mongoClientSettings);
    var database = client.GetDatabase("Testing");
    return database.GetCollection<SubjectInformation>("Watch");
  })
  .AddGraphQLServer()
  .AddQueryType<InformationQuery>()
  .AddSubscriptionType<Subscription>()
  .AddGlobalObjectIdentification()
  // Registers the filter convention of MongoDB
  .AddMongoDbFiltering()
  // Registers the sorting convention of MongoDB
  .AddMongoDbSorting()
  // Registers the projection convention of MongoDB
  .AddMongoDbProjections()
  // Registers the paging providers of MongoDB
  .AddMongoDbPagingProviders()
  .AddInMemorySubscriptions()
  .SetPagingOptions(new HotChocolate.Types.Pagination.PagingOptions
  {
    IncludeTotalCount = true,
    DefaultPageSize = 100
  });
  //.AddAuthorization();

builder.Services.AddCors(policyBuilder => policyBuilder.AddDefaultPolicy(builder =>
{
  builder.AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials();
}));

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//  .AddJwtBearer(options =>
//  {
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//      ValidIssuer = "",
//      ValidAudience = "",
//      ValidateIssuerSigningKey = true,
//      IssuerSigningKey = signingKey
//    };
//  });

builder.Services.AddAuthorization();

builder.Services.AddHostedService<MongoChangeStreamService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.Build();
app.MapGraphQL();
app.UseCors(policy =>
policy.WithOrigins("https://localhost:7071", "http://localhost:7071", "http://localhost:4200")
.AllowAnyMethod()
.WithHeaders(HeaderNames.ContentType));
//app.UseAuthentication();
//app.UseAuthorization();
app.UseWebSockets();
app.Run();
